using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class MonsterStateManager : MonoBehaviour
{
    private IMonsterState currentState;
    private MonsterController monsterController;
    private MonsterAnimationController anime;

    private async void Start()
    {
        monsterController = MonsterManager.Instance.MonsterController;
        anime = MonsterManager.Instance.AnimationController;
        await UniTask.WaitUntil(() =>
        monsterController != null &&
        monsterController.MoveTargetPos != null &&
        monsterController.MoveTargetPos.Count > 0
        );
        ChangeMonsterState(new MonsterIdleState());
    }

    private void FixedUpdate()
    {
        if (currentState != null && monsterController != null)
        {
            currentState.Move();
            monsterController.NavMeshMatchMonsterPos();
            monsterController.NavMeshMatchMonsterRotation();
        }
    }

    private void Update()
    {
        if (currentState != null && monsterController != null)
        {
            currentState.Update();
            monsterController.UpdateAttackCoolTime();
        }
    }

    public void ChangeMonsterState(IMonsterState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(monsterController, this);
    }
}
