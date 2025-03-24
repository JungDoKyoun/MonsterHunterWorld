using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SingleRoomManager : MonoBehaviourPunCallbacks
{
    public static SingleRoomManager Instance { get; private set; }

    [Header("싱글플레이 UI 요소")]
    [SerializeField] Canvas questCreateCanvas;      // 퀘스트 생성 UI
    [SerializeField] Canvas roomInfoCanvas;         // 퀘스트 정보 UI
    [SerializeField] Text questName;                // 퀘스트 이름 Text
    [SerializeField] Text roomName;                 // 방 이름 Text

    // NPC 감지 관련 변수 (싱글플레이에는 생성 NPC만 있음)
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
        questCreateCanvas.gameObject.SetActive(false);
        roomInfoCanvas.gameObject.SetActive(false);
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
                        questCreateCanvas.gameObject.SetActive(true);
                    }
                }
            }
        }

        // ESC 키를 눌러 UI 닫기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            questCreateCanvas.gameObject.SetActive(false);
            roomInfoCanvas.gameObject.SetActive(false);
        }
    }

    // 방 생성 요청: 방 이름을 이용해 Photon에 방 생성 요청
    public void CreateRoom()
    {
        string createQuestName = questName.text;
        RoomTransitionManager.Instance.GoToRoom(RoomType.SingleQuestRoom, createQuestName);
    }

    public void LeaveRoom()
    {        
        RoomTransitionManager.Instance.GoToRoom(RoomType.SingleRoom);
    }

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

    public void StartGame()
    {
        SceneManager.LoadScene("ALLTestScene");
    }
}

