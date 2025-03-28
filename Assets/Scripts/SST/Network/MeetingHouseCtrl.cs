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
        StartCoroutine(WaitForFindPlayer());
    }

    private void Update()
    {
        if (isInMeetingZone && Input.GetKeyDown(KeyCode.F))
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

        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 16 };
        LoadingSceneManager.LoadSceneWithLoading("MeetingHouse", roomOptions);
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



