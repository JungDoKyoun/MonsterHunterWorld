using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private static MonsterManager _instance;
    private MonsterStateManager _monsterStateManager;
    private MonsterController _monsterController;
    private MonsterAnimationController _animationController;
    [SerializeField] private BossMonster _monsterSO;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        MonsterStateManager = GetComponent<MonsterStateManager>();
        MonsterController = GetComponent<MonsterController>();
        AnimationController = GetComponent<MonsterAnimationController>();
    }

    public static MonsterManager Instance
    {
        get
        {
            if(_instance == null)
            {
                return null;
            }
            return _instance;
        }
    }

    public MonsterStateManager MonsterStateManager { get { return _monsterStateManager; } private set { _monsterStateManager = value; } }
    public MonsterController MonsterController { get { return _monsterController; } private set { _monsterController = value; } }
    public MonsterAnimationController AnimationController { get { return _animationController; } private set { _animationController = value; } }
    public BossMonster MonsterSO { get { return _monsterSO; } private set { _monsterSO = value; } }
}
