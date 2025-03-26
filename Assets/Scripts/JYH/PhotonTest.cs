using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PhotonTest : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform contentTransform = null;

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
