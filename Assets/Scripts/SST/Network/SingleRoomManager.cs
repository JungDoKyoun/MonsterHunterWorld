using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SingleRoomManager : MonoBehaviourPunCallbacks
{
    [Header("싱글플레이 UI 요소")]
    [SerializeField] Canvas questCreateCanvas;      // 퀘스트 생성 UI
    [SerializeField] Canvas roomInfoCanvas;         // 퀘스트 정보 UI
    [SerializeField] Text basicQuestName;           // 기본 설정된 퀘스트 이름 Text
    [SerializeField] Text questName;                // 퀘스트 정보에 표시될 퀘스트 이름 Text

    PlayerController player;
    private Transform singleQuestPanel;
    private Transform movePanel;

    // NPC 감지 관련 변수 (싱글플레이에는 생성 NPC만 있음)
    private List<NpcCtrl> activeNpcs = new List<NpcCtrl>();

    private string roomName = "SingleRoom";

    private static float InteractionRange = 2f;

    [SerializeField]
    private Transform _boxTransform = null;


    private bool _hasCinemachineFreeLook = false;

    private CinemachineFreeLook _cinemachineFreeLook = null;

    private CinemachineFreeLook getCinemachineFreeLook
    {
        get
        {
            if (_hasCinemachineFreeLook == false)
            {
                _hasCinemachineFreeLook = true;
                _cinemachineFreeLook = FindObjectOfType<CinemachineFreeLook>();
            }
            return _cinemachineFreeLook;
        }
    }

#if UNITY_EDITOR
    [SerializeField]
    private Color _gizmoColor = Color.blue;

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        if (_boxTransform != null)
        {
            Gizmos.DrawWireSphere(_boxTransform.position, InteractionRange);
        }
    }
#endif

    private void Awake()
    {
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

        // 룸정보에서 나오는 퀘스트 이름을 퀘스트 생성할 때 해당된 퀘스트 이름으로 초기화
        questName.text = basicQuestName.text;

        SoundManager.Instance.PlayBGM(SoundManager.BGMType.Single, 0.4f);

        StartCoroutine(WaitForFindPlayer());
    }

    private void Update()
    {
        // F 키를 눌러 근처 NPC와 상호작용하면 해당 UI 활성화
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (activeNpcs.Count > 0)
            {
                NpcCtrl selectedNpc = null;
                float minDistance = Mathf.Infinity;
                foreach (var npc in activeNpcs)
                {
                    float distance = Vector3.Distance(npc.transform.position, player.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        selectedNpc = npc;
                    }
                }
                if (selectedNpc != null && selectedNpc.npcType == NpcCtrl.Type.SingleQuest)
                {
                    if (!questCreateCanvas.gameObject.activeInHierarchy)
                    {
                        SoundManager.Instance.PlaySFX(SoundManager.SfxQuestType.CreateQuest);
                    }
                    questCreateCanvas.gameObject.SetActive(true);
                }
            }
        }

        // ESC 키를 눌러 UI 닫기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(questCreateCanvas.gameObject.activeInHierarchy || roomInfoCanvas.gameObject.activeInHierarchy)
            {
                SoundManager.Instance.PlaySFX(SoundManager.SfxQuestType.LeaveQuest);
            }
            questCreateCanvas.gameObject.SetActive(false);
            roomInfoCanvas.gameObject.SetActive(false);

        }

        if (player != null && singleQuestPanel != null && singleQuestPanel.gameObject.activeInHierarchy == false
            && movePanel != null && movePanel.gameObject.activeInHierarchy == false)
        {
            switch (player.enabled)
            {
                case true:
                    if (_boxTransform != null)
                    {
                        if (Vector3.Distance(_boxTransform.position, player.transform.position) < InteractionRange)
                        {
                            player.Show(PlayerInteraction.State.Box);
                            if (Input.GetKeyDown(KeyCode.F))
                            {
                                player.Show(PlayerInteraction.State.Hide);
                                player.enabled = false;
                                getCinemachineFreeLook.SetEnabled(false);
                                UIManager.Instance.StackUIOpen(UIType.AllVillageUI);
                            }
                        }
                        else
                        {
                            player.Show(PlayerInteraction.State.Hide);
                        }
                    }
                    break;
                case false:
                    if (UIManager.Instance.IsOpenBox() == false)
                    {
                        getCinemachineFreeLook.SetEnabled(true);
                        player.enabled = true;
                    }
                    break;
            }
        }
    }

    // 방 생성 요청: 방 이름을 이용해 Photon에 방 생성 요청
    public void CreateQuest()
    {
        //StartCoroutine(WaitForCreateQuestRoom());
        questCreateCanvas.gameObject.SetActive(false);
        roomInfoCanvas.gameObject.SetActive(true);

    }

    public void LeaveQuest()
    {
        //StartCoroutine(WaitForCreateSingleRoom());
        roomInfoCanvas.gameObject.SetActive(false);
        Debug.Log("싱글 퀘스트 창에서 나갔습니다.");
    }

    // NPC 감지 이벤트 핸들러: 감지된 NPC를 리스트에 추가하거나 제거
    private void HandleNpcDetectionChanged(NpcCtrl npc, bool isActive)
    {
        singleQuestPanel.gameObject.SetActive(isActive);
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
        SceneManager.LoadScene("DoKyoun");
    }

    IEnumerator WaitForFindPlayer()
    {
        while(player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if(playerObj != null)
            {
                player = playerObj.GetComponent<PlayerController>();
            }
            yield return null;
        }

        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        singleQuestPanel = playerInteraction.singleQuestPanel;
        movePanel = playerInteraction.movePanel;
        singleQuestPanel.gameObject.SetActive(false);
    }

    IEnumerator WaitForCreateQuestRoom()
    {
        PhotonNetwork.LeaveRoom();
        while (!PhotonNetwork.IsConnectedAndReady)
        {
            yield return null;
        }

        Debug.Log("방 생성할 준비가 되었습니다. 싱글 퀘스트 룸으로 이동합니다.");

        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 1 };
        PhotonNetwork.CreateRoom(basicQuestName.text, roomOptions);
    }

    IEnumerator WaitForCreateSingleRoom()
    {
        PhotonNetwork.LeaveRoom();
        while (!PhotonNetwork.IsConnectedAndReady)
        {
            yield return null;
        }

        Debug.Log("방 생성할 준비가 되었습니다. 싱글룸으로 이동합니다.");

        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 1 };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public void PlayButtonClickSFX()
    {
        SoundManager.Instance.PlayBtnClickSFX();
    }

    public void PlayWheelSFX()
    {
        SoundManager.Instance.PlayWheelSFX();
    }

    public void PlayStartButtonSFX()
    {
        SoundManager.Instance.PlayStartButtonSFX();
    }
}

