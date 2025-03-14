using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _playerPrefab;

    private Dictionary<PhotonView, int> _players = new Dictionary<PhotonView, int>();

    public override void OnEnable()
    {
        base.OnEnable();
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null && PhotonNetwork.IsMasterClient == true)
        {
            Dictionary<int, Player> players = room.Players;
            foreach (KeyValuePair<int, Player> player in players)
            {
                CreateSpawnPlayer(player.Value);
            }
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        if (PhotonNetwork.IsMasterClient == true)
        {
            CreateSpawnPlayer(player);
        }
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        if (PhotonNetwork.IsMasterClient == true)
        {
            foreach (KeyValuePair<PhotonView, int> keyValuePair in _players)
            {
                if(keyValuePair.Value == player.ActorNumber)
                {
                    keyValuePair.Key.TransferOwnership(-1);
                }
            }
        }
    }

    private void CreateSpawnPlayer(Player player)
    {
        StartCoroutine(SpawnPlayerWhenConnected());
        IEnumerator SpawnPlayerWhenConnected()
        {
            yield return new WaitUntil(predicate: () => PhotonNetwork.InRoom);
            if (_playerPrefab != null)
            {
                GameObject playerObject = PhotonNetwork.InstantiateRoomObject(_playerPrefab.name, new Vector3(0, 5, 0), Quaternion.identity, 0);
                yield return new WaitUntil(predicate: () => playerObject != null);
                int actorNumber = player.ActorNumber;
                PhotonView photonView = playerObject.GetComponent<PhotonView>();
                photonView.TransferOwnership(actorNumber);
                _players[photonView] = actorNumber;
            }
        }
    }
}