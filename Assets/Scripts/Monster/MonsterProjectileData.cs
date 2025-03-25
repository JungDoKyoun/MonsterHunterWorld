using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterProjectileData
{
    public ProjectileType ProjectileType;
    public int defaultCapacity;
    public uint damage;
    public uint speed;
    public float lifeTime;
    public GameObject hitEffect;
}
