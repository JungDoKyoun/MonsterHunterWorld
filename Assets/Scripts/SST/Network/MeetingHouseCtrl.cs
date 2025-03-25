using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ▼ 집회소에 들어가는 상호작용을 위한 스크립트
public class MeetingHouseCtrl : MonoBehaviour
{
    private Collider meetingHouseZone;

    private bool isInMeetingZone = false;

    private void Start()
    {
        meetingHouseZone = GetComponent<Collider>();
    }

    private void Update()
    {
        if(isInMeetingZone && Input.GetKeyDown(KeyCode.F))
        {
            PhotonNetwork.LeaveRoom();

            SceneManager.LoadScene("MeetingHouse");
        }                   
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



