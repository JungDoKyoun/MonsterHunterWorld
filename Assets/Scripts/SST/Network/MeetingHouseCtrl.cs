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
        // �÷��̾ ��ã�Ҵٸ� �ݺ�
        while(player == null)
        {
            // ��ã�Ҵٸ� �±װ� �÷��̾��� ���ӿ�����Ʈ�� ã�Ƽ� �ӽ� ������ ��´�.
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            // �ӽ� ���ӿ�����Ʈ ������ ���ٸ�
            if(playerObj != null)
            {
                // player�� �÷��̾� ������Ʈ�� ��´�
                player = playerObj.GetComponent<PlayerController>();
            }
            yield return null;
        }
        // �÷��̾� �ڽĿ� �ִ� Move Panel�� ã�Ƽ� ����ش�
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

        Debug.Log("�� ������ �غ� �Ǿ����ϴ�. ��ȸ�ҷ� �̵��մϴ�.");

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
