using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �� ����Ʈ ����, �� ��� ������Ʈ, ����Ʈ ���� ����
public class MeetingQuestManager : MonoBehaviourPunCallbacks
{
    [Header("����Ʈ UI")]
    [SerializeField] Canvas createRoomCanvas;       // �� ����Ʈ ���� UI
    [SerializeField] Canvas joinRoomCanvas;         // �� ����Ʈ ���� UI

    [Header("����Ʈ UI ���")]
    [SerializeField] Text questNameText;             // �� ����Ʈ(��) �̸� �ؽ�Ʈ
    [SerializeField] RectTransform roomListPanel;   // �� ����Ʈ(��) ��� �г�
    [SerializeField] GameObject roomPrefab;     // �� ��Ͽ� ä���� ����Ʈ(��) ������

    // �� NPC ���� ���� ����
    private List<NpcCtrl> activeNpcs = new List<NpcCtrl>();

    private void Awake()
    {
        // �� NPC ���� �̺�Ʈ ����
        NpcCtrl.OnNpcDetectionChanged += HandleNpcDetectionChanged;
    }

    private void OnDestroy()
    {
        // �� �ı��� ���� ����
        NpcCtrl.OnNpcDetectionChanged -= HandleNpcDetectionChanged;
    }

    private void Start()
    {
        // �� �ϴ� ����Ʈ(��) ����, ���� UI ��Ȱ��ȭ�� �ʱ�ȭ
        createRoomCanvas.gameObject.SetActive(false);
        joinRoomCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // �� ���� �÷��̾ ������ NPC�� �־ ����Ʈ�� �߰��Ǿ��ִٸ�
            if(activeNpcs.Count > 0)
            {
                // �� �÷��̾� �±׸� ���� �༮ ��ġ ����
                Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;
                // �� �Ÿ��� �� ����� NPC�� ��� ���� �ʱ�ȭ
                NpcCtrl selectedNpc = null;
                // �� �ʱ� �ּҰŸ��� ����
                float minDistance = Mathf.Infinity;

                // �� ���࿡ ���� NPC�� �÷��̾ �������� ��
                // �� �� ����� NPC�� ��ȣ�ۿ� �ϱ� ����
                foreach(var npc in activeNpcs)
                {
                    // �� NPC�� �÷��̾� ������ �Ÿ��� �ӽú����� ����
                    float distance = Vector3.Distance(npc.transform.position, playerPos.position);
                    // �� �� ������ �Ÿ��� �ּҰŸ����� �۴ٸ� �� �����ٸ�
                    if(distance < minDistance)
                    {
                        minDistance = distance;
                        selectedNpc = npc;
                    }
                }

                // �� NPC�� Ÿ�Կ� ���� ���ҿ� �´� UI�� Ȱ��ȭ �ؼ� ��ȣ�ۿ�
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
        // �� ESCŰ�� ������
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
        // �� ���� �г� �ʱ�ȭ
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

    // �� NPC ���� �̺�Ʈ �ڵ鷯: ������ NPC�� ����Ʈ�� �߰��ϰų� ����
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
