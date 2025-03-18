using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using Firebase.Database;

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
                Debug.Log("파이어베이스 문제있음");               
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
            Debug.Log("로그인 완료." + user.DisplayName + " 님 반갑습니다.");
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

                        Debug.Log("생성 완료. " + user.DisplayName + " 님 반갑습니다.");
                    }
                }
            }
        }
    }
}
