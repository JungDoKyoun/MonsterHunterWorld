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
    public int SleepIndex;
    public List<int> RestIndex;
    public List<GameObject> MoveTargetPos;
    public int RestTime;
    public int SleepTime;
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