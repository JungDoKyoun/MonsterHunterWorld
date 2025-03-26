using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class PhotonTest : MonoBehaviourPunCallbacks
{
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
        //PhotonNetwork.LoadLevel("younghan");
    }

    public override void OnRoomPropertiesUpdate(Hashtable hashtable)
    {
        foreach (string key in hashtable.Keys)
        {
            Debug.Log(key);
        }
    }
}
