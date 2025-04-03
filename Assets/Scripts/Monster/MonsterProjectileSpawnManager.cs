using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;

public enum ProjectileType
{
    FireBall
}

public class MonsterProjectileSpawnManager : MonoBehaviour
{
    private Dictionary<ProjectileType, ObjectPool<MonsterProjectile>> MonsterProjectilePool = new();

    private Dictionary<string, MonsterProjectile> _projectileInstances = new();

    //public void LoadProjectilesFromAddressable(string label, List<MonsterProjectileData> projectileDatas)
    //{
    //    Addressables.LoadAssetsAsync<GameObject>(label, prefab =>
    //    {
    //        var projectile = prefab.GetComponent<MonsterProjectile>();
    //        var type = projectile.ProjectileType;
    //        var match = projectileDatas.Find(x => x.ProjectileType == type);

    //        if (match != null)
    //        {
    //            projectile.SetData(match);
    //            if (!MonsterProjectilePool.ContainsKey(type))
    //            {
    //                CreatePool(projectile, match.defaultCapacity, type);
    //            }
    //        }
    //    });
    //}

    public void CreatePool(string name, int cap, ProjectileType type)
    {
        Debug.Log("µé¾î¿È");
        var pool = new ObjectPool<MonsterProjectile>(
            createFunc: () =>
            {
                GameObject go;
                if (PhotonNetwork.IsMasterClient)
                {
                    go = PhotonNetwork.Instantiate(name, Vector3.zero, Quaternion.identity);
                }
                else
                {
                    var prefab = Resources.Load<GameObject>(name);
                    go = Instantiate(prefab);
                    var photonView = go.GetComponent<PhotonView>();
                    if (photonView != null)
                        Destroy(photonView);
                }

                return go.GetComponent<MonsterProjectile>();
            },
            actionOnGet: obj => obj.gameObject.SetActive(true),
            actionOnRelease: obj => obj.gameObject.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            defaultCapacity: cap);

        MonsterProjectilePool[type] = pool;

        //ObjectPool<MonsterProjectile> pool = new(() =>
        //{
        //    GameObject prefab = Resources.Load<GameObject>(name);
        //    GameObject go = Instantiate(prefab);
        //    go.SetActive(false);
        //    return go.GetComponent<MonsterProjectile>();
        //},
        //actionOnGet: (proj) => proj.gameObject.SetActive(true),
        //actionOnRelease: (proj) => proj.gameObject.SetActive(false),
        //defaultCapacity: cap);

        //MonsterProjectilePool[type] = pool;
    }
    
    public MonsterProjectile GetProjectiles(ProjectileType type, string id)
    {
        //if(MonsterProjectilePool.TryGetValue(type, out var objectPool))
        //{
        //    var projectile = objectPool.Get();
        //    projectile.AssignID(System.Guid.NewGuid().ToString());
        //    return projectile;
        //}
        //return null;

        if (!MonsterProjectilePool.TryGetValue(type, out var objectPool))
        {
            return null;
        }

        var projectile = objectPool.Get();
        projectile.AssignID(id);
        _projectileInstances[id] = projectile;

        return projectile;
    }

    public void ReturnProjectileByID(string id)
    {
        if (_projectileInstances.TryGetValue(id, out var projectile))
        {
            var type = projectile.ProjectileType;
            ReturnProjectile(type, projectile);
            _projectileInstances.Remove(id);
        }
    }

    public void ReturnProjectile(ProjectileType type, MonsterProjectile prefab)
    {
        prefab.HasHit = false;
        prefab.IsReturn = false;
        prefab.ResetProjectile();

        if (_projectileInstances.ContainsKey(prefab.ProjectileID))
        {
            _projectileInstances.Remove(prefab.ProjectileID);
        }

        if (MonsterProjectilePool.TryGetValue(type, out var objectPool))
        {
            if (objectPool != null)
            {
                objectPool.Release(prefab);
            }
        }
        else
        {
            prefab.gameObject.SetActive(false);
        }
    }
}
