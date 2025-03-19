using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterStatusSO : ScriptableObject
{
    [Header("���� �⺻ ����")]
    public int ID;
    public string Name;
    public int MaxHP;
    public GameObject Prefab;
    //To-Do ������ Ŭ���� �߰�
    //public List<> DropItem;

    [Header("���� ���� ����")]
    public float Damage;
    public float Defense;
    public float AttackCoolTime;
    public float DetectRange;
    public float AttackRange;
}


[CreateAssetMenu(fileName = "BossMonster", menuName = "Monster/BossMonster")]
public class BossMonster : MonsterStatusSO
{

}
