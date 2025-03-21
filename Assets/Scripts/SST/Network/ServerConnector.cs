using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using Photon.Pun.Demo.Asteroids;

public class ServerConnector : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        // ������ �� �ѱ�� �� ���� �Ѿ
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("���� ���� �Ϸ�");

        PhotonNetwork.NickName = AuthManager.user.DisplayName;

        PhotonNetwork.JoinLobby();       
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� �����Ͽ����ϴ�");

        SceneManager.LoadScene("LobbyScene");

        //PhotonNetwork.JoinRandomRoom();
    }
}
