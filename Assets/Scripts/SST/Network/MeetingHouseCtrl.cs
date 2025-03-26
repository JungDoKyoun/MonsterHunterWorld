using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �� ��ȸ�ҿ� ���� ��ȣ�ۿ��� ���� ��ũ��Ʈ
public class MeetingHouseCtrl : MonoBehaviourPunCallbacks
{
    private Collider meetingHouseZone;

    private bool isInMeetingZone = false;

    private void Start()
    {
        meetingHouseZone = GetComponent<Collider>();
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
        PhotonNetwork.CreateRoom("MeetingHouse",roomOptions);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("�� ����");
        SceneManager.LoadScene("MeetingHouse");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInMeetingZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInMeetingZone = false;
        }
    }
}    



