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
    
    // �̸��� InputField
    [SerializeField]
    TMPro.TMP_InputField emailInput;
    //InputField emailInput;
    // ��й�ȣ InputField
    [SerializeField]
    TMPro.TMP_InputField passInput;
    //InputField ;
    // ����� �˷��� �ؽ�Ʈ
    //[SerializeField]
    //Text resultText;

    // ������ ������ ��ü
    Firebase.Auth.FirebaseAuth auth;

    // Use this for initialization
    void Awake()
    {
        // ������ ������ ��ü�� �ʱ�ȭ �Ѵ�.
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    // ȸ������ ��ư�� ������ �� �۵��� �Լ�
    public void SignUp()
    {
        // ȸ������ ��ư�� ��ǲ �ʵ尡 ������� ���� �� �۵��Ѵ�.
        if (emailInput.text.Length != 0 && passInput.text.Length != 0)
        {
            auth.CreateUserWithEmailAndPasswordAsync(emailInput.text, passInput.text).ContinueWith(
                task =>
                {
                    if (!task.IsCanceled && !task.IsFaulted)
                    {
                        Debug.Log("ȸ������ ����");
                        //resultText.text = "ȸ������ ����";
                    }
                    else
                    {
                        Debug.Log("ȸ������ ����");
                        //resultText.text = "ȸ������ ����";
                    }
                });
        }
    }

    // �α��� ��ư�� ������ �� �۵��� �Լ�
    public void SignIn()
    {
        // �α��� ��ư�� ��ǲ �ʵ尡 ������� ���� �� �۵��Ѵ�.
        if (emailInput.text.Length != 0 && passInput.text.Length != 0)
        {
            auth.SignInWithEmailAndPasswordAsync(emailInput.text, passInput.text).ContinueWith(
                task =>
                {
                    if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
                    {
                        Firebase.Auth.FirebaseUser newUser = task.Result;
                        Debug.Log("�α��� ����");
                        //resultText.text = "�α��� ����";
                    }
                    else
                    {
                        Debug.Log("�α��� ����");
                        //resultText.text = "�α��� ����";
                    }
                });
        }
    }
    
    #endregion
}