using Photon.Pun;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public Text ConnectionStatus;
    public Text IDtext;
    public Button connetBtn;

    //void Start()
    //{

    //}

    void Update()
    {
        ConnectionStatus.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    // Update is called once per frame
    public override void OnConnectedToMaster()
    {
        print("서버 접속 완료");
        PhotonNetwork.LocalPlayer.NickName = IDtext.text;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("로비 접속 완료");
    }
}