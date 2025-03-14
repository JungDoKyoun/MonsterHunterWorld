using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _playerPrefab;

    private Dictionary<PlayerController, int> _playerControllers = new Dictionary<PlayerController, int>();

    private void Start()
    {
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null && PhotonNetwork.IsMasterClient == true)
        {
            Dictionary<int, Player> players = room.Players;
            foreach (KeyValuePair<int, Player> player in players)
            {
                Create(player.Value);
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
        Create(player);
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        if(PhotonNetwork.IsMasterClient == true)
        {
            foreach(KeyValuePair<PlayerController, int> keyValuePair in _playerControllers)
            {
                if(player.ActorNumber == keyValuePair.Value)
                {
                    keyValuePair.Key.ActorNumber = -1;
                }
            }
        }
    }

    private void Create(Player player)
    {
        StartCoroutine(SpawnPlayerWhenConnected());
        IEnumerator SpawnPlayerWhenConnected()
        {
            if (_playerPrefab != null)
            {
                GameObject gameObject = PhotonNetwork.InstantiateRoomObject(_playerPrefab.name, new Vector3(0, 5, 0), Quaternion.identity, 0);
                yield return new WaitUntil(predicate: () => gameObject != null);
                PlayerController playerController = gameObject.GetComponent<PlayerController>();
                if(playerController != null)
                {
                    int actorNumber = player.ActorNumber;
                    playerController.ActorNumber = actorNumber;
                    _playerControllers.Add(playerController, actorNumber);
                }
            }
        }
    }
}