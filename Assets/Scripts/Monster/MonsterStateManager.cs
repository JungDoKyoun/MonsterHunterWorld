using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateManager : MonoBehaviour
{
    private IMonsterState currentState;

    private void Awake()
    {
        ChangeMonsterState(new MonsterIdleState());
    }

    private void FixedUpdate()
    {
        currentState.Move();
    }

    private void Update()
    {
        currentState.Update();
    }

    public void ChangeMonsterState(IMonsterState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        //currentState.Enter(MonsterManager.Instance.MonsterController, this);
    }
}
