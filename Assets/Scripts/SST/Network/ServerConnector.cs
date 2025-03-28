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
    RoomOptions singleRoomOption = new RoomOptions { MaxPlayers = 1};

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

        LoadingSceneManager.LoadSceneWithLoading("SingleRoom", singleRoomOption);
    }
}
