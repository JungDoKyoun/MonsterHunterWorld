using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private static readonly int MaxPlayerCount = 4;
    private static readonly string RoomName = "Room";

    public void CreateRoom()
    {
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null)
        {
            int index = 1;
            string userId = PhotonNetwork.LocalPlayer.UserId;
            Hashtable hashtable = room.CustomProperties;
            foreach(string key in hashtable.Keys)
            {
                List<string> list = hashtable[key] != null ? hashtable[key].ToString().Split(' ').ToList() : new List<string>();
                if(list.Contains(userId) == true)
                {
                    return;
                }
                if (key == RoomName + index)
                {
                    index++;
                }
            }
            room.SetCustomProperties(new Hashtable() { { RoomName + index, userId } });
        }
    }

    //[PunRPC]
    //private void CreateRoom(string userId)
    //{
    //    Room room = PhotonNetwork.CurrentRoom;
    //    Hashtable hashtable = room.CustomProperties;
    //    int index = 1;
    //    while (hashtable.ContainsKey(RoomName + index) == false || string.IsNullOrEmpty(hashtable[RoomName + index].ToString()) == false)
    //    {
    //        index++;
    //    }
    //    room.SetCustomProperties(new Hashtable() { { RoomName + index, userId } });
    //}

    //[PunRPC]
    //private void Add(string roomName, string userId)
    //{
    //    Room room = PhotonNetwork.CurrentRoom;
    //    Hashtable hashtable = room.CustomProperties;
    //    room.SetCustomProperties(new Hashtable() { { roomName, hashtable[roomName].ToString() + ' ' + userId } });
    //}

    //[PunRPC]
    //private void Remove(string roomName, string userId)
    //{
    //    Room room = PhotonNetwork.CurrentRoom;
    //    Hashtable hashtable = room.CustomProperties;
    //    List<string> list = hashtable[roomName].ToString().Split(' ').ToList();
    //    if (list.Count > 0 && list[0] == userId)
    //    {
    //        room.SetCustomProperties(new Hashtable() { { roomName, null} });
    //    }
    //    else
    //    {
    //        list.Remove(userId);
    //        StringBuilder stringBuilder = new StringBuilder();
    //        for (int i = 0; i < list.Count; i++)
    //        {
    //            if (i == 0)
    //            {
    //                stringBuilder.Append(list[i]);
    //            }
    //            else
    //            {
    //                stringBuilder.Append(' ' + list[i]);
    //            }
    //        }
    //        room.SetCustomProperties(new Hashtable() { { roomName, stringBuilder.ToString() } });
    //    }
    //}

    //[ContextMenu("¹æ »ý¼º")]
    //public bool TryCreateRoom()
    //{
    //    Room room = PhotonNetwork.CurrentRoom;
    //    if(room != null)
    //    {
    //        string userId = PhotonNetwork.LocalPlayer.UserId;
    //        Hashtable hashtable = room.CustomProperties;
    //        foreach(string value in hashtable.Values)
    //        {
    //            List<string> list = value.Split(' ').ToList();
    //            if (list.Contains(userId) == true)
    //            {
    //                return false;
    //            }
    //        }
    //        if(PhotonNetwork.IsMasterClient == true)
    //        {
    //            CreateRoom(userId);
    //        }
    //        else
    //        {
    //            photonView.RPC("CreateRoom", RpcTarget.MasterClient, userId);
    //        }
    //        return true;
    //    }
    //    return false;
    //}

    //public bool TryJoinRoom()
    //{
    //    return TryJoinRoom(null);
    //}

    //public bool TryJoinRoom(string roomName)
    //{
    //    Room room = PhotonNetwork.CurrentRoom;
    //    if (room != null)
    //    {
    //        string userId = PhotonNetwork.LocalPlayer.UserId;
    //        Hashtable hashtable = room.CustomProperties;
    //        foreach (string value in hashtable.Values)
    //        {
    //            List<string> list = value.Split(' ').ToList();
    //            if (list.Contains(userId) == true)
    //            {
    //                return false;
    //            }
    //        }
    //        if (string.IsNullOrEmpty(roomName) == true)
    //        {
    //            foreach (string key in hashtable.Keys)
    //            {
    //                List<string> list = hashtable[key].ToString().Split(' ').ToList();
    //                if(list.Count < MaxPlayerCount)
    //                {
    //                    if(PhotonNetwork.IsMasterClient == true)
    //                    {
    //                        Add(key, userId);
    //                    }
    //                    else
    //                    {
    //                        photonView.RPC("Add", RpcTarget.MasterClient, key, userId);
    //                    }
    //                    return true;
    //                }
    //            }
    //        }
    //        else if(hashtable.ContainsKey(roomName) == true)
    //        {
    //            List<string> list = hashtable[roomName].ToString().Split(' ').ToList();
    //            if (list.Count < MaxPlayerCount)
    //            {
    //                if (PhotonNetwork.IsMasterClient == true)
    //                {
    //                    Add(roomName, userId);
    //                }
    //                else
    //                {
    //                    photonView.RPC("Add", RpcTarget.MasterClient, roomName, userId);
    //                }
    //                return true;
    //            }
    //        }
    //    }
    //    return false;
    //}

    //public bool TryLeaveRoom()
    //{
    //    Room room = PhotonNetwork.CurrentRoom;
    //    if (room != null)
    //    {
    //        string userId = PhotonNetwork.LocalPlayer.UserId;
    //        Hashtable hashtable = room.CustomProperties;
    //        foreach (string key in hashtable.Keys)
    //        {
    //            List<string> list = hashtable[key].ToString().Split(' ').ToList();
    //            if (list.Contains(userId) == true)
    //            {
    //                if(PhotonNetwork.IsMasterClient == true)
    //                {
    //                    Remove(key, userId);
    //                }
    //                else
    //                {
    //                    photonView.RPC("Remove", RpcTarget.MasterClient, key, userId);
    //                }
    //                return true;
    //            }
    //        }
    //    }
    //    return false;
    //}

    //public override void OnPlayerLeftRoom(Player player)
    //{
    //    if(PhotonNetwork.IsMasterClient == true)
    //    {
    //        string userId = player.UserId;
    //        Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
    //        foreach (string key in hashtable.Keys)
    //        {
    //            List<string> list = hashtable[key].ToString().Split(' ').ToList();
    //            if (list.Contains(userId) == true)
    //            {
    //                Remove(key, userId);
    //                return;
    //            }
    //        }
    //    }
    //}
}