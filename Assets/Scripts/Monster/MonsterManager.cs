using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private static MonsterManager _instance;
    private MonsterStateManager _monsterStateManager;
    private MonsterController _monsterController;

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
}
