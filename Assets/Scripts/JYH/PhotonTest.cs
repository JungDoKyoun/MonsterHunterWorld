using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

[DisallowMultipleComponent]

public class PhotonTest : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _roomText = null;

    [SerializeField]
    private Button _buttonPrefab;

    [SerializeField]
    private Transform _contentTransform = null;

    [SerializeField]
    private List<Button> _buttonList = new List<Button>();
    private Dictionary<string, Button> _dictionary = new Dictionary<string, Button>();

    private static readonly int MaxPlayerCount = 4;
    private static readonly string ReadyTag = "Ready";
    public static readonly string HuntingRoomTag = "HuntingRoom";

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
                string selection = key;
                Button button = Instantiate(_buttonPrefab, _contentTransform);
                button.onClick.AddListener(() => { JoinRoom(selection); });
                button.GetComponentInChildren<Text>().SetText(roomInfo[key].ToString());
                _dictionary.Add(key, button);
            }
        }
        foreach(string key in _dictionary.Keys)
        {
            if (roomInfo.ContainsKey(key) == false)
            {
                _dictionary[key].gameObject.SetActive(false);
            }
        }
    }

    private void JoinRoom(string userId)
    {
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null)
        {
            Player localPlayer = PhotonNetwork.LocalPlayer;
            if (localPlayer.CustomProperties.ContainsKey(HuntingRoomTag) == false)
            {
                Dictionary<int, Player> players = room.Players;
                if (string.IsNullOrEmpty(userId) == false)
                {
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
                    }
                }
                else
                {
                    Dictionary<string, int> roomInfos = new Dictionary<string, int>();
                    foreach (Player player in players.Values)
                    {
                        if (player != localPlayer)
                        {
                            Hashtable hashtable = player.CustomProperties;
                            if (hashtable.ContainsKey(HuntingRoomTag) == true)
                            {
                                string value = hashtable[HuntingRoomTag].ToString();
                                if (roomInfos.ContainsKey(value) == true)
                                {
                                    roomInfos[value] += 1;
                                }
                                else
                                {
                                    roomInfos.Add(value, 1);
                                }
                            }
                        }
                    }
                    foreach (KeyValuePair<string, int> keyValuePair in roomInfos)
                    {
                        if (keyValuePair.Value < MaxPlayerCount)
                        {
                            localPlayer.SetCustomProperties(new Hashtable() { { HuntingRoomTag, keyValuePair.Key }, { ReadyTag, false } });
                        }
                    }
                }
            }
        }
    }

    public void CreateRoom()
    {
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null)
        {
            Player localPlayer = PhotonNetwork.LocalPlayer;
            if (localPlayer.CustomProperties.ContainsKey(HuntingRoomTag) == false)
            {
                localPlayer.SetCustomProperties(new Hashtable() { { HuntingRoomTag, localPlayer.UserId }, { ReadyTag, false } });
                _roomText.SetText("방 생성 성공");
            }
        }
    }

    public void ReadyRoom()
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

    public void LeaveRoom()
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
                _roomText.SetText("방 나가기 성공");
                return;
            }
        }
        _roomText.SetText("방에 참여한 상태가 아님");
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
                PhotonNetwork.LoadLevel("younghan"); //이거 바꿔야함
            }
        }
        foreach (Button button in _buttonList)
        {
            button.SetInteractable(true);
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
                return;
            }
        }
        UpdateRoomInfo();
    }
}