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

    public void LoadProjectilesFromAddressable(string label, List<MonsterProjectileData> projectileDatas)
    {
        Addressables.LoadAssetsAsync<GameObject>(label, prefab =>
        {
            var projectile = prefab.GetComponent<MonsterProjectile>();
            var type = projectile.ProjectileType;
            var match = projectileDatas.Find(x => x.ProjectileType == type);

            if(match != null)
            {
                projectile.SetData(match);
                if(!MonsterProjectilePool.ContainsKey(type))
                {
                    CreatePool(projectile, match.defaultCapacity, type);
                }
            }
        });
    }

    public void CreatePool(MonsterProjectile prefab, int cap, ProjectileType type)
    {
        Debug.Log("µé¾î¿È");
        var pool = new ObjectPool<MonsterProjectile>(
            createFunc: () => Instantiate(prefab),
            actionOnGet: obj => obj.gameObject.SetActive(true),
            actionOnRelease: obj => obj.gameObject.SetActive(false),
            actionOnDestroy: obj => Destroy(obj),
            defaultCapacity: cap);

        MonsterProjectilePool[type] = pool;
    }

    public MonsterProjectile GetProjectiles(ProjectileType type)
    {
        if(MonsterProjectilePool.TryGetValue(type, out var objectPool))
        {
            return objectPool.Get();
        }
        return null;
    }

    public void ReturnProjectile(ProjectileType type, MonsterProjectile prefab)
    {
        prefab.ResetProjectile();
        if(MonsterProjectilePool.TryGetValue(type, out var objectPool))
        {
            objectPool.Release(prefab);
        }
        else
        {
            Destroy(prefab);
        }
    }
}
