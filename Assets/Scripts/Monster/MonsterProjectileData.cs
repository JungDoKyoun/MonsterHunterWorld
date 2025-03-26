using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterProjectileData
{
    public ProjectileType ProjectileType;
    public int defaultCapacity;
    public int damage;
    public int speed;
    public float lifeTime;
    public GameObject hitEffect;
}
