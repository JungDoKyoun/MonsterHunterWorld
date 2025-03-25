using System.Linq;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public partial class MeetingRoomManager : MonoBehaviourPunCallbacks
{
    private static readonly int MaxPlayerCount = 4;
    private static readonly string RoomName = "HuntingRoom";

    private void CreateRoom()
    {
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null)
        {
            Hashtable hashtable = room.CustomProperties;
            if (hashtable != null)
            {
                int index = 1;
                while (hashtable.ContainsKey(RoomName + index) == false)
                {
                    index++;
                }
                PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { { RoomName, index } });
            }
        }
    }

    private bool TryJoinRoom(int index)
    {
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null)
        {
            Hashtable hashtable = room.CustomProperties;
            if (hashtable != null && hashtable.ContainsKey(RoomName + index) == true)
            {
                string value = hashtable[RoomName + index].ToString();
                List<string> list = value.Split(' ').ToList();
                if(list.Contains(PhotonNetwork.LocalPlayer.UserId) == false && list.Count < MaxPlayerCount)
                {
                    PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { { RoomName, index } });
                    return true;
                }
            }
        }
        return false;
    }

    public override void OnPlayerLeftRoom(Player player)
    {

    }

    public override void OnRoomPropertiesUpdate(Hashtable hashtable)
    {

    }

    //public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    //{

    //}
}