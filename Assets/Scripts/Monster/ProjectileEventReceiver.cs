using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEventReceiver : MonoBehaviour, IOnEventCallback
{
    public MonsterProjectileSpawnManager spawnManager;

    void OnEnable() => PhotonNetwork.AddCallbackTarget(this);
    void OnDisable() => PhotonNetwork.RemoveCallbackTarget(this);

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 100)
        {
            string projectileId = (string)photonEvent.CustomData;

            MonsterProjectile[] all = FindObjectsOfType<MonsterProjectile>();
            foreach (var proj in all)
            {
                if (proj.ProjectileID == projectileId)
                {
                    spawnManager.ReturnProjectile(proj.ProjectileType, proj);
                    break;
                }
            }
        }
    }
}
