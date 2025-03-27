using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PhotonTest : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Button _buttonPrefab;
    [SerializeField]
    private Text _roomText = null;
    [SerializeField]
    private Transform _contentTransform = null;

    private Dictionary<int, Button> _dictionary = new Dictionary<int, Button>();

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    private void UpdateRoomInfo()
    {
        Dictionary<int, int> roomInfo = new Dictionary<int, int>();
        Room room = PhotonNetwork.CurrentRoom;
        if(room != null)
        {
            Dictionary<int, Player> players = room.Players;
            foreach(Player player in players.Values)
            {
                Hashtable hashtable = player.CustomProperties;
                if(hashtable.ContainsKey(RoomManager.HuntingRoomTag) == true && int.TryParse(hashtable[RoomManager.HuntingRoomTag].ToString(), out int index))
                {
                    if(roomInfo.ContainsKey(index) == true)
                    {
                        roomInfo[index] += 1;
                    }
                    else
                    {
                        roomInfo.Add(index, 1);
                    }
                }
            }
        }
        foreach(int key in roomInfo.Keys)
        {
            if(_dictionary.ContainsKey(key) == true)
            {
                _dictionary[key].Set(roomInfo[key].ToString());
                _dictionary[key].gameObject.SetActive(true);
            }
            else if(_buttonPrefab != null && _contentTransform != null)
            {
                Button button = Instantiate(_buttonPrefab, _contentTransform);
                button.GetComponentInChildren<Text>().Set(roomInfo[key].ToString());
                _dictionary.Add(key, button);
            }
        }
        foreach(KeyValuePair<int, Button> keyValuePair in _dictionary)
        {
            if (roomInfo.ContainsKey(keyValuePair.Key) == false)
            {
                keyValuePair.Value.gameObject.SetActive(false);
            }
        }
    }

    string userid = null;

    public override void OnConnectedToMaster()
    {
        userid = PhotonNetwork.LocalPlayer.UserId;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        if(userid == PhotonNetwork.LocalPlayer.UserId)
        {
            Debug.Log("동일");
        }
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        if (userid == PhotonNetwork.LocalPlayer.UserId)
        {
            Debug.Log("동일");
        }
        Room room = PhotonNetwork.CurrentRoom;
        _roomText.Set(room.Name);
        if (room != null)
        {
            Player player = PhotonNetwork.LocalPlayer;
            Debug.Log(room.Name);

            //Hashtable hashtable = player.CustomProperties;
            //if (hashtable.ContainsKey("Room") == true)
            //{
            //    PhotonNetwork.LoadLevel("younghan");
            //}
        }
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
