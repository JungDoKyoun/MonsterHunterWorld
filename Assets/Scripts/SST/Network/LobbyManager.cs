using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] Text roomName;
    [SerializeField] GameObject roomPrefab;
    [SerializeField] Transform roomListPanel;
    
    private void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    private void Update()
    {
        
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(roomName.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName.text);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� �����Ͽ����ϴ�");
    }

    public override void OnLeftLobby()
    {
        Debug.Log("�κ񿡼� �������ϴ�");
    }

    // �� �κ� ����� �˾Ƽ� �ݹ� ȣ���
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // �� ��� �� ����Ʈ ������ �ݺ����� ����
        foreach (RoomInfo room in roomList)
        {
            // �� �� ����Ʈ �г� �Ͽ� ��ư �ϳ� ����
            var roomBtn = Instantiate(roomPrefab, roomListPanel);
            // �� �� �̸��� ��ư �ؽ�Ʈ�� �����
            roomBtn.GetComponentInChildren<Text>().text = room.Name;
        }
    }
}
