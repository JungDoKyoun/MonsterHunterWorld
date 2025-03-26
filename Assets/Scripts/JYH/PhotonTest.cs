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

    private Dictionary<string, Button> _buttonList = new Dictionary<string, Button>();

    [Serializable]
    private struct Test
    {
        public string key;
        public string value;

        public Test(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }

    [SerializeField]
    private List<Test> list = new List<Test>();

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
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
            room.SetCustomProperties(new Hashtable() { {"dd", "d"} });

            //PhotonNetwork.LoadLevel("younghan");
        }
    }

    public override void OnRoomPropertiesUpdate(Hashtable hashtable)
    {
        foreach (string key in hashtable.Keys)
        {
            if(_buttonList.ContainsKey(key) == true)
            {
                if (hashtable[key] != null)
                {
                    _buttonList[key].Set(hashtable[key].ToString());
                }
                else
                {
                    Destroy(_buttonList[key]);
                    _buttonList.Remove(key);
                }
            }
            else if(_contentTransform != null && _buttonPrefab != null)
            {
                Button button = Instantiate(_buttonPrefab, _contentTransform);
                button.Set(hashtable[key].ToString());
                _buttonList.Add(key, button);
            }
        }
        list.Clear();
        Room room = PhotonNetwork.CurrentRoom;
        if(room != null)
        {
            hashtable = room.CustomProperties;
            foreach (string key in hashtable.Keys)
            {
                string value = hashtable[key] != null ? hashtable[key].ToString() : null;
                list.Add(new Test(key, value));
            }
        }
    }
}
