using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

[DisallowMultipleComponent]
public class RoomManager : MonoBehaviourPunCallbacks
{
    private static readonly int MaxPlayerCount = 4;
    private static readonly string ReadyTag = "Ready";
    public static readonly string HuntingRoomTag = "HuntingRoom";

    public bool TryCreateRoom()
    {
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null)
        {
            Player localPlayer = PhotonNetwork.LocalPlayer;
            if (localPlayer.CustomProperties.ContainsKey(HuntingRoomTag) == false)
            {
                localPlayer.SetCustomProperties(new Hashtable() { { HuntingRoomTag, localPlayer.UserId }, {ReadyTag, false}});
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

    public bool TryReadyRoom()
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
                                return false;
                            }
                        }
                    }
                    localPlayer.SetCustomProperties(new Hashtable() { {HuntingRoomTag, userId},{ ReadyTag, true } });
                    return true;
                }
                else
                {
                    string value = hashtable.ContainsKey(ReadyTag) == true ? hashtable[ReadyTag].ToString() : null;
                    bool ready = bool.TryParse(value, out bool result) == true ? result : false;
                    localPlayer.SetCustomProperties(new Hashtable() { { ReadyTag, !ready } });
                    return true;
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
                foreach(RoomInfo roomInfo in roomInfos)
                {
                    list.Add(roomInfo.Name);
                }
                int index = 1;
                while(true)
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
                            CustomRoomProperties = new Hashtable() { { HuntingRoomTag, userId } }
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
                    foreach(string key in hashtable.Keys)
                    {
                        Debug.Log(key);
                    }
                    Debug.Log(hashtable.Count);
                    if(hashtable.ContainsKey(HuntingRoomTag) == true && hashtable[HuntingRoomTag].ToString() == userId)
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
            if(hashtable.ContainsKey(HuntingRoomTag) == true && hashtable[HuntingRoomTag].ToString() == userId)
            {
                PhotonNetwork.LoadLevel("younghan"); //이거 바꿔야함
            }
        }
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;
        Hashtable hashtable = localPlayer.CustomProperties;
        if (hashtable.ContainsKey(HuntingRoomTag) == true && hashtable[HuntingRoomTag].ToString() == player.UserId)
        {
            Debug.Log("방장 나감");
            localPlayer.SetCustomProperties(new Hashtable() { { HuntingRoomTag, null }, { ReadyTag, null } });
        }
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
            }
        }
    }
}