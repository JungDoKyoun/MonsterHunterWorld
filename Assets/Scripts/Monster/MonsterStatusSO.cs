using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterStatusSO : ScriptableObject
{
    [Header("몬스터 기본 정보")]
    public int ID;
    public string Name;
    public int MaxHP;
    public GameObject Prefab;
    //To-Do 아이템 클래스 추가
    //public List<> DropItem;

    [Header("몬스터 전투 관련")]
    public float BiteDamage;
    public float TaileDamage;
    public float ChargeDamage;
    public float FlyDamage;
    public float Defense;
    public float AttackCoolTime;
    public float DetectRange;
    public float AttackRange;
    public float MinAttackRange;
}


[CreateAssetMenu(fileName = "BossMonster", menuName = "Monster/BossMonster")]
public class BossMonster : MonsterStatusSO
{

}
