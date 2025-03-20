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

    [SerializeField] RectTransform createRoomPanel;
    [SerializeField] RectTransform joinRoomPanel;

    [SerializeField] Canvas roomInfoCanvas;

    [SerializeField] RectTransform playListPanel;
    [SerializeField] GameObject playerListPrefab;

    float panelMoveSpeed = 10.0f;

    // ���߿� UI ��ũ��ó�� �̵��ϰ� ȿ�� �� ����
    Vector2 originPos = Vector2.up * 1500;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }        
    }

    private void Start()
    {
        // �� UI ��ũ�� �̵� ȿ�� ���߿�
        //createRoomPanel.anchoredPosition = originPos;
        //joinRoomPanel.anchoredPosition = originPos;
        createRoomPanel.gameObject.SetActive(false);
        joinRoomPanel.gameObject.SetActive(false);
        roomInfoCanvas.gameObject.SetActive(false);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ActiveCreateRoomUI();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DeactiveCreateRoomUI();
            }
        }

        else if (Input.GetKeyDown(KeyCode.G))
        {
            ActiveJoinRoomUI();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DeactiveJoinRoomUI();
            }
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
        createRoomPanel.gameObject.SetActive(false);
        roomInfoCanvas.gameObject.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("�濡 �����Ͽ����ϴ�");
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
        }
    }

    public void ActiveCreateRoomUI()
    {
        createRoomPanel.gameObject.SetActive(true);
    }

    public void DeactiveCreateRoomUI()
    {
        createRoomPanel.gameObject.SetActive(false);
    }

    public void ActiveJoinRoomUI()
    {
        joinRoomPanel.gameObject.SetActive(true);
    }

    public void DeactiveJoinRoomUI()
    {
        joinRoomPanel.gameObject.SetActive(false);
    }
}
