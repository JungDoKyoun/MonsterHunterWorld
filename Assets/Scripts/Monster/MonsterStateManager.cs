using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateManager : MonoBehaviour
{
    private IMonsterState currentState;
    private MonsterController monsterController;
    private MonsterAnimationController anime;

    private void Start()
    {
        monsterController = MonsterManager.Instance.MonsterController;
        anime = MonsterManager.Instance.AnimationController;
        ChangeMonsterState(new MonsterIdleState());
    }

    private void FixedUpdate()
    {
        currentState.Move();
        monsterController.NavMeshMatchMonsterPos();
        monsterController.NavMeshMatchMonsterRotation();
    }

    private void Update()
    {
        currentState.Update();
        monsterController.UpdateAttackCoolTime();
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    monsterController.TakeDamage(100);
        //}
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            monsterController.TakeHeadDamage(120);
        }
    }

    public void ChangeMonsterState(IMonsterState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(monsterController, this, anime);
    }
}
