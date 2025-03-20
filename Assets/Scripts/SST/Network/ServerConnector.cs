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

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비에 입장하였습니다");

        SceneManager.LoadScene("LobbyScene");

        //PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("랜덤 룸(집회소) 입장 실패. 새로운 룸(집회소) 생성중..");

        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("룸(집회소)에 입장하였습니다");

        SceneManager.LoadScene("LobbyScene");
    }
}
