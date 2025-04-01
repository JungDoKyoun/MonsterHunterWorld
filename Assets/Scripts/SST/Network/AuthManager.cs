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
                Debug.Log("파이어베이스 문제있음");
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
                    message = "이메일 누락";
                    break;
                case AuthError.MissingPassword:
                    message = "비밀번호 누락";
                    break;
                case AuthError.WrongPassword:
                    message = "비밀번호 틀림";
                    break;
                case AuthError.UserNotFound:
                    message = "아이디가 존재하지 않습니다";
                    break;
                case AuthError.InvalidEmail:
                    message = "이메일 형태가 맞지 않습니다";
                    break;
                default:
                    message = "기타 사유. 관리자에게 문의 바랍니다";
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
                    Debug.Log("로드 취소");
#endif
                }
                else if (task.IsFaulted)
                {
#if UNITY_EDITOR
                    Debug.Log("로드 실패");
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
                                Debug.Log("데이터 저장 완료!");

#endif
                            }
                        });
                    }
                    PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
                }
            });
            // 로그인 성공 후 Photon 네트워크에 연결 시작
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
            warningText.text = "닉네임 미기입";
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
                        message = "이메일 누락";
                        break;
                    case AuthError.MissingPassword:
                        message = "비밀번호 누락";
                        break;
                    case AuthError.WeakPassword:
                        message = "비밀번호 보안 약함";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "중복된 이메일 입니다";
                        break;
                    default:
                        message = "기타 사유. 관리자에게 문의해주세요";
                        break;
                }

                warningText.text = message;
            }
            else
            {
                user = registerTask.Result.User;

                // 계정 생성 후 user 객체가 유효하면, 사용자 프로필을 업데이트
                if(user != null)
                {
                    // 파이어베이스 UserProfile 객체 생성 후 유저 닉네임 설정
                    UserProfile userProfile = new UserProfile { DisplayName = userName };

                    // 사용자 프로필 업데이트 메서드 호출 (Firebase에 사용자 정보 저장)
                    var profileTask = user.UpdateUserProfileAsync(userProfile);

                    yield return new WaitUntil(predicate: () => profileTask.IsCompleted);

                    if(profileTask.Exception != null)
                    {
                        FirebaseException firebaseEX = profileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEX.ErrorCode;

                        warningText.text = "닉네임 사용 실패";
                    }
                    else
                    {
                        warningText.text = "";
#if UNITY_EDITOR
                        Debug.Log("생성 완료. " + user.DisplayName + " 님 반갑습니다.");
                        
#endif
                    }
                }
            }
        }
    }
}
