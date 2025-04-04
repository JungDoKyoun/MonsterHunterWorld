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
    private Collider meetingHouseZone;

    RoomOptions meetingRoomOption = new RoomOptions { MaxPlayers = 16 };

    private PlayerController player;
    private Transform targetPanel;

    private bool isInMeetingZone = false;

    private void Start()
    {
        meetingHouseZone = GetComponent<Collider>();
        StartCoroutine(WaitForFindPlayer());
    }

    [SerializeField]
    private GameObject _readyCanvasObject;

    private void Update()
    {
        switch(isInMeetingZone)
        {
            case true:
                if (targetPanel != null && targetPanel.gameObject.activeInHierarchy == false && _readyCanvasObject != null && _readyCanvasObject.activeInHierarchy == false)
                {
                    targetPanel.gameObject.SetActive(true);
                }
                break;
            case false:
                if (targetPanel != null && targetPanel.gameObject.activeInHierarchy == true)
                {
                    targetPanel.gameObject.SetActive(false);
                }
                break;
        }

        if (isInMeetingZone && _readyCanvasObject != null && _readyCanvasObject.activeInHierarchy == false && Input.GetKeyDown(KeyCode.F))
        {           
            StartCoroutine(WaitForCreateMeetingRoom());            
        }
    }

    IEnumerator WaitForFindPlayer()
    {
        // 플레이어를 못찾았다면 반복
        while(player == null)
        {
            // 못찾았다면 태그가 플레이어인 게임오브젝트를 찾아서 임시 변수에 담는다.
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            // 임시 게임오브젝트 변수에 담겼다면
            if(playerObj != null)
            {
                // player에 플레이어 컴포넌트를 담는다
                player = playerObj.GetComponent<PlayerController>();
            }
            yield return null;
        }
        // 플레이어 자식에 있는 Move Panel을 찾아서 담아준다
        targetPanel = player.GetComponent<PlayerInteraction>().movePanel;
        targetPanel.gameObject.SetActive(false);
    }

    IEnumerator WaitForCreateMeetingRoom()
    {        
        PhotonNetwork.LeaveRoom();
        while (!PhotonNetwork.IsConnectedAndReady)
        {
            yield return null;
        }

        Debug.Log("방 생성할 준비가 되었습니다. 집회소로 이동합니다.");

        LoadingSceneManager.LoadSceneWithLoading("MeetingHouse", meetingRoomOption);
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
