using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform spawnPos;
    [SerializeField] CinemachineFreeLook cam;

    private void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPos.position, spawnPos.rotation);

        player.name = PhotonNetwork.LocalPlayer.NickName;

        cam.LookAt = player.transform;
        cam.Follow = player.transform;
    }
}
