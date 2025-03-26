using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

// �� ����Ʈ ������ ������ ���� �����ϴ� ��ũ��Ʈ
public partial class MeetingRoomManager : MonoBehaviourPunCallbacks
{
    [Header("�� ���� UI")]
    [SerializeField] private Canvas roomInfoCanvas;          // �� ���ο��� �������� UI ĵ����
    [SerializeField] private RectTransform playListPanel;      // �÷��̾� ����Ʈ�� ǥ�õ� �г�
    [SerializeField] private GameObject playerListPrefab;      // �÷��̾� ����Ʈ ������ ������

    [Header("�� �غ�, ��ŸƮ ��ư")]
    [SerializeField] private Button questStartButton;          // ������ ���� ������ �� ����ϴ� ��ư
    [SerializeField] private Button questReadyButton;          // �Ϲ� �÷��̾ �غ� ���¸� ������ �� ����ϴ� ��ư

    private void Awake()
    {
        // �ʱ⿡ �� ���� UI�� ��Ȱ��ȭ
        roomInfoCanvas.gameObject.SetActive(false);
        questStartButton.gameObject.SetActive(false);
        questReadyButton.gameObject.SetActive(false);
    }

    #region Photon Callbacks for Room

    // �濡 �������� �� ȣ��Ǵ� �ݹ�
    public override void OnJoinedRoom()
    {
        Debug.Log("�濡 �����Ͽ����ϴ�");
        roomInfoCanvas.gameObject.SetActive(true);

        // �����̸� ���� ���� ��ư, �Ϲ� �÷��̾�� �غ� ��ư�� Ȱ��ȭ
        if (PhotonNetwork.IsMasterClient)
        {
            questReadyButton.gameObject.SetActive(false);
            questStartButton.gameObject.SetActive(true);
        }
        else
        {
            questStartButton.gameObject.SetActive(false);
            questReadyButton.gameObject.SetActive(true);
        }

        UpdatePlayerListUI();
    }

    //// ���ο� �÷��̾ �濡 �����ϸ� �÷��̾� ����Ʈ�� ����
    //public override void OnPlayerEnteredRoom(Player newPlayer)
    //{
    //    Debug.Log(newPlayer.NickName + " ���� �����߽��ϴ�");
    //    UpdatePlayerListUI();
    //}

    //// �÷��̾ ���� ������ �÷��̾� ����Ʈ�� ����
    //public override void OnPlayerLeftRoom(Player otherPlayer)
    //{
    //    Debug.Log(otherPlayer.NickName + " ���� �������ϴ�");
    //    UpdatePlayerListUI();
    //}

    // !! Ŀ����������Ƽ ȣ���Լ�
    // �÷��̾��� Custom Properties(��: "isReady")�� ����Ǹ� ȣ���
    //public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    //{
    //    if (changedProps.ContainsKey("isReady"))
    //    {
    //        UpdatePlayerListUI();
    //    }
    //}

    #endregion

    #region UI Update and Room Control

    // �÷��̾� ����Ʈ UI�� ���� ��ġ�� �Լ�
    private void UpdatePlayerListUI()
    {
        // ���� ����Ʈ �׸� ��� ����
        foreach (Transform child in playListPanel)
        {
            Destroy(child.gameObject);
        }

        // ���� �濡 �ִ� ��� �÷��̾� ���� ������� ����Ʈ ����
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject listItem = Instantiate(playerListPrefab, playListPanel);
            listItem.GetComponentInChildren<Text>().text = player.NickName;

            // �÷��̾ �غ� ���¶�� �̹��� ������ �ʷϻ����� ǥ��
            if (player.CustomProperties.ContainsKey("isReady") && (bool)player.CustomProperties["isReady"])
            {
                listItem.GetComponent<Image>().color = Color.green;
            }
            else
            {
                listItem.GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void LeaveRoom()
    {
        
    }

    //// �Ϲ� �÷��̾ �غ� ��ư�� ������ ȣ��Ǵ� �Լ�
    //public void OnReady()
    //{
    //    // Photon Custom Properties�� ���� ���� �÷��̾��� �غ� ���¸� true�� ����
    //    ExitGames.Client.Photon.Hashtable readyProps = new ExitGames.Client.Photon.Hashtable();
    //    readyProps["isReady"] = true;
    //    PhotonNetwork.LocalPlayer.SetCustomProperties(readyProps);
    //}

    // ������ ���� ���� ��ư�� ������ ȣ��Ǵ� �Լ�
    public void InGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // ��� �÷��̾�� �Բ� InGame ������ ��ȯ
            PhotonNetwork.LoadLevel("InGame");
        }
    }

    #endregion
}
   

