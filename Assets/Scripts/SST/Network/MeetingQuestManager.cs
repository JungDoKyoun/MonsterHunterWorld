using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �� ����Ʈ ����, ����Ʈ ����, ������ �� ��� ������Ʈ, �� ���� �����ϴ� ����
public partial class MeetingQuestManager : MonoBehaviourPunCallbacks
{
    [Header("����Ʈ �г�")]
    [SerializeField] RectTransform createRoomPanel;     // �� ����Ʈ ���� �г�
    [SerializeField] RectTransform joinRoomPanel;       // �� ����Ʈ ���� �г�
    [SerializeField] RectTransform roomInfoPanel;       // �� �� ���� �г�

    [Header("����Ʈ UI ���")]
    [SerializeField] Text questNameText;            // �� ����Ʈ(��) �̸� �ؽ�Ʈ
    [SerializeField] RectTransform roomListPanel;   // �� ����Ʈ(��) ��� �г�
    [SerializeField] GameObject roomPrefab;         // �� ��Ͽ� ä���� ����Ʈ(��) ������

    [Header("�� ���� UI")]
    [SerializeField] private RectTransform playListPanel;      // �÷��̾� ����Ʈ�� ǥ�õ� �г�
    [SerializeField] private GameObject playerListPrefab;      // �÷��̾� ����Ʈ ������ ������

    [Header("�� �غ�, ��ŸƮ ��ư")]
    [SerializeField] private Button questStartButton;          // ������ ���� ������ �� ����ϴ� ��ư
    [SerializeField] private Button questReadyButton;          // �Ϲ� �÷��̾ �غ� ���¸� ������ �� ����ϴ� ��ư

    // �� ���� ������ �÷��̾ ���ͼ� Ȱ��ȭ �� NPC ���� ����Ʈ
    private List<NpcCtrl> activeNpcs = new List<NpcCtrl>();

    private void Awake()
    {
        createRoomPanel.gameObject.SetActive(false);
        joinRoomPanel.gameObject.SetActive(false);
        roomInfoPanel.gameObject.SetActive(false);
        
        // �� NPC ���� �̺�Ʈ ����
        NpcCtrl.OnNpcDetectionChanged += HandleNpcDetectionChanged;
    }

    private void OnDestroy()
    {
        // �� �ı��� ���� ����
        NpcCtrl.OnNpcDetectionChanged -= HandleNpcDetectionChanged;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // �� ���� �÷��̾ ������ NPC�� �־ ����Ʈ�� �߰��Ǿ��ִٸ�
            if(activeNpcs.Count > 0)
            {
                // �� �÷��̾� �±׸� ���� �༮ ��ġ ����
                Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;
                // �� �Ÿ��� �� ����� NPC�� ��� ���� �ʱ�ȭ
                NpcCtrl selectedNpc = null;
                // �� �ʱ� �ּҰŸ��� ����
                float minDistance = Mathf.Infinity;

                // �� ���࿡ ���� NPC�� �÷��̾ �������� ��
                // �� �� ����� NPC�� ��ȣ�ۿ� �ϱ� ����
                foreach(var npc in activeNpcs)
                {
                    // �� NPC�� �÷��̾� ������ �Ÿ��� �ӽú����� ����
                    float distance = Vector3.Distance(npc.transform.position, playerPos.position);
                    // �� �� ������ �Ÿ��� �ּҰŸ����� �۴ٸ� �� �����ٸ�
                    if(distance < minDistance)
                    {
                        minDistance = distance;
                        selectedNpc = npc;
                    }
                }

                // �� NPC�� Ÿ�Կ� ���� ���ҿ� �´� �г��� Ȱ��ȭ �ؼ� ����Ʈ �����̳� ����.
                if(selectedNpc != null)
                {
                    if(selectedNpc.npcType == NpcCtrl.Type.Create)
                    {
                        createRoomPanel.gameObject.SetActive(true);
                    }
                    else if(selectedNpc.npcType == NpcCtrl.Type.Join)
                    {
                        joinRoomPanel.gameObject.SetActive(true);
                    }
                }
            }
        }
        // �� ESCŰ ������ ����Ʈ ���� �гε� �� ���ϴ�.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            createRoomPanel.gameObject.SetActive(false);
            joinRoomPanel.gameObject.SetActive(false);
        }
    }

    // �� NPC ���� �̺�Ʈ �ڵ鷯: ������ NPC�� ����Ʈ�� �߰��ϰų� ����
    private void HandleNpcDetectionChanged(NpcCtrl npc, bool isActive)
    {
        if (isActive)
        {
            if(!activeNpcs.Contains(npc))
            {
                activeNpcs.Add(npc);
            }
        }
        else
        {
            if(activeNpcs.Contains(npc))
            {
                activeNpcs.Remove(npc);
            }
        }
    }

    // �ݹ��Լ��� ����� ����Դϴ�. �� ����Ʈ�� ������Ʈ �մϴ�.
    // ������ ���� ����� ����Ʈ ���� UI���� �ش� �������� ���� ��ư ��������
    // �� ��� �гο� �����մϴ�. ��ư�� �ش� �濡 ������ �� �ִ� OnClick �̺�Ʈ�� �־��ݴϴ�.
    public void RoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject roomBtn = Instantiate(roomPrefab, roomListPanel);
        roomBtn.GetComponentInChildren<Text>().text = questNameText.text;
        //roomBtn.GetComponent<Button>().onClick.AddListener(JoinRoom);
    }

    // �ݹ��Լ��� ����� ����Դϴ�. �÷��̾� ����Ʈ�� ������Ʈ �մϴ�.
    // �÷��̾ ������ �÷��̾� ������ ǥ������ �̹��� ��������
    // �÷��̾� ����Ʈ �гο� �����ϴ� ���Դϴ�.
    public void PlayerListUpdate(Player player, bool isReady)
    {
        GameObject listItem = Instantiate(playerListPrefab, playListPanel);
        listItem.GetComponentInChildren<Text>().text = player.NickName;
    }

    // ���� �÷��̾� ����Ʈ ������Ʈ�� �����Ǿ��ֽ��ϴ�. �ش� �÷��̾� ���� �����տ���
    // ȣ��Ʈ�� ���� �������� �ְ�, �÷��̾�� ���� üũ�ϴ� �̹����� �޷��ֽ��ϴ�.
    // ����Ʈ �غ� ��ư�� Ŭ���ϸ� ���� �̹����� �ʷϻ����� �����߾����ϴ�.
    // !! Ŀ����������Ƽ�� ����ؼ� isReady��� Ű�� bool���� �־�������ϴ�.
    public void IsReady(Player player, bool isReady)
    {
        //if (isReady)
        //{
        //    listItem.GetComponent<Image>().color = Color.green;
        //}
        //else
        //{
        //    listItem.GetComponent<Image>().color = Color.white;
        //}
    }

    #region �ݹ�� ������ �Լ���

    // Ư�� �÷��̾ ���� �����ϸ� ���� ����Ʈ�� ������Ʈ ���ִ� �ݹ��Լ�
    //public override void OnRoomListUpdate(List<RoomInfo> roomList)
    //{
    //    // �� ���� �г� �ʱ�ȭ
    //    foreach(Transform child in roomListPanel)
    //    {
    //        Destroy(child.gameObject);
    //    }

    //    foreach(RoomInfo room in roomList)
    //    {
    //        GameObject roomBtn = Instantiate(roomPrefab, roomListPanel);
    //        roomBtn.GetComponentInChildren<Text>().text = questNameText.text;
    //        roomBtn.GetComponent<Button>().onClick.AddListener(JoinRoom);
    //    }
    //}

    //// �÷��̾� ����Ʈ UI�� ���� ��ġ�� �Լ�
    //private void UpdatePlayerListUI()
    //{
    //    // ���� ����Ʈ �׸� ��� ����
    //    foreach (Transform child in playListPanel)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //    // ���� �濡 �ִ� ��� �÷��̾� ���� ������� ����Ʈ ����
    //    foreach (Player player in PhotonNetwork.PlayerList)
    //    {
    //        GameObject listItem = Instantiate(playerListPrefab, playListPanel);
    //        listItem.GetComponentInChildren<Text>().text = player.NickName;

    //        // �÷��̾ �غ� ���¶�� �̹��� ������ �ʷϻ����� ǥ��
    //        if (player.CustomProperties.ContainsKey("isReady") && (bool)player.CustomProperties["isReady"])
    //        {
    //            listItem.GetComponent<Image>().color = Color.green;
    //        }
    //        else
    //        {
    //            listItem.GetComponent<Image>().color = Color.white;
    //        }
    //    }
    //}

    //// �Ϲ� �÷��̾ �غ� ��ư�� ������ ȣ��Ǵ� �Լ�
    //public void OnReady()
    //{
    //    // Photon Custom Properties�� ���� ���� �÷��̾��� �غ� ���¸� true�� ����
    //    ExitGames.Client.Photon.Hashtable readyProps = new ExitGames.Client.Photon.Hashtable();
    //    readyProps["isReady"] = true;
    //    PhotonNetwork.LocalPlayer.SetCustomProperties(readyProps);
    //}

    //// ������ ���� ���� ��ư�� ������ ȣ��Ǵ� �Լ�
    //public void InGame()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        // ��� �÷��̾�� �Բ� InGame ������ ��ȯ
    //        PhotonNetwork.LoadLevel("InGame");
    //    }
    //}

    //// �濡 �������� �� ȣ��Ǵ� �ݹ�
    //public override void OnJoinedRoom()
    //{
    //    Debug.Log("�濡 �����Ͽ����ϴ�");

    //    // �����̸� ���� ���� ��ư, �Ϲ� �÷��̾�� �غ� ��ư�� Ȱ��ȭ
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        questReadyButton.gameObject.SetActive(false);
    //        questStartButton.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        questStartButton.gameObject.SetActive(false);
    //        questReadyButton.gameObject.SetActive(true);
    //    }

    //    UpdatePlayerListUI();
    //}

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

}
