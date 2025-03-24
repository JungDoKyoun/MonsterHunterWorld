using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SingleRoomManager : MonoBehaviourPunCallbacks
{
    public static SingleRoomManager Instance { get; private set; }

    [Header("�̱��÷��� UI ���")]
    [SerializeField] Canvas questCreateCanvas;      // ����Ʈ ���� UI
    [SerializeField] Canvas roomInfoCanvas;         // ����Ʈ ���� UI
    [SerializeField] Text questName;                // ����Ʈ �̸� Text
    [SerializeField] Text roomName;                 // �� �̸� Text

    // NPC ���� ���� ���� (�̱��÷��̿��� ���� NPC�� ����)
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
        questCreateCanvas.gameObject.SetActive(false);
        roomInfoCanvas.gameObject.SetActive(false);
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
                        questCreateCanvas.gameObject.SetActive(true);
                    }
                }
            }
        }

        // ESC Ű�� ���� UI �ݱ�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            questCreateCanvas.gameObject.SetActive(false);
            roomInfoCanvas.gameObject.SetActive(false);
        }
    }

    // �� ���� ��û: �� �̸��� �̿��� Photon�� �� ���� ��û
    public void CreateRoom()
    {
        string createQuestName = questName.text;
        RoomTransitionManager.Instance.GoToRoom(RoomType.SingleQuestRoom, createQuestName);
    }

    public void LeaveRoom()
    {        
        RoomTransitionManager.Instance.GoToRoom(RoomType.SingleRoom);
    }

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

    public void StartGame()
    {
        SceneManager.LoadScene("ALLTestScene");
    }
}

