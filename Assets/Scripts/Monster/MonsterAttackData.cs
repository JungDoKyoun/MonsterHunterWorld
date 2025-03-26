using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterAttackData
{
    public string AttackName;
    public int Damage;
    public string AnimeName;
    public bool UsesProjectile;
    public ProjectileType ProjectileType;
    public string AttackColliderName;
}
