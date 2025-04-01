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
    [Header("�̱��÷��� UI ���")]
    [SerializeField] Canvas questCreateCanvas;      // ����Ʈ ���� UI
    [SerializeField] Canvas roomInfoCanvas;         // ����Ʈ ���� UI
    [SerializeField] Text basicQuestName;           // �⺻ ������ ����Ʈ �̸� Text
    [SerializeField] Text questName;                // ����Ʈ ������ ǥ�õ� ����Ʈ �̸� Text

    PlayerController player;
    private Transform singleQuestPanel;
    private Transform movePanel;

    // NPC ���� ���� ���� (�̱��÷��̿��� ���� NPC�� ����)
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
        // NPC ���� �̺�Ʈ ����
        NpcCtrl.OnNpcDetectionChanged += HandleNpcDetectionChanged;        
    }

    private void OnDestroy()
    {
        // �̺�Ʈ ���� ����
        NpcCtrl.OnNpcDetectionChanged -= HandleNpcDetectionChanged;
    }

    private void Start()
    {

        // �ʱ⿡ �κ� ���� UI ĵ���� ��Ȱ��ȭ
        questCreateCanvas.gameObject.SetActive(false);
        roomInfoCanvas.gameObject.SetActive(false);

        // ���������� ������ ����Ʈ �̸��� ����Ʈ ������ �� �ش�� ����Ʈ �̸����� �ʱ�ȭ
        questName.text = basicQuestName.text;

        SoundManager.Instance.PlayBGM(SoundManager.BGMType.Single, 0.4f);

        StartCoroutine(WaitForFindPlayer());
    }

    private void Update()
    {
        // F Ű�� ���� ��ó NPC�� ��ȣ�ۿ��ϸ� �ش� UI Ȱ��ȭ
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

        // ESC Ű�� ���� UI �ݱ�
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

    // �� ���� ��û: �� �̸��� �̿��� Photon�� �� ���� ��û
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
        Debug.Log("�̱� ����Ʈ â���� �������ϴ�.");
    }

    // NPC ���� �̺�Ʈ �ڵ鷯: ������ NPC�� ����Ʈ�� �߰��ϰų� ����
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

        Debug.Log("�� ������ �غ� �Ǿ����ϴ�. �̱� ����Ʈ ������ �̵��մϴ�.");

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

        Debug.Log("�� ������ �غ� �Ǿ����ϴ�. �̱۷����� �̵��մϴ�.");

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

