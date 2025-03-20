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

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� �����Ͽ����ϴ�");

        SceneManager.LoadScene("LobbyScene");

        //PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("���� ��(��ȸ��) ���� ����. ���ο� ��(��ȸ��) ������..");

        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("��(��ȸ��)�� �����Ͽ����ϴ�");

        SceneManager.LoadScene("LobbyScene");
    }
}
