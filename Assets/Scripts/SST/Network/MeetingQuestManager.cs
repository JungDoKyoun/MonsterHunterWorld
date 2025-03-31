using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using SimpleJSON;

[DisallowMultipleComponent]
[RequireComponent(typeof(Canvas))]
public class MeetingQuestManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _questPanel;
    [SerializeField]
    private GameObject _registerPanel;
    [SerializeField]
    private Transform _listPanel;
    [SerializeField]
    private Transform[] _questTransforms = new Transform[(byte)Mode.End];
    [SerializeField]
    private Button _createButton;
    private Dictionary<string, Button> _buttonDictionary = new Dictionary<string, Button>();
    [SerializeField]
    private Text _purposeText;
    [SerializeField]
    private Text _itemText;
    [SerializeField]
    private Text _hostText;
    [SerializeField]
    private PlayerController _playerPrefab;
    private PlayerController _playerController;

    [Serializable]
    private struct Member
    {
        [SerializeField]
        private Transform root;
        [SerializeField]
        private Text text;
        [SerializeField]
        private Image image;

        public void Hide()
        {
            text.SetText(null);
            root.SetActive(false);
        }

        public void Show(string nickname, bool value)
        {
            text.SetText(nickname);
            if (value == true)
            {
                image.SetColor(Color.green);
            }
            else
            {
                image.SetColor(Color.red);
            }
            root.SetActive(true);
        }
    }

    [SerializeField]
    private Member[] _members = new Member[MaxPlayerCount];
    [SerializeField, Range(0, 10)]
    private float InteractionRange = 2f;
    private bool _hasRoom = false;

    [SerializeField] 
    private GameObject loadingObject;
    [SerializeField] 
    private Image loadingImage;                // 회전할 로딩 이미지
    [SerializeField] 
    private Text loadingText;                  // 깜빡이는 로딩 텍스트

    private static readonly int MaxPlayerCount = 4;
    private static readonly float BlinkSpeed = 10f;             // 텍스트 깜빡임 속도
    private static readonly string UserIdTag = "UserId";
    private static readonly string ReadyTag = "Ready";
    private static readonly string HuntingRoomTag = "HuntingRoom";

    private enum Mode: byte
    {
        Create,
        Join,
        Box,
        End
    }

#if UNITY_EDITOR
    [SerializeField]
    private Mode mode = Mode.End;

    private void OnValidate()
    {
        Show(mode);
        foreach (Member member in _members)
        {
            member.Hide();
        }
    }

    [SerializeField]
    private Color _gizmoColor = Color.blue;

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        foreach(Transform transform in _questTransforms)
        {
            if(transform != null)
            {
                Gizmos.DrawWireSphere(transform.position, InteractionRange);
            }
        }
    }
#endif

    private bool _quickPlay = false;

    private bool _hasCinemachineFreeLook = false;

    private CinemachineFreeLook _cinemachineFreeLook = null;

    private CinemachineFreeLook getCinemachineFreeLook
    {
        get
        {
            if(_hasCinemachineFreeLook == false)
            {
                _hasCinemachineFreeLook = true;
                _cinemachineFreeLook = FindObjectOfType<CinemachineFreeLook>();
            }
            return _cinemachineFreeLook;
        }
    }


    private void Awake()
    {
        if(_createButton != null)
        {
            _createButton.onClick.AddListener(CreateQuest);
        }
        if (PhotonNetwork.InRoom == true)
        {
            Player localPlayer = PhotonNetwork.LocalPlayer;
            localPlayer.SetCustomProperties(new Hashtable() { { UserIdTag, localPlayer.UserId } });
            if (_playerPrefab != null)
            {
                GameObject gameObject = PhotonNetwork.Instantiate(_playerPrefab.name, Vector3.zero, Quaternion.Euler(new Vector3(0, 180, 0)), 0);
                _playerController = gameObject.GetComponent<PlayerController>();
                getCinemachineFreeLook.Set(_playerController.transform);
            }
        }
        else
        {
            _quickPlay = true;
            StartCoroutine(DoStart());
            System.Collections.IEnumerator DoStart()
            {
                PhotonNetwork.ConnectUsingSettings();
                yield return new WaitUntil(predicate: () => PhotonNetwork.NetworkClientState == ClientState.ConnectedToMaster);
                PhotonNetwork.JoinLobby();
                yield return new WaitUntil(predicate: () => PhotonNetwork.InLobby);
                PhotonNetwork.JoinRandomOrCreateRoom();
            }
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
                    int index = -1;
                    if (_hasRoom == false)
                    {
                        int length = _questTransforms.Length;
                        for(int i = 0; i < length; i++)
                        {
                            if (_questTransforms[i] != null)
                            {
                                float distance = Vector3.Distance(position, _questTransforms[i].position);
                                if (distance < InteractionRange && (index == -1 || Vector3.Distance(position, _questTransforms[index].position) > distance))
                                {
                                    index = i;
                                }
                            }
                        }
                    }
                    switch((Mode)index)
                    {
                        case Mode.Create:
                            _playerController.Show(PlayerInteraction.State.CreateQuest);
                            if (Input.GetKeyDown(KeyCode.F))
                            {
                                _playerController.enabled = false;
                                Show(Mode.Create);
                            }
                            break;
                        case Mode.Join:
                            _playerController.Show(PlayerInteraction.State.JoinQuest);
                            if (Input.GetKeyDown(KeyCode.F))
                            {
                                _playerController.enabled = false;
                                Show(Mode.Join);
                            }
                            break;
                        case Mode.Box:
                            _playerController.Show(PlayerInteraction.State.Box);
                            if (Input.GetKeyDown(KeyCode.F))
                            {
                                _playerController.enabled = false;
                                getCinemachineFreeLook.SetEnabled(false);
                                _playerController.Show(PlayerInteraction.State.Hide);
                                UIManager.Instance.StackUIOpen(UIType.AllVillageUI);
                            }
                            break;
                        default:
                            _playerController.Show(PlayerInteraction.State.Hide);
                            break;
                    }
                    break;
                case false:
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        getCinemachineFreeLook.SetEnabled(true);
                        _playerController.enabled = true;
                        Show(Mode.End);
                    }
                    break;
            }
        }
    }

    private void Show(Mode mode)
    {
        switch(mode)
        {
            case Mode.Create:
                _purposeText.SetText("퀘스트 카운터");
                _itemText.SetText("집회소 퀘스트");
                _createButton.SetActive(true);
                _listPanel.SetActive(false);
                _questPanel.Set(true);
                break;
            case Mode.Join:
                _purposeText.SetText("퀘스트 받기");
                _itemText.SetText("집회소 퀘스트 목록");
                _createButton.SetActive(false);
                _listPanel.SetActive(true);
                _questPanel.Set(true);
                break;
            default:
                _questPanel.Set(false);
                break;
        }
    }

    private void UpdateRoomInfo()
    {
        List<(string, bool)> list = new List<(string, bool)>();
        Dictionary<string, int> roomInfo = new Dictionary<string, int>();
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null)
        {
            Hashtable hashtable = PhotonNetwork.LocalPlayer.CustomProperties;
            string roomName = hashtable.ContainsKey(HuntingRoomTag) == true && hashtable[HuntingRoomTag] != null ? hashtable[HuntingRoomTag].ToString() : null;
            Dictionary<int, Player> players = room.Players;
            foreach (Player player in players.Values)
            {
                hashtable = player.CustomProperties;
                if (hashtable.ContainsKey(HuntingRoomTag) == true)
                {
                    string userId = hashtable[HuntingRoomTag].ToString();
                    if(roomName == userId)
                    {
                        if(userId == hashtable[UserIdTag].ToString())
                        {
                            list.Insert(0, (player.NickName, true));
                        }
                        else
                        {
                            bool ready = hashtable.ContainsKey(ReadyTag) == true && hashtable[ReadyTag] != null && bool.TryParse(hashtable[ReadyTag].ToString(), out bool value) == true ? value : false;
                            list.Add((player.NickName, ready));
                        }
                    }
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
            string userId = key;
            if (_buttonDictionary.ContainsKey(key) == true)
            {
                _buttonDictionary[key].SetActive(true);
            }
            else if (_createButton != null && _listPanel != null)
            {
                Button button = Instantiate(_createButton, _listPanel);
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => { JoinQuest(userId); });
                _buttonDictionary.Add(key, button);
            }
        }
        foreach (string key in _buttonDictionary.Keys)
        {
            if (roomInfo.ContainsKey(key) == false)
            {
                _buttonDictionary[key].gameObject.SetActive(false);
            }
        }
        for(int i = 0; i < _members.Length; i++)
        {
            if(i < list.Count)
            {
                _members[i].Show(list[i].Item1, list[i].Item2);
            }
            else
            {
                _members[i].Hide();
            }
        }
    }

    private void CreateQuest()
    {
        if (_hasRoom == false)
        {
            Room room = PhotonNetwork.CurrentRoom;
            if (room != null)
            {
                Player localPlayer = PhotonNetwork.LocalPlayer;
                if (localPlayer.CustomProperties.ContainsKey(HuntingRoomTag) == false)
                {
                    localPlayer.SetCustomProperties(new Hashtable() { { HuntingRoomTag, localPlayer.UserId }, { ReadyTag, false } });
                    _hasRoom = true;
                    _playerController.enabled = true;
                    Show(Mode.End);
                    _hostText.SetText("퀘스트 시작");
                    _registerPanel.SetActive(true);
                }
            }
        }
    }

    private void JoinQuest(string userId)
    {
        if (_hasRoom == false)
        {
            Room room = PhotonNetwork.CurrentRoom;
            if (room != null)
            {
                Player localPlayer = PhotonNetwork.LocalPlayer;
                if (localPlayer.CustomProperties.ContainsKey(HuntingRoomTag) == false)
                {
                    Dictionary<int, Player> players = room.Players;
                    int count = 0;
                    foreach (Player player in players.Values)
                    {
                        if (player != localPlayer)
                        {
                            Hashtable hashtable = player.CustomProperties;
                            if (hashtable.ContainsKey(HuntingRoomTag) == true && hashtable[HuntingRoomTag].ToString() == userId)
                            {
                                count++;
                            }
                        }
                    }
                    if (count > 0 && count < MaxPlayerCount)
                    {
                        localPlayer.SetCustomProperties(new Hashtable() { { HuntingRoomTag, userId }, { ReadyTag, false } });
                        _hasRoom = true;
                        _playerController.enabled = true;
                        Show(Mode.End);
                        _hostText.SetText("퀘스트 준비");
                        _registerPanel.SetActive(true);
                    }
                }
            }
        }
    }

    public void ReadyQuest()
    {
        if (_hasRoom == true)
        {
            Room room = PhotonNetwork.CurrentRoom;
            if (room != null)
            {
                Player localPlayer = PhotonNetwork.LocalPlayer;
                Hashtable hashtable = localPlayer.CustomProperties;
                if (hashtable.ContainsKey(HuntingRoomTag) == true)
                {
                    string userId = hashtable[HuntingRoomTag].ToString();
                    if (userId == localPlayer.UserId)
                    {
                        Dictionary<int, Player> players = room.Players;
                        foreach (Player player in players.Values)
                        {
                            if (player != localPlayer)
                            {
                                hashtable = player.CustomProperties;
                                if (hashtable.ContainsKey(HuntingRoomTag) == true && hashtable[HuntingRoomTag].ToString() == userId &&
                                    (hashtable.ContainsKey(ReadyTag) == false || bool.TryParse(hashtable[ReadyTag].ToString(), out bool ready) == false || ready == false))
                                {
                                    return;
                                }
                            }
                        }
                        localPlayer.SetCustomProperties(new Hashtable() { { HuntingRoomTag, userId }, { ReadyTag, true } });
                    }
                    else
                    {
                        string value = hashtable.ContainsKey(ReadyTag) == true ? hashtable[ReadyTag].ToString() : null;
                        bool ready = bool.TryParse(value, out bool result) == true ? result : false;
                        localPlayer.SetCustomProperties(new Hashtable() { { ReadyTag, !ready } });
                    }
                }
            }
        }
    }

    public void LeaveQuest()
    {
        if (_hasRoom == true)
        {
            Room room = PhotonNetwork.CurrentRoom;
            if (room != null)
            {
                Player localPlayer = PhotonNetwork.LocalPlayer;
                Hashtable hashtable = localPlayer.CustomProperties;
                if (hashtable.ContainsKey(HuntingRoomTag) == true)
                {
                    string userId = hashtable[HuntingRoomTag].ToString();
                    localPlayer.SetCustomProperties(new Hashtable() { { HuntingRoomTag, null }, { ReadyTag, null } });
                    if (userId == localPlayer.UserId)
                    {
                        Dictionary<int, Player> players = room.Players;
                        foreach (Player player in players.Values)
                        {
                            if (localPlayer != player)
                            {
                                hashtable = player.CustomProperties;
                                if (hashtable.ContainsKey(HuntingRoomTag) == true && hashtable[HuntingRoomTag].ToString() == userId)
                                {
                                    player.SetCustomProperties(new Hashtable() { { HuntingRoomTag, null }, { ReadyTag, null } });
                                }
                            }
                        }
                    }
                    _hasRoom = false;
                    _registerPanel.SetActive(false);
                }
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;
        Hashtable hashtable = localPlayer.CustomProperties;
        if (hashtable.ContainsKey(HuntingRoomTag) == false)
        {
            PhotonNetwork.JoinRandomOrCreateRoom();
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomInfos)
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;
        Hashtable hashtable = localPlayer.CustomProperties;
        if (hashtable.ContainsKey(HuntingRoomTag) == true)
        {
            string userId = hashtable[HuntingRoomTag].ToString();
            if (userId == localPlayer.UserId) //방장인 경우 방 만들어야함
            {
                List<string> list = new List<string>();
                foreach (RoomInfo roomInfo in roomInfos)
                {
                    list.Add(roomInfo.Name);
                }
                int index = 1;
                while (true)
                {
                    string roomName = HuntingRoomTag + index;
                    if (list.Contains(roomName) == true)
                    {
                        index++;
                    }
                    else
                    {
                        RoomOptions roomOptions = new RoomOptions
                        {
                            MaxPlayers = MaxPlayerCount,
                            CustomRoomProperties = new Hashtable() { { HuntingRoomTag, userId } },
                            CustomRoomPropertiesForLobby = new string[] { HuntingRoomTag }
                        };
                        PhotonNetwork.CreateRoom(roomName, roomOptions);
                        break;
                    }
                }
            }
            else //입장자는 방을 조회해서 참여
            {
                foreach (RoomInfo roomInfo in roomInfos)
                {
                    hashtable = roomInfo.CustomProperties;
                    if (hashtable.ContainsKey(HuntingRoomTag) == true && hashtable[HuntingRoomTag].ToString() == userId)
                    {
                        PhotonNetwork.JoinRoom(roomInfo.Name);
                        break;
                    }
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
                PhotonNetwork.LoadLevel("ALLTestScene");
                return;
            }
        }
        if (_quickPlay == true && _playerPrefab != null)
        {
            GameObject gameObject = PhotonNetwork.Instantiate(_playerPrefab.name, Vector3.zero, Quaternion.Euler(new Vector3(0, 180, 0)), 0);
            _playerController = gameObject.GetComponent<PlayerController>();
            getCinemachineFreeLook.Set(_playerController.transform);
            localPlayer.SetCustomProperties(new Hashtable() { { UserIdTag, localPlayer.UserId } });
        }
        UpdateRoomInfo();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;
        Hashtable hashtable = localPlayer.CustomProperties;
        if (hashtable.ContainsKey(HuntingRoomTag) == true)
        {
            Dictionary<int, Player> players = PhotonNetwork.CurrentRoom.Players;
            foreach (Player player in players.Values)
            {
                if (player.UserId == hashtable[HuntingRoomTag].ToString())
                {
                    return;
                }
            }
            if (PhotonNetwork.InRoom == true)
            {
                localPlayer.SetCustomProperties(new Hashtable() { { HuntingRoomTag, null }, { ReadyTag, null } });
            }
        }
        UpdateRoomInfo();
    }

    public override void OnPlayerPropertiesUpdate(Player player, Hashtable hashtable)
    {
        if (hashtable.ContainsKey(HuntingRoomTag) == true && hashtable.ContainsKey(ReadyTag) == true && hashtable[ReadyTag] != null
           && bool.TryParse(hashtable[ReadyTag].ToString(), out bool ready) == true && ready == true)
        {
            string userId = hashtable[HuntingRoomTag].ToString();
            Player localPlayer = PhotonNetwork.LocalPlayer;
            hashtable = localPlayer.CustomProperties;
            if (hashtable.ContainsKey(HuntingRoomTag) == true && hashtable[HuntingRoomTag].ToString() == userId)
            {
                localPlayer.SetCustomProperties(new Hashtable() { { ReadyTag, null } });
                PhotonNetwork.LeaveRoom();
                loadingObject.Set(true);
                StartCoroutine(BlinkText());
                StartCoroutine(RotateImage());
                System.Collections.IEnumerator RotateImage()
                {
                    while (true)
                    {
                        loadingImage.transform.Rotate(0, 0, -140f * Time.deltaTime);
                        yield return null;
                    }
                }
                System.Collections.IEnumerator BlinkText()
                {
                    Color basicColor = loadingText.color;
                    while (true)
                    {
                        float alpha = Mathf.PingPong(Time.deltaTime * BlinkSpeed, 1f);
                        loadingText.color = new Color(basicColor.r, basicColor.g, basicColor.b, alpha);
                        yield return null;
                    }
                }
                return;
            }
        }
        UpdateRoomInfo();
    }
}