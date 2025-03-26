using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
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

    private Dictionary<int, Button> _buttonList = new Dictionary<int, Button>();

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
                if(hashtable.ContainsKey("Room") == true && int.TryParse(hashtable["Room"].ToString(), out int index))
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

    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Room room = PhotonNetwork.CurrentRoom;
        _roomText.Set(room.Name);
        if (room != null)
        {
            Player player = PhotonNetwork.LocalPlayer;



            Hashtable hashtable = player.CustomProperties;
            if (hashtable.ContainsKey("Room") == true)
            {
                PhotonNetwork.LoadLevel("younghan");
            }
        }
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        UpdateRoomInfo();
    }

    public override void OnPlayerPropertiesUpdate(Player player, Hashtable hashtable)
    {
        Debug.Log("입장");
        if (hashtable.ContainsKey("Room") == true)
        {
            Debug.Log("삭제");
            Room room = PhotonNetwork.CurrentRoom;
            if (room != null)
            {
                Dictionary<int, Player> players = room.Players;
                foreach(KeyValuePair<int, Player> keyValuePair in players)
                {

                }
            }
        }
    }
}
