using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ▼ 집회소에 들어가는 상호작용을 위한 스크립트
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

        Debug.Log("방 생성할 준비가 되었습니다. 집회소로 이동합니다.");

        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 16 };
        PhotonNetwork.CreateRoom("MeetingHouse",roomOptions);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("방 입장");
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



