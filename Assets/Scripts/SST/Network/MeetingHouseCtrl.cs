using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// �� ��ȸ�ҿ� ���� ��ȣ�ۿ��� ���� ��ũ��Ʈ
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



