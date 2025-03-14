using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private CinemachineFreeLook _freeLockCamera;
    [SerializeField]
    private GameObject _playerPrefab;

    public override void OnEnable()
    {
        base.OnEnable();
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null && PhotonNetwork.IsMasterClient == true)
        {
            Dictionary<int, Player> players = room.Players;
            foreach(KeyValuePair<int, Player> player in players)
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
        CreateSpawnPlayer(player);
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
                playerObject.GetComponent<PhotonView>().TransferOwnership(player.ActorNumber);
                photonView.RPC("SetCamera", player, playerObject.transform);
            }
        }
    }

    [PunRPC]
    private void SetCamera(Transform transform)
    {
        Debug.Log("¼ö½Å");
        _freeLockCamera.Set(transform);
    }
}