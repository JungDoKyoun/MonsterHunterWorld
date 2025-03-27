using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// �� ��ȸ�ҿ� ���� ��ȣ�ۿ��� ���� ��ũ��Ʈ
public class MeetingHouseCtrl : MonoBehaviourPunCallbacks
{
    private int index = 1;
    private Collider meetingHouseZone;

    private PlayerController player;
    private Transform targetPanel;

    private bool isInMeetingZone = false;

    private void Start()
    {
        meetingHouseZone = GetComponent<Collider>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        targetPanel = player.transform.Find("Move Panel");
    }

    private void Update()
    {
        if (isInMeetingZone && Input.GetKeyDown(KeyCode.F))
        {           
            StartCoroutine(WaitForCreateMeetingRoom());            
        }
    }

    IEnumerator WaitForCreateMeetingRoom()
    {        
        PhotonNetwork.LeaveRoom();
        while (!PhotonNetwork.IsConnectedAndReady)
        {
            yield return null;
        }

        Debug.Log("�� ������ �غ� �Ǿ����ϴ�. ��ȸ�ҷ� �̵��մϴ�.");

        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 16 };
        PhotonNetwork.JoinOrCreateRoom("MeetingHouse" + index, roomOptions, TypedLobby.Default);
    }

    // ���࿡ ���� 16�� ������ ���� ���� ��� ����ó���� ���� ���� �ݹ� �Լ�
    // ���� �濡 ���°� �����ϸ� ���� ���� ����� index�� �������Ѽ�
    // MeetingHouse1 �濡 ������ index++ -> MeetingHouse2 �� ���� ������ �����
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        index++;
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 16 };
        PhotonNetwork.JoinOrCreateRoom("MeetingHouse" + index, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("�� ����");
        LoadingSceneManager.LoadSceneWithLoading("MeetingHouse");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInMeetingZone = true;
            targetPanel.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInMeetingZone = false;
            targetPanel.gameObject.SetActive(false);
        }
    }
}    



