using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class RoomManager : MonoBehaviourPunCallbacks
{
    private static readonly int MaxPlayerCount = 4;
    private static readonly string ReadyTag = "Ready";
    public static readonly string HuntingRoomTag = "HuntingRoom";

    private List<RoomInfo> _roomList = new List<RoomInfo>();

    [PunRPC]
    private void LeaveRoom(string userId)
    {
        Hashtable hashtable = PhotonNetwork.LocalPlayer.CustomProperties;
        if (hashtable.ContainsKey(HuntingRoomTag) == true && hashtable[HuntingRoomTag].ToString() == userId)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public bool TryCreateRoom()
    {
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null)
        {
            Player localPlayer = PhotonNetwork.LocalPlayer;
            if (localPlayer.CustomProperties.ContainsKey(HuntingRoomTag) == false)
            {
                localPlayer.SetCustomProperties(new Hashtable() { { HuntingRoomTag, localPlayer.UserId }, {ReadyTag, true}});
                return true;
            }
        }
        return false;
    }

    public bool TryJoinRoom()
    {
        return TryJoinRoom(null);
    }

    public bool TryJoinRoom(string userId)
    {
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null)
        {
            Player localPlayer = PhotonNetwork.LocalPlayer;
            if (localPlayer.CustomProperties.ContainsKey(HuntingRoomTag) == false)
            {
                Dictionary<int, Player> players = room.Players;
                if(string.IsNullOrEmpty(userId) == false)
                {
                    int count = 0;
                    foreach(Player player in players.Values)
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
                    if(count > 0 && count < MaxPlayerCount)
                    {
                        localPlayer.SetCustomProperties(new Hashtable() { { HuntingRoomTag, userId }, { ReadyTag, false } });
                        return true;
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
                    foreach(KeyValuePair<string, int> keyValuePair in roomInfos)
                    {
                        if(keyValuePair.Value < MaxPlayerCount)
                        {
                            localPlayer.SetCustomProperties(new Hashtable() { { HuntingRoomTag, keyValuePair.Key }, { ReadyTag, false } });
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public bool TryLeaveRoom()
    {
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null)
        {
            Player localPlayer = PhotonNetwork.LocalPlayer;
            Hashtable hashtable = localPlayer.CustomProperties;
            if (hashtable.ContainsKey(HuntingRoomTag) == true)
            {
                string userId = hashtable[HuntingRoomTag].ToString();
                localPlayer.SetCustomProperties(new Hashtable() { {HuntingRoomTag, null}, { ReadyTag, null } });
                if(userId == localPlayer.UserId)
                {
                    Dictionary<int, Player> players = room.Players;
                    foreach(Player player in players.Values)
                    {
                        if(localPlayer != player)
                        {
                            hashtable = player.CustomProperties;
                            if (hashtable.ContainsKey(HuntingRoomTag) == true && hashtable[HuntingRoomTag].ToString() == userId)
                            {
                                player.SetCustomProperties(new Hashtable() { { HuntingRoomTag, null }, { ReadyTag, null } });
                            }
                        }
                    }
                }
                return true;
            }
        }
        return false;
    }

    public bool TryStartRoom()
    {
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null)
        {
            Player localPlayer = PhotonNetwork.LocalPlayer;
            Hashtable hashtable = localPlayer.CustomProperties;
            if(hashtable.ContainsKey(HuntingRoomTag) == true && hashtable[HuntingRoomTag].ToString() == localPlayer.UserId)
            {
                LeaveRoom(localPlayer.UserId);
                photonView.RPC("LeaveRoom", RpcTarget.Others, localPlayer.UserId);
            }
        }
        return false;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _roomList = roomList;
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        string userId = player.UserId;
        Dictionary<int, Player> players = PhotonNetwork.CurrentRoom.Players;
        foreach(Player otherPlayer in players.Values)
        {
            Hashtable hashtable = otherPlayer.CustomProperties;
            if(hashtable.ContainsKey(HuntingRoomTag) == true && hashtable[HuntingRoomTag].ToString() == userId)
            {
                otherPlayer.SetCustomProperties(new Hashtable() { { HuntingRoomTag, null }, { ReadyTag, null } });
            }
        }
    }
}