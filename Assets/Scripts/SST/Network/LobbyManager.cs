using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager Instance { get; private set; }

    [Header("로비 UI 관련")]
    [SerializeField] private Text roomName;                   // 방 생성/입장 시 입력하는 방 이름
    [SerializeField] private GameObject roomPrefab;           // 방 목록에 표시할 버튼 프리팹
    [SerializeField] private Transform roomListPanel;         // 방 목록이 나열될 패널

    [Header("퀘스트 생성, 입장 UI 캔버스")]
    [SerializeField] private Canvas createRoomCanvas;         // 방 생성 UI 캔버스
    [SerializeField] private Canvas joinRoomCanvas;           // 방 입장 UI 캔버스

    // NPC 감지 관련 변수 (예: NPC와 상호작용 시 UI 활성화)
    private List<NpcCtrl> activeNpcs = new List<NpcCtrl>();

    //Vector2 originPos = Vector2.up * 1500; // 추후 UI 스크롤 연출에 활용

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // NPC 감지 이벤트 구독
        NpcCtrl.OnNpcDetectionChanged += HandleNpcDetectionChanged;
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        NpcCtrl.OnNpcDetectionChanged -= HandleNpcDetectionChanged;
    }

    private void Start()
    {
        // 초기에 로비 관련 UI 캔버스 비활성화
        createRoomCanvas.gameObject.SetActive(false);
        joinRoomCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        // F 키를 눌러 근처 NPC와 상호작용하면 해당 UI 활성화
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (activeNpcs.Count > 0)
            {
                Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;
                NpcCtrl selectedNpc = null;
                float minDistance = Mathf.Infinity;

                foreach (var npc in activeNpcs)
                {
                    float distance = Vector3.Distance(npc.transform.position, playerPos.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        selectedNpc = npc;
                    }
                }

                if (selectedNpc != null)
                {
                    if (selectedNpc.npcType == NpcCtrl.Type.Create)
                    {
                        ActiveCreateRoomUI();
                    }
                    else if (selectedNpc.npcType == NpcCtrl.Type.Join)
                    {
                        ActiveJoinRoomUI();
                    }
                }
            }
        }

        // ESC 키를 눌러 UI 닫기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeactiveCreateRoomUI();
            DeactiveJoinRoomUI();
        }
    }

    #region 로비 기능

    // 방 생성 요청: 방 이름을 이용해 Photon에 방 생성 요청
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.CreateRoom(roomName.text, roomOptions);
        DeactiveCreateRoomUI();
    }

    // 방 입장 요청: 입력한 방 이름으로 방 입장
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName.text);
        DeactiveJoinRoomUI();
    }

    // 랜덤 방 입장 요청
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    #endregion

    #region Photon Callbacks

    // 로비에서 방 목록 업데이트: Photon 서버에서 보내주는 방 리스트를 기반으로 UI를 갱신
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // 기존에 표시된 방 목록 삭제
        foreach (Transform child in roomListPanel)
        {
            Destroy(child.gameObject);
        }

        // 새로운 방 정보를 버튼으로 생성하여 표시
        foreach (RoomInfo room in roomList)
        {
            GameObject roomBtn = Instantiate(roomPrefab, roomListPanel);
            roomBtn.GetComponentInChildren<Text>().text = room.Name;
            // 버튼 클릭 시 JoinRoom 함수 호출 (방 이름은 UI 텍스트에서 참조)
            roomBtn.GetComponent<Button>().onClick.AddListener(JoinRoom);
        }
    }

    #endregion

    #region NPC Interaction

    // NPC 감지 이벤트 핸들러: 감지된 NPC를 리스트에 추가하거나 제거
    private void HandleNpcDetectionChanged(NpcCtrl npc, bool isActive)
    {
        if (isActive)
        {
            if (!activeNpcs.Contains(npc))
                activeNpcs.Add(npc);
        }
        else
        {
            if (activeNpcs.Contains(npc))
                activeNpcs.Remove(npc);
        }
    }

    // 방 생성 UI 활성화
    public void ActiveCreateRoomUI()
    {
        createRoomCanvas.gameObject.SetActive(true);
    }

    // 방 생성 UI 비활성화
    public void DeactiveCreateRoomUI()
    {
        createRoomCanvas.gameObject.SetActive(false);
    }

    // 방 입장 UI 활성화
    public void ActiveJoinRoomUI()
    {
        joinRoomCanvas.gameObject.SetActive(true);
    }

    // 방 입장 UI 비활성화
    public void DeactiveJoinRoomUI()
    {
        joinRoomCanvas.gameObject.SetActive(false);
    }

    #endregion
}

