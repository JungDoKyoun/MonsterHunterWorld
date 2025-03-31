using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using Firebase.Database;
using Photon.Pun;
using Firebase.Extensions;

[Serializable]
public class Data
{
    public int Equipment;

    public Data(int index)
    {
        this.Equipment = index;
    }
}

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
                Debug.Log("���̾�̽� ��������");               
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
            Debug.Log("�α��� �Ϸ�." + user.DisplayName + " �� �ݰ����ϴ�.");

            string userEmail = user.Email.Trim();
            int index = 0;
            Data playerData = new Data(index);

            string JsonData = JsonUtility.ToJson(playerData);

            dbRef.Child(user.UserId).SetRawJsonValueAsync(JsonData);

            dbRef.Child(user.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.Log("�ε� ���");
                }
                else if (task.IsFaulted)
                {
                    Debug.Log("�ε� ����");
                }
                else
                {
                    DataSnapshot dataSnapshot = task.Result;
                    string key = dataSnapshot.Child("Equipment").Key;
                    int value = 0;
                    //int value = (int)dataSnapshot.Child("Equipment").Value;

                    PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable()
                    {
                        {key, value}
                    });
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

                        Debug.Log("���� �Ϸ�. " + user.DisplayName + " �� �ݰ����ϴ�.");
                    }
                }
            }
        }
    }
}
