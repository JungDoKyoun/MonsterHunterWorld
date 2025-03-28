using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

[DisallowMultipleComponent]
[RequireComponent(typeof(Canvas))]
public class MeetingQuestManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _questPanel;
    [SerializeField]
    private GameObject _registerPanel;
    [SerializeField]
    private Text _purposeText;
    [SerializeField]
    private Text _itemText;
    [SerializeField]
    private Button _createButton;
    [SerializeField]
    private Transform _listPanel;
    [SerializeField]
    private PlayerController _playerPrefab;
    private PlayerController _playerController;
    [SerializeField]
    private List<NpcCtrl> _npcCtrls = new List<NpcCtrl>();

    [SerializeField]
    private string _selection = null;

    private Dictionary<string, Button> _buttonDictionary = new Dictionary<string, Button>();

    private static readonly int MaxPlayerCount = 4;
    private static readonly string ReadyTag = "Ready";
    private static readonly string HuntingRoomTag = "HuntingRoom";

    private enum Mode: byte
    {
        None,
        Create,
        Join
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
        StartCoroutine(DoStart());  //나중에 지울거임
        System.Collections.IEnumerator DoStart()
        {
            PhotonNetwork.ConnectUsingSettings();
            yield return new WaitUntil(predicate: () => PhotonNetwork.NetworkClientState == ClientState.ConnectedToMaster);
            PhotonNetwork.JoinLobby();
            yield return new WaitUntil(predicate: () => PhotonNetwork.InLobby);
            PhotonNetwork.JoinRandomOrCreateRoom();
        }
    }

    private void Update()
    {
        if (_playerController != null)
        {
            switch (_playerController.enabled)
            {
                case true:
                    Vector3 position = _playerController.transform.position;
                    NpcCtrl selectedNpc = null;
                    foreach (NpcCtrl npcCtrl in _npcCtrls)
                    {
                        if (npcCtrl.isPlayerInRange && (selectedNpc == null || Vector3.Distance(selectedNpc.transform.position, position) > Vector3.Distance(npcCtrl.transform.position, position)))
                        {
                            selectedNpc = npcCtrl;
                        }
                    }
                    if (selectedNpc != null)
                    {
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            switch (selectedNpc.npcType)
                            {
                                case NpcCtrl.Type.Create:
                                    _playerController.enabled = false;
                                    Show(Mode.Create);
                                    break;
                                case NpcCtrl.Type.Join:
                                    _playerController.enabled = false;
                                    Show(Mode.Join);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        //밖에 나가면 취소시키기
                    }
                    break;
                case false:
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        _playerController.enabled = true;
                        Show(Mode.None);
                    }
                    break;
            }
        }
    }

    private void Show(Mode mode)
    {
        switch(mode)
        {
            case Mode.None:
                _questPanel.Set(false);
                _registerPanel.Set(false);
                break;
            case Mode.Create:
                _purposeText.SetText("퀘스트 카운터");
                _itemText.SetText("집회소 퀘스트");
                _createButton.SetActive(true);
                _listPanel.SetActive(false);
                _questPanel.Set(true);
                _registerPanel.Set(false);
                break;
            case Mode.Join:
                _purposeText.SetText("퀘스트 받기");
                _itemText.SetText("집회소 퀘스트 목록");
                _createButton.SetActive(false);
                _listPanel.SetActive(true);
                _questPanel.Set(true);
                _registerPanel.Set(false);
                break;
        }
    }

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
        foreach (string key in roomInfo.Keys)
        {
            if (_buttonDictionary.ContainsKey(key) == true)
            {
                _buttonDictionary[key].SetActive(true);
            }
            else if (_createButton != null && _listPanel != null)
            {
                Button button = Instantiate(_createButton, _listPanel);
                button.onClick.AddListener(() => { _selection = key; });
                _buttonDictionary.Add(key, button);
            }
        }
        foreach (string key in _buttonDictionary.Keys)
        {
            if (roomInfo.ContainsKey(key) == false)
            {
                _buttonDictionary[key].gameObject.SetActive(false);
                if (key == _selection)
                {
                    _selection = null;
                }
            }
        }
    }

    public override void OnJoinedRoom()
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;
        Hashtable hashtable = localPlayer.CustomProperties;
        if (hashtable.ContainsKey(HuntingRoomTag) == true)
        {
            string userId = hashtable[HuntingRoomTag].ToString();
            hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
            if (hashtable.ContainsKey(HuntingRoomTag) == true && hashtable[HuntingRoomTag].ToString() == userId)
            {
                PhotonNetwork.LoadLevel("younghan"); //이거 바꿔야함
                return;
            }
        }
        if (_playerPrefab != null)
        {
            GameObject gameObject = PhotonNetwork.Instantiate(_playerPrefab.name, Vector3.zero, Quaternion.Euler(new Vector3(0, 180, 0)), 0);
            _playerController = gameObject.GetComponent<PlayerController>();
            FindObjectOfType<CinemachineFreeLook>().Set(_playerController.transform);
        }
        UpdateRoomInfo();
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        UpdateRoomInfo();
    }

    public override void OnPlayerPropertiesUpdate(Player player, Hashtable hashtable)
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;
        if (hashtable.ContainsKey(HuntingRoomTag) == true && hashtable.ContainsKey(ReadyTag) == true && hashtable[ReadyTag] != null
           && bool.TryParse(hashtable[ReadyTag].ToString(), out bool ready) == true && ready == true)
        {
            string userId = hashtable[HuntingRoomTag].ToString();
            hashtable = localPlayer.CustomProperties;
            if (hashtable.ContainsKey(HuntingRoomTag) == true && hashtable[HuntingRoomTag].ToString() == userId)
            {
                localPlayer.SetCustomProperties(new Hashtable() { { ReadyTag, null } });
                PhotonNetwork.LeaveRoom();
            }
        }
        else
        {

        }
            UpdateRoomInfo();
    }
}