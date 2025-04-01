using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Text;

public class AuthManager : MonoBehaviour
{
    public FirebaseAuth auth;
    public static FirebaseUser user;
    public static DatabaseReference dbRef;

    public InputField emailField;
    public InputField passwordField;
    public InputField nickField;

    public Text warningText;

    private void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;

            if(dependencyStatus == Firebase.DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                dbRef = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("���̾�̽� ��������");
#endif
            }
        });
    }

    private void Start()
    {
        warningText.text = "";
    }
    
    public void LogIn()
    {
        StartCoroutine(LogInCor(emailField.text, passwordField.text));
    }

    IEnumerator LogInCor(string email, string pw)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, pw);

        yield return new WaitUntil(predicate: () =>  loginTask.IsCompleted);

        if(loginTask.Exception != null)
        {
            FirebaseException firebaseEX = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEX.ErrorCode;

            string message = "";

            switch(errorCode)
            {
                case AuthError.MissingEmail:
                    message = "�̸��� ����";
                    break;
                case AuthError.MissingPassword:
                    message = "��й�ȣ ����";
                    break;
                case AuthError.WrongPassword:
                    message = "��й�ȣ Ʋ��";
                    break;
                case AuthError.UserNotFound:
                    message = "���̵� �������� �ʽ��ϴ�";
                    break;
                case AuthError.InvalidEmail:
                    message = "�̸��� ���°� ���� �ʽ��ϴ�";
                    break;
                default:
                    message = "��Ÿ ����. �����ڿ��� ���� �ٶ��ϴ�";
                    break;
            }

            warningText.text = message;
        }
        else
        {
            warningText.text = "";

            user = loginTask.Result.User;
            nickField.text = user.DisplayName;

            dbRef.Child(user.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
#if UNITY_EDITOR
                    Debug.Log("�ε� ���");
#endif
                }
                else if (task.IsFaulted)
                {
#if UNITY_EDITOR
                    Debug.Log("�ε� ����");
#endif
                }
                else
                {
                    bool[] items = new bool[6];
                    DataSnapshot dataSnapshot = task.Result;
                    ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
                    foreach (DataSnapshot child in dataSnapshot.Children)
                    {
                        switch(child.Key)
                        {
                            case PlayerCostume.WeaponTag:
                                if(child.Value != null && int.TryParse(child.Value.ToString(), out int weapon) == true)
                                {
                                    hashtable.Add(child.Key, weapon);
                                    items[0] = true;
                                }
                                break;
                            case PlayerCostume.HandTag:
                                if (child.Value != null && int.TryParse(child.Value.ToString(), out int hand) == true)
                                {
                                    hashtable.Add(child.Key, hand);
                                    items[1] = true;
                                }
                                break;
                            case PlayerCostume.BreastTag:
                                if (child.Value != null && int.TryParse(child.Value.ToString(), out int breast) == true)
                                {
                                    hashtable.Add(child.Key, breast);
                                    items[2] = true;
                                }
                                break;
                            case PlayerCostume.HeadTag:
                                if (child.Value != null && int.TryParse(child.Value.ToString(), out int head) == true)
                                {
                                    hashtable.Add(child.Key, head);
                                    items[3] = true;
                                }
                                break;
                            case PlayerCostume.LegTag:
                                if (child.Value != null && int.TryParse(child.Value.ToString(), out int leg) == true)
                                {
                                    hashtable.Add(child.Key, leg);
                                    items[4] = true;
                                }
                                break;
                            case PlayerCostume.WaistTag:
                                if (child.Value != null && int.TryParse(child.Value.ToString(), out int waist) == true)
                                {
                                    hashtable.Add(child.Key, waist);
                                    items[5] = true;
                                }
                                break;
                        }
                    }
                    Dictionary<string, int> data = new Dictionary<string, int>();
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (items[i] == false)
                        {
                            switch(i)
                            {
                                case 0:
                                    data.Add(PlayerCostume.WeaponTag, 0);
                                    hashtable.Add(PlayerCostume.WeaponTag, 0);
                                    break;
                                case 1:
                                    data.Add(PlayerCostume.HandTag, 0);
                                    hashtable.Add(PlayerCostume.HandTag, 0);
                                    break;
                                case 2:
                                    data.Add(PlayerCostume.BreastTag, 0);
                                    hashtable.Add(PlayerCostume.BreastTag, 0);
                                    break;
                                case 3:
                                    data.Add(PlayerCostume.HeadTag, 0);
                                    hashtable.Add(PlayerCostume.HeadTag, 0);
                                    break;
                                case 4:
                                    data.Add(PlayerCostume.LegTag, 0);
                                    hashtable.Add(PlayerCostume.LegTag, 0);
                                    break;
                                case 5:
                                    data.Add(PlayerCostume.WaistTag, 0);
                                    hashtable.Add(PlayerCostume.WaistTag, 0);
                                    break;
                            }
                        }
                    }
                    if(data.Count > 0)
                    {
                        dbRef.Child(user.UserId).SetValueAsync(data).ContinueWith(task =>
                        {
                            if (task.IsCompleted)
                            {
#if UNITY_EDITOR
                                Debug.Log("������ ���� �Ϸ�!");

#endif
                            }
                        });

                    }

                    //TODO: �κ� �����ؼ� �������ֱ�
                    PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
                }
            });
            // �α��� ���� �� Photon ��Ʈ��ũ�� ���� ����
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void Register()
    {
        StartCoroutine(RegisterCor(emailField.text, passwordField.text, nickField.text));
    }

    IEnumerator RegisterCor(string email, string pw, string userName)
    {
        if(userName == "")
        {
            warningText.text = "�г��� �̱���";
        }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, pw);

            yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

            if(registerTask.Exception != null)
            {
                FirebaseException firebaseEX = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEX.ErrorCode;

                string message = "";

                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "�̸��� ����";
                        break;
                    case AuthError.MissingPassword:
                        message = "��й�ȣ ����";
                        break;
                    case AuthError.WeakPassword:
                        message = "��й�ȣ ���� ����";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "�ߺ��� �̸��� �Դϴ�";
                        break;
                    default:
                        message = "��Ÿ ����. �����ڿ��� �������ּ���";
                        break;
                }

                warningText.text = message;
            }
            else
            {
                user = registerTask.Result.User;

                // ���� ���� �� user ��ü�� ��ȿ�ϸ�, ����� �������� ������Ʈ
                if(user != null)
                {
                    // ���̾�̽� UserProfile ��ü ���� �� ���� �г��� ����
                    UserProfile userProfile = new UserProfile { DisplayName = userName };

                    // ����� ������ ������Ʈ �޼��� ȣ�� (Firebase�� ����� ���� ����)
                    var profileTask = user.UpdateUserProfileAsync(userProfile);

                    yield return new WaitUntil(predicate: () => profileTask.IsCompleted);

                    if(profileTask.Exception != null)
                    {
                        FirebaseException firebaseEX = profileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEX.ErrorCode;

                        warningText.text = "�г��� ��� ����";
                    }
                    else
                    {
                        warningText.text = "";
#if UNITY_EDITOR
                        Debug.Log("���� �Ϸ�. " + user.DisplayName + " �� �ݰ����ϴ�.");

                        // ���� ���� �� �ʱ� �κ��丮 ����
                        InitInventoryForNewUser();
#endif
                    }
                }
            }
        }
    }

    void InitInventoryForNewUser()
    {
        var user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user == null) return;

        // �ʱ� �κ��丮 ����
        List<BaseItem> inventory = new List<BaseItem>();
        List<BaseItem> boxInven = new List<BaseItem>();
        List<BaseItem> equipInventory = new List<BaseItem>();
        List<BaseItem> equippedInventory = new List<BaseItem>(new BaseItem[9]);

        BaseItem empty = ItemDataBase.Instance.emptyItem;

        // �ʱ� �κ��丮
        for (int i = 0; i < 24; i++)
            inventory.Add(empty);

        // â�� �ʱ� ������: ���� 10��,��� 10��, ���� 3��
        for (int i = 0; i < 100; i++)
            boxInven.Add(empty);

        boxInven[0] = ItemDataBase.Instance.GetItem(ItemName.Potion).Clone();
        boxInven[0].count = 10;
        boxInven[1] = ItemDataBase.Instance.GetItem(ItemName.WellDoneSteak).Clone();
        boxInven[1].count = 10;
        boxInven[2] = ItemDataBase.Instance.GetItem(ItemName.PitfallTrap).Clone();
        boxInven[2].count = 3;

        // ��� �κ��丮 
        for (int i = 0; i < 50; i++)
            equipInventory.Add(empty);


        // ���� ���� (���� ���� ���� ������ �߰� �� ����)
        for (int i = 0; i < 9; i++)
            equippedInventory[i] = empty;

        equippedInventory[(int)EquipSlot.Weapon] = ItemDataBase.Instance.GetItem(ItemName.HuntersKnife_I).Clone();
        equippedInventory[(int)EquipSlot.Head] = ItemDataBase.Instance.GetItem(ItemName.LeatherHead).Clone();
        equippedInventory[(int)EquipSlot.Chest] = ItemDataBase.Instance.GetItem(ItemName.LeatherVest).Clone();
        equippedInventory[(int)EquipSlot.Arms] = ItemDataBase.Instance.GetItem(ItemName.LeatherGloves).Clone();
        equippedInventory[(int)EquipSlot.Waist] = ItemDataBase.Instance.GetItem(ItemName.LeatherBelt).Clone();
        equippedInventory[(int)EquipSlot.Legs] = ItemDataBase.Instance.GetItem(ItemName.LeatherPants).Clone();

        // ����
        StringBuilder sb = new StringBuilder();
        void SaveList(string key, List<BaseItem> list)
        {
            sb.Clear();
            foreach (var item in list)
            {
                sb.AppendLine($"{(int)item.id},{item.count}");
            }

            FirebaseDatabase.DefaultInstance.RootReference
                .Child(user.UserId)
                .Child(key)
                .SetValueAsync(sb.ToString());
        }

        SaveList("inventoryData", inventory);
        SaveList("BoxInvenData", boxInven);
        SaveList("EquipInventoryData", equipInventory);
        SaveList("EquippedInventoryData", equippedInventory);

        // ���Ժ� ���� (����/��)
        FirebaseDatabase.DefaultInstance.RootReference.Child(user.UserId).Child("Weapon").SetValueAsync((int)ItemName.HuntersKnife_I);
        FirebaseDatabase.DefaultInstance.RootReference.Child(user.UserId).Child("Breast").SetValueAsync((int)ItemName.LeatherVest);
        FirebaseDatabase.DefaultInstance.RootReference.Child(user.UserId).Child("Head").SetValueAsync((int)ItemName.LeatherHead);
        FirebaseDatabase.DefaultInstance.RootReference.Child(user.UserId).Child("Hand").SetValueAsync((int)ItemName.LeatherGloves);
        FirebaseDatabase.DefaultInstance.RootReference.Child(user.UserId).Child("Waist").SetValueAsync((int)ItemName.LeatherBelt);
        FirebaseDatabase.DefaultInstance.RootReference.Child(user.UserId).Child("Leg").SetValueAsync((int)ItemName.LeatherPants);


        Debug.Log("�ʱ� �κ��丮 �� ��� ���� �Ϸ�");
    }

}
