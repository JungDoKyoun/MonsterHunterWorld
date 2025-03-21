using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager Instance { get; private set; }

    [Header("�κ� UI ����")]
    [SerializeField] private Text roomName;                   // �� ����/���� �� �Է��ϴ� �� �̸�
    [SerializeField] private GameObject roomPrefab;           // �� ��Ͽ� ǥ���� ��ư ������
    [SerializeField] private Transform roomListPanel;         // �� ����� ������ �г�

    [Header("����Ʈ ����, ���� UI ĵ����")]
    [SerializeField] private Canvas createRoomCanvas;         // �� ���� UI ĵ����
    [SerializeField] private Canvas joinRoomCanvas;           // �� ���� UI ĵ����

    // NPC ���� ���� ���� (��: NPC�� ��ȣ�ۿ� �� UI Ȱ��ȭ)
    private List<NpcCtrl> activeNpcs = new List<NpcCtrl>();

    //Vector2 originPos = Vector2.up * 1500; // ���� UI ��ũ�� ���⿡ Ȱ��

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
        createRoomCanvas.gameObject.SetActive(false);
        joinRoomCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        // F Ű�� ���� ��ó NPC�� ��ȣ�ۿ��ϸ� �ش� UI Ȱ��ȭ
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

        // ESC Ű�� ���� UI �ݱ�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DeactiveCreateRoomUI();
            DeactiveJoinRoomUI();
        }
    }

    #region �κ� ���

    // �� ���� ��û: �� �̸��� �̿��� Photon�� �� ���� ��û
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.CreateRoom(roomName.text, roomOptions);
        DeactiveCreateRoomUI();
    }

    // �� ���� ��û: �Է��� �� �̸����� �� ����
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName.text);
        DeactiveJoinRoomUI();
    }

    // ���� �� ���� ��û
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    #endregion

    #region Photon Callbacks

    // �κ񿡼� �� ��� ������Ʈ: Photon �������� �����ִ� �� ����Ʈ�� ������� UI�� ����
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // ������ ǥ�õ� �� ��� ����
        foreach (Transform child in roomListPanel)
        {
            Destroy(child.gameObject);
        }

        // ���ο� �� ������ ��ư���� �����Ͽ� ǥ��
        foreach (RoomInfo room in roomList)
        {
            GameObject roomBtn = Instantiate(roomPrefab, roomListPanel);
            roomBtn.GetComponentInChildren<Text>().text = room.Name;
            // ��ư Ŭ�� �� JoinRoom �Լ� ȣ�� (�� �̸��� UI �ؽ�Ʈ���� ����)
            roomBtn.GetComponent<Button>().onClick.AddListener(JoinRoom);
        }
    }

    #endregion

    #region NPC Interaction

    // NPC ���� �̺�Ʈ �ڵ鷯: ������ NPC�� ����Ʈ�� �߰��ϰų� ����
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

    // �� ���� UI Ȱ��ȭ
    public void ActiveCreateRoomUI()
    {
        createRoomCanvas.gameObject.SetActive(true);
    }

    // �� ���� UI ��Ȱ��ȭ
    public void DeactiveCreateRoomUI()
    {
        createRoomCanvas.gameObject.SetActive(false);
    }

    // �� ���� UI Ȱ��ȭ
    public void ActiveJoinRoomUI()
    {
        joinRoomCanvas.gameObject.SetActive(true);
    }

    // �� ���� UI ��Ȱ��ȭ
    public void DeactiveJoinRoomUI()
    {
        joinRoomCanvas.gameObject.SetActive(false);
    }

    #endregion
}

