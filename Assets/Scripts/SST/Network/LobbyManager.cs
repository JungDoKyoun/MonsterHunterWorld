using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private static LobbyManager instance;
    public static LobbyManager Instance { get { return instance; } }

    [SerializeField] Text roomName;
    [SerializeField] GameObject roomPrefab;
    [SerializeField] Transform roomListPanel;

    [SerializeField] RectTransform createRoomPanel;
    [SerializeField] RectTransform joinRoomPanel;

    [SerializeField] Canvas roomInfoCanvas;

    [SerializeField] RectTransform playListPanel;
    [SerializeField] GameObject playerListPrefab;

    float panelMoveSpeed = 10.0f;

    // 나중에 UI 스크롤처럼 이동하게 효과 줄 거임
    Vector2 originPos = Vector2.up * 1500;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }        
    }

    private void Start()
    {
        // ▼ UI 스크롤 이동 효과 나중에
        //createRoomPanel.anchoredPosition = originPos;
        //joinRoomPanel.anchoredPosition = originPos;
        createRoomPanel.gameObject.SetActive(false);
        joinRoomPanel.gameObject.SetActive(false);
        roomInfoCanvas.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ActiveCreateRoomUI();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DeactiveCreateRoomUI();
            }
        }

        else if (Input.GetKeyDown(KeyCode.G))
        {
            ActiveJoinRoomUI();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DeactiveJoinRoomUI();
            }
        }
    }

    // 퀘스트 이름을 그대로 룸 이름에도 세팅 ( 버튼 클릭시 발동 )
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(roomName.text, roomOptions);        
    }

    // 룸 이름 그대로 방 입장 ( 버튼 클릭 시 )
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName.text);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnCreatedRoom()
    {
        // 일단 누군가 방 만들면 조인룸 패널이 조작 가능한 위치로 오게 끔 설정
        // joinRoomPanel.anchoredPosition = Vector2.zero;
        Debug.Log("방이 생성되었습니다");
        createRoomPanel.gameObject.SetActive(false);
        roomInfoCanvas.gameObject.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방에 입장하였습니다");
        roomInfoCanvas.gameObject.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " 님이 입장했습니다");

        //패널 밑에 있는 아이템 싹 제거
        RectTransform[] pList = playListPanel.GetComponentsInChildren<RectTransform>();

        foreach(var b in pList)
        {
            Destroy(b.gameObject);
        }

        foreach(var a in PhotonNetwork.PlayerList)
        {
            var pListPrefab = Instantiate(playerListPrefab, playListPanel);
            pListPrefab.GetComponentInChildren<Text>().text = a.NickName;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " 님이 나갔습니다");
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

    public void ActiveCreateRoomUI()
    {
        createRoomPanel.gameObject.SetActive(true);
    }

    public void DeactiveCreateRoomUI()
    {
        createRoomPanel.gameObject.SetActive(false);
    }

    public void ActiveJoinRoomUI()
    {
        joinRoomPanel.gameObject.SetActive(true);
    }

    public void DeactiveJoinRoomUI()
    {
        joinRoomPanel.gameObject.SetActive(false);
    }
}
