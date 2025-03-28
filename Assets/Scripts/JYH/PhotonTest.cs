using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

[DisallowMultipleComponent]
[RequireComponent(typeof(RoomManager))]

public class PhotonTest : MonoBehaviourPunCallbacks
{
    private bool _hasRoomManager = false;

    private RoomManager _roomManager = null;

    private RoomManager getRoomManager
    {
        get
        {
            if(_hasRoomManager == false)
            {
                _hasRoomManager = TryGetComponent(out _roomManager); 
            }
            return _roomManager;
        }
    }

    [SerializeField]
    private Text _roomText = null;

    [SerializeField]
    private Button _buttonPrefab;

    [SerializeField]
    private Transform _contentTransform = null;

    [SerializeField]
    private List<Button> _buttonList = new List<Button>();
    private Dictionary<string, Button> _dictionary = new Dictionary<string, Button>();

    [SerializeField]
    private string _selection = null;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
        foreach(Button button in _buttonList)
        {
            button.SetInteractable(false);
        }
    }

    private void UpdateRoomInfo()
    {
        Dictionary<string, int> roomInfo = new Dictionary<string, int>();
        Room room = PhotonNetwork.CurrentRoom;
        if(room != null)
        {
            Dictionary<int, Player> players = room.Players;
            foreach(Player player in players.Values)
            {
                Hashtable hashtable = player.CustomProperties;
                if(hashtable.ContainsKey(RoomManager.HuntingRoomTag) == true)
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
        foreach(string key in roomInfo.Keys)
        {
            if(_dictionary.ContainsKey(key) == true)
            {
                _dictionary[key].SetText(roomInfo[key].ToString());
                _dictionary[key].gameObject.SetActive(true);
            }
            else if(_buttonPrefab != null && _contentTransform != null)
            {
                Button button = Instantiate(_buttonPrefab, _contentTransform);
                button.onClick.AddListener(() => { _selection = key; });
                button.GetComponentInChildren<Text>().SetText(roomInfo[key].ToString());
                _dictionary.Add(key, button);
            }
        }
        foreach(string key in _dictionary.Keys)
        {
            if (roomInfo.ContainsKey(key) == false)
            {
                _dictionary[key].gameObject.SetActive(false);
                if(key == _selection)
                {
                    _selection = null;
                }
            }
        }
    }

    public void CreateRoom()
    {
        bool done = getRoomManager.TryCreateQuest();
        if(done == true)
        {
            _roomText.SetText("방 생성 성공");
        }
        else
        {
            Room room = PhotonNetwork.CurrentRoom;
            if(room != null)
            {
                _roomText.SetText("이미 방에 참여 중");
            }
            else
            {
                _roomText.SetText("집회소에 없음");
            }
        }
    }

    public void JoinRoom()
    {
        bool done = getRoomManager.TryJoinQuest(_selection);
        if(done == true)
        {
            _roomText.SetText("방 참가 성공");
        }
        else
        {
            _roomText.SetText("방 참가 실패");
        }
    }

    public void ReadyRoom()
    {
        bool done = getRoomManager.TryReadyQuest();
        if(done == true)
        {

        }
        else
        {

        }
    }

    public void LeaveRoom()
    {
        bool done = getRoomManager.TryLeaveQuest();
        if (done == true)
        {
            _roomText.SetText("방 나가기 성공");
        }
        else
        {
            _roomText.SetText("방에 참여한 상태가 아님");
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
        if (hashtable.ContainsKey(RoomManager.HuntingRoomTag) == false)
        {
            PhotonNetwork.JoinRandomOrCreateRoom();
        }
    }

    public override void OnJoinedRoom()
    {
        foreach (Button button in _buttonList)
        {
            button.SetInteractable(true);
        }
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