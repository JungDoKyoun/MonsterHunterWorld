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
        Debug.Log("로비에 입장하였습니다");
    }

    public override void OnLeftLobby()
    {
        Debug.Log("로비에서 나갔습니다");
    }

    // ▼ 로비 입장시 알아서 콜백 호출됨
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // ▼ 모든 룸 리스트 정보를 반복으로 돌며
        foreach (RoomInfo room in roomList)
        {
            // ▼ 룸 리스트 패널 하에 버튼 하나 생성
            var roomBtn = Instantiate(roomPrefab, roomListPanel);
            // ▼ 룸 이름을 버튼 텍스트에 담아줌
            roomBtn.GetComponentInChildren<Text>().text = room.Name;
        }
    }
}
