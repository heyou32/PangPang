using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    #region Working on Retro Youtube
    /*
    public bool IsFirebaseReady { get; private set; }
    public bool IsSignInOnProgress { get; private set; }

    public TMPro.TMP_InputField emailField;
    public TMPro.TMP_InputField passwordField;
    //public InputField usernameField;
    public Button signInButton;
    public Button signOnButton;
    //public Button signOutButton;//**

    public static FirebaseApp firebaseApp;
    public static FirebaseAuth firebaseAuth;

    public static FirebaseUser User;

    public void Start()
    {
        signInButton.interactable = false;
        //signOutButton.interactable = false; //**

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var result = task.Result;

                if (result != DependencyStatus.Available)
                {
                    Debug.LogError(message: result.ToString());
                    IsFirebaseReady = false;
                }
                else
                {
                    IsSignInOnProgress = true;

                    firebaseApp = FirebaseApp.DefaultInstance;
                    firebaseAuth = FirebaseAuth.DefaultInstance;
                }

                signInButton.interactable = IsFirebaseReady;
            }
        );
    }

    public void SignIn()
    {
        if(!IsFirebaseReady || IsSignInOnProgress || User != null)
        {
            return;
        }

        IsSignInOnProgress = true;
        signInButton.interactable= false;

        firebaseAuth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWithOnMainThread(continuation: (task) =>
        {
            Debug.Log(message: $"Sign in status : {task.Status}");

            IsSignInOnProgress = false;
            signInButton.interactable = true;

            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if (task.IsCanceled)
            {
                Debug.LogError(message: "Sign-in cancled");
            }
            else
            {
                User = task.Result;
                Debug.Log(User.Email);
                SceneManager.LoadScene("Lobby");
            }
        });
    }
    */
    #endregion

    #region Old Version FirebaseManager
    
    // 이메일 InputField
    [SerializeField]
    TMPro.TMP_InputField emailInput;
    //InputField emailInput;
    // 비밀번호 InputField
    [SerializeField]
    TMPro.TMP_InputField passInput;
    //InputField ;
    // 결과를 알려줄 텍스트
    //[SerializeField]
    //Text resultText;

    // 인증을 관리할 객체
    Firebase.Auth.FirebaseAuth auth;

    // Use this for initialization
    void Awake()
    {
        // 인증을 관리할 객체를 초기화 한다.
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    // 회원가입 버튼을 눌렀을 때 작동할 함수
    public void SignUp()
    {
        // 회원가입 버튼은 인풋 필드가 비어있지 않을 때 작동한다.
        if (emailInput.text.Length != 0 && passInput.text.Length != 0)
        {
            auth.CreateUserWithEmailAndPasswordAsync(emailInput.text, passInput.text).ContinueWith(
                task =>
                {
                    if (!task.IsCanceled && !task.IsFaulted)
                    {
                        Debug.Log("회원가입 성공");
                        //resultText.text = "회원가입 성공";
                    }
                    else
                    {
                        Debug.Log("회원가입 실패");
                        //resultText.text = "회원가입 실패";
                    }
                });
        }
    }

    // 로그인 버튼을 눌렀을 때 작동할 함수
    public void SignIn()
    {
        // 로그인 버튼은 인풋 필드가 비어있지 않을 때 작동한다.
        if (emailInput.text.Length != 0 && passInput.text.Length != 0)
        {
            auth.SignInWithEmailAndPasswordAsync(emailInput.text, passInput.text).ContinueWith(
                task =>
                {
                    if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
                    {
                        Firebase.Auth.FirebaseUser newUser = task.Result;
                        Debug.Log("로그인 성공");
                        //resultText.text = "로그인 성공";
                    }
                    else
                    {
                        Debug.Log("로그인 실패");
                        //resultText.text = "로그인 실패";
                    }
                });
        }
    }
    
    #endregion
}