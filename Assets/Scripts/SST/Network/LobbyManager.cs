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

    [SerializeField] Canvas createRoomCanvas;
    [SerializeField] Canvas joinRoomCanvas;
    [SerializeField] Canvas roomInfoCanvas;

    [SerializeField] RectTransform playListPanel;
    [SerializeField] GameObject playerListPrefab;

    private List<NpcCtrl> activeNpcs = new List<NpcCtrl>();

    float panelMoveSpeed = 10.0f;

    // 나중에 UI 스크롤처럼 이동하게 효과 줄 거임
    Vector2 originPos = Vector2.up * 1500;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        NpcCtrl.OnNpcDetectionChanged += HandleNpcDetectionChanged;
    }

    private void Start()
    {
        // ▼ UI 스크롤 이동 효과 나중에
        //createRoomPanel.anchoredPosition = originPos;
        //joinRoomPanel.anchoredPosition = originPos;
        createRoomCanvas.gameObject.SetActive(false);
        joinRoomCanvas.gameObject.SetActive(false);
        roomInfoCanvas.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (activeNpcs.Count > 0)
            {
                Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;
                NpcCtrl selectedNpc = null;
                float minDistance = Mathf.Infinity;

                foreach(var npc in activeNpcs)
                {
                    float distance = Vector3.Distance(npc.transform.position, playerPos.position);

                    if(distance < minDistance)
                    {
                        minDistance = distance;
                        selectedNpc = npc;
                    }
                }

                if (selectedNpc != null)
                {
                    if(selectedNpc.npcType == NpcCtrl.Type.Create)
                    {
                        ActiveCreateRoomUI();
                    }
                    else if(selectedNpc.npcType == NpcCtrl.Type.Join)
                    {
                        ActiveJoinRoomUI();
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeactiveCreateRoomUI();
            DeactiveJoinRoomUI();
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
        createRoomCanvas.gameObject.SetActive(false);
        roomInfoCanvas.gameObject.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방에 입장하였습니다");
        joinRoomCanvas.gameObject.SetActive(false);
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
            roomBtn.GetComponent<Button>().onClick.AddListener(JoinRoom);
        }
    }

    private void HandleNpcDetectionChanged(NpcCtrl npc, bool isActive)
    {
        if (isActive)
        {
            // NPC가 활성 상태일때 그 NPC가 리스트에 추가 안되어있으면 추가
            if (!activeNpcs.Contains(npc))
            {
                activeNpcs.Add(npc);
            }
        }
        else
        {
            // NPC가 활성 상태가 아닌데 리스트에 npc가 있다면 제거
            if (activeNpcs.Contains(npc))
            {
                activeNpcs.Remove(npc);
            }
        }
    }

    public void ActiveCreateRoomUI()
    {
        createRoomCanvas.gameObject.SetActive(true);
    }

    public void DeactiveCreateRoomUI()
    {
        createRoomCanvas.gameObject.SetActive(false);
    }

    public void ActiveJoinRoomUI()
    {
        joinRoomCanvas.gameObject.SetActive(true);
    }

    public void DeactiveJoinRoomUI()
    {
        joinRoomCanvas.gameObject.SetActive(false);
    }
}
