using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterProjectile : MonoBehaviour
{
    [SerializeField] private ProjectileType projectileType;
    private MonsterProjectileData _data;
    private Vector3 _direction;

    public ProjectileType ProjectileType { get { return projectileType; } set { projectileType = value; } }
    private void Update()
    {
        Shoot();
    }

    public void SetData(MonsterProjectileData newData)
    {
        _data = newData;
    }

    public void Shoot()
    {
        uint Projectilespeed = _data.speed;
        transform.Translate(Vector3.forward * Projectilespeed * Time.deltaTime, Space.Self);
    }
}
