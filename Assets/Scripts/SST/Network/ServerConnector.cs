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

    public override void OnConnectedToMaster()
    {
        Debug.Log("서버 연결 완료");

        if(AuthManager.user != null)
        {
            PhotonNetwork.NickName = AuthManager.user.DisplayName;
        }
        else
        {
            Debug.LogError("AuthManager.user 가 null 입니다");
            PhotonNetwork.NickName = "Guest";
        }

        // 싱글플레이 모드용 방 "SingleRoom" 으로 입장
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 1 };
        PhotonNetwork.JoinOrCreateRoom("SingleRoom", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("싱글룸에 입장하였습니다.");

        // 싱글플레이 씬으로 전환
        if(SceneManager.GetActiveScene().name != "SingleRoom")
        {
            SceneManager.LoadScene("SingleRoom");
        }
    }
}
