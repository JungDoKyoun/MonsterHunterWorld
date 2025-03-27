using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ▼ 퀘스트 생성, 퀘스트 입장, 입장할 방 목록 업데이트, 방 정보 관리하는 역할
public partial class MeetingQuestManager : MonoBehaviourPunCallbacks
{
    [Header("퀘스트 패널")]
    [SerializeField] RectTransform createRoomPanel;     // ◀ 퀘스트 생성 패널
    [SerializeField] RectTransform joinRoomPanel;       // ◀ 퀘스트 입장 패널
    [SerializeField] RectTransform roomInfoPanel;       // ◀ 방 정보 패널

    [Header("퀘스트 UI 요소")]
    [SerializeField] Text questNameText;            // ◀ 퀘스트(방) 이름 텍스트
    [SerializeField] RectTransform roomListPanel;   // ◀ 퀘스트(방) 목록 패널
    [SerializeField] GameObject roomPrefab;         // ◀ 목록에 채워질 퀘스트(방) 프리팹

    [Header("방 정보 UI")]
    [SerializeField] private RectTransform playListPanel;      // 플레이어 리스트가 표시될 패널
    [SerializeField] private GameObject playerListPrefab;      // 플레이어 리스트 아이템 프리팹

    [Header("방 준비, 스타트 버튼")]
    [SerializeField] private Button questStartButton;          // 방장이 게임 시작할 때 사용하는 버튼
    [SerializeField] private Button questReadyButton;          // 일반 플레이어가 준비 상태를 설정할 때 사용하는 버튼

    // ▼ 감지 범위에 플레이어가 들어와서 활성화 된 NPC 담을 리스트
    private List<NpcCtrl> activeNpcs = new List<NpcCtrl>();

    private void Awake()
    {
        createRoomPanel.gameObject.SetActive(false);
        joinRoomPanel.gameObject.SetActive(false);
        roomInfoPanel.gameObject.SetActive(false);
        
        // ▼ NPC 감지 이벤트 구독
        NpcCtrl.OnNpcDetectionChanged += HandleNpcDetectionChanged;
    }

    private void OnDestroy()
    {
        // ▼ 파괴시 구독 해지
        NpcCtrl.OnNpcDetectionChanged -= HandleNpcDetectionChanged;
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

                // ▼ NPC의 타입에 따라서 역할에 맞는 패널을 활성화 해서 퀘스트 생성이나 입장.
                if(selectedNpc != null)
                {
                    if(selectedNpc.npcType == NpcCtrl.Type.Create)
                    {
                        createRoomPanel.gameObject.SetActive(true);
                    }
                    else if(selectedNpc.npcType == NpcCtrl.Type.Join)
                    {
                        joinRoomPanel.gameObject.SetActive(true);
                    }
                }
            }
        }
        // ▼ ESC키 누르면 퀘스트 관련 패널들 다 끕니다.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            createRoomPanel.gameObject.SetActive(false);
            joinRoomPanel.gameObject.SetActive(false);
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

    // 콜백함수로 썼었던 기능입니다. 룸 리스트를 업데이트 합니다.
    // 누군가 방을 만들면 퀘스트 입장 UI에서 해당 방정보를 담은 버튼 프리팹을
    // 방 목록 패널에 생성합니다. 버튼에 해당 방에 입장할 수 있는 OnClick 이벤트를 넣어줍니다.
    public void RoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject roomBtn = Instantiate(roomPrefab, roomListPanel);
        roomBtn.GetComponentInChildren<Text>().text = questNameText.text;
        //roomBtn.GetComponent<Button>().onClick.AddListener(JoinRoom);
    }

    // 콜백함수로 썼었던 기능입니다. 플레이어 리스트를 업데이트 합니다.
    // 플레이어가 들어오면 플레이어 정보를 표시해줄 이미지 프리팹을
    // 플레이어 리스트 패널에 생성하는 식입니다.
    public void PlayerListUpdate(Player player, bool isReady)
    {
        GameObject listItem = Instantiate(playerListPrefab, playListPanel);
        listItem.GetComponentInChildren<Text>().text = player.NickName;
    }

    // 위의 플레이어 리스트 업데이트와 연관되어있습니다. 해당 플레이어 정보 프리팹에는
    // 호스트는 따로 프리팹이 있고, 플레이어는 레디를 체크하는 이미지가 달려있습니다.
    // 퀘스트 준비 버튼을 클릭하면 레디 이미지를 초록색으로 변경했었습니다.
    // !! 커스텀프로퍼티를 사용해서 isReady라는 키에 bool값을 넣어줬었습니다.
    public void IsReady(Player player, bool isReady)
    {
        //if (isReady)
        //{
        //    listItem.GetComponent<Image>().color = Color.green;
        //}
        //else
        //{
        //    listItem.GetComponent<Image>().color = Color.white;
        //}
    }

    #region 콜백과 연동된 함수들

    // 특정 플레이어가 방을 생성하면 방목록 리스트를 업데이트 해주는 콜백함수
    //public override void OnRoomListUpdate(List<RoomInfo> roomList)
    //{
    //    // ▼ 방목록 패널 초기화
    //    foreach(Transform child in roomListPanel)
    //    {
    //        Destroy(child.gameObject);
    //    }

    //    foreach(RoomInfo room in roomList)
    //    {
    //        GameObject roomBtn = Instantiate(roomPrefab, roomListPanel);
    //        roomBtn.GetComponentInChildren<Text>().text = questNameText.text;
    //        roomBtn.GetComponent<Button>().onClick.AddListener(JoinRoom);
    //    }
    //}

    //// 플레이어 리스트 UI를 새로 고치는 함수
    //private void UpdatePlayerListUI()
    //{
    //    // 기존 리스트 항목 모두 제거
    //    foreach (Transform child in playListPanel)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //    // 현재 방에 있는 모든 플레이어 정보 기반으로 리스트 생성
    //    foreach (Player player in PhotonNetwork.PlayerList)
    //    {
    //        GameObject listItem = Instantiate(playerListPrefab, playListPanel);
    //        listItem.GetComponentInChildren<Text>().text = player.NickName;

    //        // 플레이어가 준비 상태라면 이미지 색상을 초록색으로 표시
    //        if (player.CustomProperties.ContainsKey("isReady") && (bool)player.CustomProperties["isReady"])
    //        {
    //            listItem.GetComponent<Image>().color = Color.green;
    //        }
    //        else
    //        {
    //            listItem.GetComponent<Image>().color = Color.white;
    //        }
    //    }
    //}

    //// 일반 플레이어가 준비 버튼을 누르면 호출되는 함수
    //public void OnReady()
    //{
    //    // Photon Custom Properties를 통해 로컬 플레이어의 준비 상태를 true로 설정
    //    ExitGames.Client.Photon.Hashtable readyProps = new ExitGames.Client.Photon.Hashtable();
    //    readyProps["isReady"] = true;
    //    PhotonNetwork.LocalPlayer.SetCustomProperties(readyProps);
    //}

    //// 방장이 게임 시작 버튼을 누르면 호출되는 함수
    //public void InGame()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        // 모든 플레이어와 함께 InGame 씬으로 전환
    //        PhotonNetwork.LoadLevel("InGame");
    //    }
    //}

    //// 방에 입장했을 때 호출되는 콜백
    //public override void OnJoinedRoom()
    //{
    //    Debug.Log("방에 입장하였습니다");

    //    // 방장이면 게임 시작 버튼, 일반 플레이어면 준비 버튼을 활성화
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        questReadyButton.gameObject.SetActive(false);
    //        questStartButton.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        questStartButton.gameObject.SetActive(false);
    //        questReadyButton.gameObject.SetActive(true);
    //    }

    //    UpdatePlayerListUI();
    //}

    //// 새로운 플레이어가 방에 입장하면 플레이어 리스트를 갱신
    //public override void OnPlayerEnteredRoom(Player newPlayer)
    //{
    //    Debug.Log(newPlayer.NickName + " 님이 입장했습니다");
    //    UpdatePlayerListUI();
    //}

    //// 플레이어가 방을 나가면 플레이어 리스트를 갱신
    //public override void OnPlayerLeftRoom(Player otherPlayer)
    //{
    //    Debug.Log(otherPlayer.NickName + " 님이 나갔습니다");
    //    UpdatePlayerListUI();
    //}

    // !! 커스텀프로퍼티 호출함수
    // 플레이어의 Custom Properties(예: "isReady")가 변경되면 호출됨
    //public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    //{
    //    if (changedProps.ContainsKey("isReady"))
    //    {
    //        UpdatePlayerListUI();
    //    }
    //}

    #endregion

}
