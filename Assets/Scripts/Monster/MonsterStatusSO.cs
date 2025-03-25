using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterStatusSO : ScriptableObject
{
    [Header("���� �⺻ ����")]
    public int ID;
    public string Name;
    public uint MaxHP;
    public uint RoSpeed;
    public GameObject Prefab;
    //To-Do ������ Ŭ���� �߰�
    //public List<> DropItem;

    [Header("���� ���� ����")]
    public uint BiteDamage;
    public uint TaileDamage;
    public uint ChargeDamage;
    public uint FlyDamage;
    public uint Defense;
    public uint HeadMaxDamage;
    public float AttackCoolTime;
    public float DetectRange;
    public float AttackRange;
    public float MinAttackRange;
    public float SturnTime;
    public List<MonsterAttackData> MonsterAttackDatas;
}


[CreateAssetMenu(fileName = "BossMonster", menuName = "Monster/BossMonster")]
public class BossMonster : MonsterStatusSO
{
    public List<MonsterProjectileData> ProjectileDatas;
    public string Label;
}
