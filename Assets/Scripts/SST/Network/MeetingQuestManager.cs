using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ▼ 퀘스트 생성, 방 목록 업데이트, 퀘스트 입장 역할
public class MeetingQuestManager : MonoBehaviourPunCallbacks
{
    [Header("퀘스트 UI")]
    [SerializeField] Canvas createRoomCanvas;       // ◀ 퀘스트 생성 UI
    [SerializeField] Canvas joinRoomCanvas;         // ◀ 퀘스트 입장 UI

    [Header("퀘스트 UI 요소")]
    [SerializeField] Text questNameText;             // ◀ 퀘스트(방) 이름 텍스트
    [SerializeField] RectTransform roomListPanel;   // ◀ 퀘스트(방) 목록 패널
    [SerializeField] GameObject roomPrefab;     // ◀ 목록에 채워질 퀘스트(방) 프리팹

    // ▼ NPC 감지 관련 변수
    private List<NpcCtrl> activeNpcs = new List<NpcCtrl>();

    private void Awake()
    {
        // ▼ NPC 감지 이벤트 구독
        NpcCtrl.OnNpcDetectionChanged += HandleNpcDetectionChanged;
    }

    private void OnDestroy()
    {
        // ▼ 파괴시 구독 해지
        NpcCtrl.OnNpcDetectionChanged -= HandleNpcDetectionChanged;
    }

    private void Start()
    {
        // ▼ 일단 퀘스트(방) 생성, 입장 UI 비활성화로 초기화
        createRoomCanvas.gameObject.SetActive(false);
        joinRoomCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // ▼ 만약 플레이어를 감지한 NPC가 있어서 리스트에 추가되어있다면
            if(activeNpcs.Count > 0)
            {
                // ▼ 플레이어 태그를 가진 녀석 위치 저장
                Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;
                // ▼ 거리가 더 가까운 NPC가 담길 변수 초기화
                NpcCtrl selectedNpc = null;
                // ▼ 초기 최소거리는 무한
                float minDistance = Mathf.Infinity;

                // ▼ 만약에 여러 NPC가 플레이어를 감지했을 때
                // ▼ 더 가까운 NPC와 상호작용 하기 위함
                foreach(var npc in activeNpcs)
                {
                    // ▼ NPC와 플레이어 사이의 거리를 임시변수에 저장
                    float distance = Vector3.Distance(npc.transform.position, playerPos.position);
                    // ▼ 둘 사이의 거리가 최소거리보다 작다면 즉 가깝다면
                    if(distance < minDistance)
                    {
                        minDistance = distance;
                        selectedNpc = npc;
                    }
                }

                // ▼ NPC의 타입에 따라서 역할에 맞는 UI를 활성화 해서 상호작용
                if(selectedNpc != null)
                {
                    if(selectedNpc.npcType == NpcCtrl.Type.Create)
                    {
                        createRoomCanvas.gameObject.SetActive(true);
                    }
                    else if(selectedNpc.npcType == NpcCtrl.Type.Join)
                    {
                        joinRoomCanvas.gameObject.SetActive(true);
                    }
                }
            }
        }
        // ▼ ESC키로 나가기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            createRoomCanvas.gameObject.SetActive(false);
            joinRoomCanvas.gameObject.SetActive(false);
        }
    }

    public void CreateRoom()
    {
        RoomTransitionManager.Instance.GoToRoom(RoomType.MultiQuestRoom, questNameText.text);
        createRoomCanvas.gameObject.SetActive(false);
    }

    public void JoinRoom()
    {
        RoomTransitionManager.Instance.GoToRoom(RoomType.MultiQuestRoom, questNameText.text);
        joinRoomCanvas.gameObject.SetActive(false);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // ▼ 방목록 패널 초기화
        foreach(Transform child in roomListPanel)
        {
            Destroy(child.gameObject);
        }

        foreach(RoomInfo room in roomList)
        {
            GameObject roomBtn = Instantiate(roomPrefab, roomListPanel);
            roomBtn.GetComponentInChildren<Text>().text = questNameText.text;
            roomBtn.GetComponent<Button>().onClick.AddListener(JoinRoom);
        }
    }

    // ▼ NPC 감지 이벤트 핸들러: 감지된 NPC를 리스트에 추가하거나 제거
    private void HandleNpcDetectionChanged(NpcCtrl npc, bool isActive)
    {
        if (isActive)
        {
            if(!activeNpcs.Contains(npc))
            {
                activeNpcs.Add(npc);
            }
        }
        else
        {
            if(activeNpcs.Contains(npc))
            {
                activeNpcs.Remove(npc);
            }
        }
    }
}
