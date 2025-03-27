using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

// �� ����Ʈ ����, ����Ʈ ����, ������ �� ��� ������Ʈ, �� ���� �����ϴ� ����
[DisallowMultipleComponent]
[RequireComponent(typeof(Canvas))]
public class MeetingQuestManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text purposeText;
    [SerializeField]
    private Text itemText;


    [Header("����Ʈ �г�")]
    [SerializeField] RectTransform createRoomPanel;     // �� ����Ʈ ���� �г�
    [SerializeField] RectTransform joinRoomPanel;       // �� ����Ʈ ���� �г�
    [SerializeField] RectTransform roomInfoPanel;       // �� �� ���� �г�

    [Header("����Ʈ UI ���")]
    [SerializeField] Text questNameText;            // �� ����Ʈ(��) �̸� �ؽ�Ʈ
    [SerializeField] RectTransform roomListPanel;   // �� ����Ʈ(��) ��� �г�
    [SerializeField] Button roomPrefab;             // �� ��Ͽ� ä���� ����Ʈ(��) ������ ��ư

    [Header("�� ���� UI")]
    [SerializeField] private RectTransform playListPanel;      // �÷��̾� ����Ʈ�� ǥ�õ� �г�
    [SerializeField] private GameObject playerListPrefab;      // �÷��̾� ����Ʈ ������ ������

    [Header("�� �غ�, ��ŸƮ ��ư")]
    [SerializeField] private Button questStartButton;          // ������ ���� ������ �� ����ϴ� ��ư
    [SerializeField] private Button questReadyButton;          // �Ϲ� �÷��̾ �غ� ���¸� ������ �� ����ϴ� ��ư

    // �� ���� ������ �÷��̾ ���ͼ� Ȱ��ȭ �� NPC ���� ����Ʈ
    private List<NpcCtrl> activeNpcs = new List<NpcCtrl>();
    private Dictionary<string, Button> buttonDictionary = new Dictionary<string, Button>();

    private enum Mode: byte
    {
        None,
        Create,
        Join,
        Wait
    }

#if UNITY_EDITOR

    [SerializeField]
    private Mode mode = Mode.None;

    private void OnValidate()
    {
        Show(mode);
    }
#endif

    private void Awake()
    {    
        NpcCtrl.OnNpcDetectionChanged += HandleNpcDetectionChanged; //NPC ���� �̺�Ʈ ����
    }

    private void OnDestroy()
    {
        NpcCtrl.OnNpcDetectionChanged -= HandleNpcDetectionChanged; //�ı��� ���� ����
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

    private void Show(Mode mode)
    {
        switch(mode)
        {
            case Mode.None:
                break;
            case Mode.Create:
                purposeText.Set("����Ʈ ī����");
                itemText.Set("��ȸ�� ����Ʈ");
                break;
            case Mode.Join:
                purposeText.Set("����Ʈ �ޱ�");
                itemText.Set("��ȸ�� ����Ʈ ���");
                break;
        }
    }


    // �÷��̾� ����Ʈ UI�� ���� ��ġ�� �Լ�
    private void UpdateRoomInfo()
    {
        Dictionary<string, int> roomInfo = new Dictionary<string, int>();
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null)
        {
            Dictionary<int, Player> players = room.Players;
            foreach (Player player in players.Values)
            {
                Hashtable hashtable = player.CustomProperties;
                if (hashtable.ContainsKey(RoomManager.HuntingRoomTag) == true)
                {
                    string userId = hashtable[RoomManager.HuntingRoomTag].ToString();
                    if (roomInfo.ContainsKey(userId) == true)
                    {
                        roomInfo[userId] += 1;
                    }
                    else
                    {
                        roomInfo.Add(userId, 1);
                    }
                }
            }
        }
        //foreach (string key in roomInfo.Keys)
        //{
        //    if (buttonDictionary.ContainsKey(key) == true)
        //    {
        //        buttonDictionary[key].Set(roomInfo[key].ToString());
        //        buttonDictionary[key].gameObject.SetActive(true);
        //    }
        //    else if (roomPrefab != null && roomListPanel != null)
        //    {
        //        Button button = Instantiate(roomPrefab, roomListPanel);
        //        button.onClick.AddListener(() => { _selection = key; });
        //        button.GetComponentInChildren<Text>().Set(roomInfo[key].ToString());
        //        buttonDictionary.Add(key, button);
        //    }
        //}
        //foreach (string key in buttonDictionary.Keys)
        //{
        //    if (roomInfo.ContainsKey(key) == false)
        //    {
        //        buttonDictionary[key].gameObject.SetActive(false);
        //        if (key == _selection)
        //        {
        //            _selection = null;
        //        }
        //    }
        //}
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

    public override void OnJoinedRoom()
    {

        UpdateRoomInfo();
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        UpdateRoomInfo();
    }
    public override void OnPlayerPropertiesUpdate(Player player, Hashtable hashtable)
    {
        UpdateRoomInfo();
    }
}