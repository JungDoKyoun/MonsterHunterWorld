using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("�� UI ����")]
    [SerializeField] private Canvas roomInfoCanvas;          // �� ���ο��� �������� UI ĵ����
    [SerializeField] private RectTransform playListPanel;      // �÷��̾� ����Ʈ�� ǥ�õ� �г�
    [SerializeField] private GameObject playerListPrefab;      // �÷��̾� ����Ʈ ������ ������
    [SerializeField] private GameObject hostPlayerPrefab;      // ȣ��Ʈ ����Ʈ ������ ������

    [Header("�� ��ư : ��ŸƮ, ����")]
    [SerializeField] private Button questStartButton;          // ������ ���� ������ �� ����ϴ� ��ư
    [SerializeField] private Button questReadyButton;          // �Ϲ� �÷��̾ �غ� ���¸� ������ �� ����ϴ� ��ư

    private void Start()
    {
        // �ʱ⿡ �� ���� UI�� ��Ȱ��ȭ
        roomInfoCanvas.gameObject.SetActive(false);
        questStartButton.gameObject.SetActive(false);
        questReadyButton.gameObject.SetActive(false);
    }

    #region �����ݹ�

    // �濡 �������� �� ȣ��Ǵ� �ݹ�
    public override void OnJoinedRoom()
    {
        Debug.Log("�濡 �����߽��ϴ�");
        roomInfoCanvas.gameObject.SetActive(true);

        // �����̸� ���� ���� ��ư, �Ϲ� �÷��̾�� ���� ���� ��ư Ȱ��ȭ
        if (PhotonNetwork.IsMasterClient)
        {
            questStartButton.gameObject.SetActive(true);
            questReadyButton.gameObject.SetActive(false);
        }
        else
        {
            questReadyButton.gameObject.SetActive(true);
            questStartButton.gameObject.SetActive(false);
        }

        UpdatePlayerListUI();
    }

    // ���ο� �÷��̾ �濡 �����ϸ� �÷��̾� ����Ʈ�� ����
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " ���� �����߽��ϴ�");
        UpdateStartButton();
        UpdatePlayerListUI();
    }

    // �÷��̾ ���� ������ �÷��̾� ����Ʈ�� ����
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " ���� �������ϴ�");
        UpdateStartButton();
        UpdatePlayerListUI();
    }

    // �÷��̾��� Custom Properties(��: "isReady")�� ����Ǹ� ȣ���
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("isReady"))
        {
            UpdateStartButton();
            UpdatePlayerListUI();
        }
    }

    // �� ������ ��û (�κ�� ���ư� ���)
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        roomInfoCanvas.gameObject.SetActive(false);
    }

    #endregion

    #region UI,�� ��Ʈ��

    // �÷��̾� ����Ʈ UI�� ���� ��ġ�� �Լ�
    private void UpdatePlayerListUI()
    {
        // ���� ����Ʈ �׸� ��� ����
        foreach(Transform child in playListPanel)
        {
            Destroy(child.gameObject);
        }

        // ���� �濡 �ִ� ��� �÷��̾� ���� ������� ����Ʈ ����
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // �濡 ���� �÷��̾ ���� �÷��̾��Ʈ �������� �÷��̾� ����Ʈ �гο� �ν��Ͻ�ȭ
            // �� ������ �ؽ�Ʈ�� �ش� �÷��̾� �г������� �ٲ��ش�.

            // �� �÷��̾ ������ Ŭ���̾�Ʈ������ Ȯ��
            if (player.ActorNumber == PhotonNetwork.MasterClient.ActorNumber)
            {
                GameObject hostList = Instantiate(hostPlayerPrefab, playListPanel);
                hostList.GetComponentInChildren<Text>().text = player.NickName;
            }
            else
            {
                GameObject playerList = Instantiate(playerListPrefab, playListPanel);
                playerList.GetComponentInChildren<Text>().text = player.NickName;

                // �÷��̾ �غ� ���¶�� �̹��� ������ �ʷϻ����� ǥ��
                if (player.CustomProperties.ContainsKey("isReady") && (bool)player.CustomProperties["isReady"])
                {
                    Image readyImg = playerList.transform.Find("CheckReady").GetComponent<Image>();
                    readyImg.color = Color.green;                    
                }
                else
                {
                    Image readyImg = playerList.transform.Find("CheckReady").GetComponent<Image>();
                    readyImg.color = Color.white;                    
                }
            }
        }

    }

    // �Ϲ� �÷��̾ �غ� ��ư�� ������ ȣ��Ǵ� �Լ�
    public void OnReady()
    {
        // ������ bool���� false�� �ϴ� �ʱ�ȭ
        bool currentReady = false;

        // ���࿡ Ŀ����������Ƽ�� isReadyŰ�� ���ԵǾ� �ִٸ�
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("isReady"))
        {
            // ���� bool ���� ���ԵǾ� �ִ� isReady���� ����
            currentReady = (bool)PhotonNetwork.LocalPlayer.CustomProperties["isReady"];
        }

        // ���� bool���� �ݴ�� �ӽú����� ����
        bool newReady = !currentReady;

        // Photon Custom Properties�� ���� ���� �÷��̾��� �غ� ���¸� ��ư�� ����������
        // �ݴ�� �����ϰ� �Ѵ�. ( Ready ���¸� ���ٰ� �״ٰ� �ϱ� ���ؼ� )
        ExitGames.Client.Photon.Hashtable readyProps = new ExitGames.Client.Photon.Hashtable();
        readyProps["isReady"] = newReady;
        PhotonNetwork.LocalPlayer.SetCustomProperties(readyProps);
    }

    // ��� �÷��̾ ���� ���� ������ üũ�� ��ȯ����
    private bool AreAllPlayerReady()
    {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            if(!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("isReady")
                || !(bool)player.CustomProperties["isReady"])
            {
                return false;
            }
        }
        return true;
    }

    // ������ ��ŸƮ ��ư�� ������ ��� �÷��̾ ���� ������ ���� Ȱ��ȭ �ǰ� ������Ʈ
    private void UpdateStartButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            questStartButton.interactable = AreAllPlayerReady();
        }
    }

    // ������ ���� ���� ��ư�� ������ ȣ��Ǵ� �Լ�
    public void InGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // ��� �÷��̾ ���� ���¶��
            if (AreAllPlayerReady())
            {
                // ��� �÷��̾�� �Բ� InGame ������ ��ȯ
                PhotonNetwork.LoadLevel("ALLTestScene");
            }
            else
            {
                Debug.Log("��� �÷��̾ �غ� ���� �ʾҽ��ϴ�");
            }
        }
    }

    #endregion
}

