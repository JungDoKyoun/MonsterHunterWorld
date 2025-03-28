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
        // ������ �� �ѱ�� �� ���� �Ѿ
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("���� ���� �Ϸ�");

        if(AuthManager.user != null)
        {
            PhotonNetwork.NickName = AuthManager.user.DisplayName;
        }
        else
        {
            Debug.LogError("AuthManager.user �� null �Դϴ�");
            PhotonNetwork.NickName = "Guest";
        }

        LoadingSceneManager.LoadSceneWithLoading("SingleRoom", singleRoomOption);
    }
}
