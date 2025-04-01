using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterStatusSO : ScriptableObject
{
    [Header("���� �⺻ ����")]
    public int ID;
    public string Name;
    public int MaxHP;
    public int RoSpeed;
    public GameObject Prefab;
    //To-Do ������ Ŭ���� �߰�
    //public List<> DropItem;

    [Header("���� ���� ����")]
    public int Defense;
    public int HeadMaxDamage;
    public float AttackCoolTime;
    public float DetectRange;
    public float AttackRange;
    public float MinAttackRange;
    public float SturnTime;
    public List<MonsterAttackData> MonsterAttackDatas;
}