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
        // 방장이 씬 넘기면 다 같이 넘어감
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 연결 완료");

        PhotonNetwork.NickName = AuthManager.user.DisplayName;

        PhotonNetwork.JoinLobby();       
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비에 입장하였습니다");

        SceneManager.LoadScene("LobbyScene");

        //PhotonNetwork.JoinRandomRoom();
    }
}
