using UnityEngine;
using UnityEngine.UI;

public class FirebaseManager : MonoBehaviour
{
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
}