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

        // �̱��÷��� ���� �� "SingleRoom" ���� ����
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 1 };
        PhotonNetwork.JoinOrCreateRoom("SingleRoom", roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("�̱۷뿡 �����Ͽ����ϴ�.");

        // �̱��÷��� ������ ��ȯ
        if(SceneManager.GetActiveScene().name != "SingleRoom")
        {
            SceneManager.LoadScene("SingleRoom");
        }
    }
}
