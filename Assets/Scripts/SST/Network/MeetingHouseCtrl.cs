using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ▼ 집회소에 들어가는 상호작용을 위한 스크립트
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

        Debug.Log("방 생성할 준비가 되었습니다. 집회소로 이동합니다.");

        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 16 };
        PhotonNetwork.JoinOrCreateRoom("MeetingHouse" + index, roomOptions, TypedLobby.Default);
    }

    // 만약에 위의 16명 설정한 방이 꽉찰 경우 예외처리를 위해 만든 콜백 함수
    // 위의 방에 들어가는게 실패하면 새로 방을 만든다 index를 증가시켜서
    // MeetingHouse1 방에 못들어가면 index++ -> MeetingHouse2 방 참가 없으면 만들기
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        index++;
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 16 };
        PhotonNetwork.JoinOrCreateRoom("MeetingHouse" + index, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("방 입장");
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



