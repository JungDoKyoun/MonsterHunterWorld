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

    // ���߿� UI ��ũ��ó�� �̵��ϰ� ȿ�� �� ����
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
        // �� UI ��ũ�� �̵� ȿ�� ���߿�
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

    // ����Ʈ �̸��� �״�� �� �̸����� ���� ( ��ư Ŭ���� �ߵ� )
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(roomName.text, roomOptions);        
    }

    // �� �̸� �״�� �� ���� ( ��ư Ŭ�� �� )
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
        // �ϴ� ������ �� ����� ���η� �г��� ���� ������ ��ġ�� ���� �� ����
        // joinRoomPanel.anchoredPosition = Vector2.zero;
        Debug.Log("���� �����Ǿ����ϴ�");
        createRoomCanvas.gameObject.SetActive(false);
        roomInfoCanvas.gameObject.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("�濡 �����Ͽ����ϴ�");
        joinRoomCanvas.gameObject.SetActive(false);
        roomInfoCanvas.gameObject.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " ���� �����߽��ϴ�");

        //�г� �ؿ� �ִ� ������ �� ����
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
        Debug.Log(otherPlayer.NickName + " ���� �������ϴ�");
    }

    // �� �κ� ����� �˾Ƽ� �ݹ� ȣ���
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // �� ��� �� ����Ʈ ������ �ݺ����� ����
        foreach (RoomInfo room in roomList)
        {
            // �� �� ����Ʈ �г� �Ͽ� ��ư �ϳ� ����
            var roomBtn = Instantiate(roomPrefab, roomListPanel);
            // �� �� �̸��� ��ư �ؽ�Ʈ�� �����
            roomBtn.GetComponentInChildren<Text>().text = room.Name;
            roomBtn.GetComponent<Button>().onClick.AddListener(JoinRoom);
        }
    }

    private void HandleNpcDetectionChanged(NpcCtrl npc, bool isActive)
    {
        if (isActive)
        {
            // NPC�� Ȱ�� �����϶� �� NPC�� ����Ʈ�� �߰� �ȵǾ������� �߰�
            if (!activeNpcs.Contains(npc))
            {
                activeNpcs.Add(npc);
            }
        }
        else
        {
            // NPC�� Ȱ�� ���°� �ƴѵ� ����Ʈ�� npc�� �ִٸ� ����
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
