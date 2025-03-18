using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IMonsterState
{
    public abstract void Enter(MonsterController monster, MonsterStateManager stateManager, MonsterAnimationController anime);
    public abstract void Exit();
    public abstract void Move();
    public abstract void Update();
}

public class MonsterIdleState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;
    MonsterAnimationController _anime;
    float _tmepTime;
    Vector3 _targetPos;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager, MonsterAnimationController anime)
    {
        _monster = monster;
        _stateManager = stateManager;
        _anime = anime;
        _anime.PlayMonsterIdleAnime(true);
        _tmepTime = 0;
        _targetPos = _monster.GetNextPatrolPos();
        _monster.SetTargetPos(_targetPos);
    }

    public override void Exit()
    {
        _anime.PlayMonsterIdleAnime(false);
    }

    public override void Move()
    {
        
    }

    public override void Update()
    {
        _tmepTime += Time.deltaTime;

        _monster.NavMeshMatchMonsterPos();
        if (_monster.IsRestPos())
        {
            if(_tmepTime > 10)
            {
                if (_monster.IsNeedRo())
                {
                    _stateManager.ChangeMonsterState(new MonsterRotationState());
                    return;
                }
                else
                {
                    _stateManager.ChangeMonsterState(new MonsterPatrolState());
                    return;
                }
            }
        }
        else if (_monster.IsNeedRo())
        {
            _stateManager.ChangeMonsterState(new MonsterRotationState());
            return;
        }
        else
        {
            _stateManager.ChangeMonsterState(new MonsterPatrolState());
            return;
        }
    }
}

public class MonsterRotationState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;
    MonsterAnimationController _anime;
    Vector3 _targetPos;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager, MonsterAnimationController anime)
    {
        _monster = monster;
        _stateManager = stateManager;
        _anime = anime;
        _targetPos = _monster.GetNextPatrolPos();
        _monster.SetTargetPos(_targetPos);
        _monster.RotateToTarget();
    }

    public override void Exit()
    {
        _anime.PlayMonsterRotateAnime(false);
    }

    public override void Move()
    {
        
    }

    public override void Update()
    {
        if(_monster.IsRo == false)
        {
            _stateManager.ChangeMonsterState(new MonsterPatrolState());
            return;
        }
        _monster.NavMeshMatchMonsterPos();
    }
}

public class MonsterPatrolState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;
    MonsterAnimationController _anime;
    Vector3 _targetPos;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager, MonsterAnimationController anime)
    {
        _monster = monster;
        _stateManager = stateManager;
        _anime = anime;
        _targetPos = _monster.GetCurrentPatrolPos();
        _monster.SetTargetPos(_targetPos);
        _anime.PlayMonsterMoveAnime(true);
    }

    public override void Exit()
    {
        _anime.PlayMonsterMoveAnime(false);
    }

    public override void Move()
    {
        
    }

    public override void Update()
    {
        if (_monster.IsReachTarget())
        {
            if (_monster.IsRestPos())
            {
                _stateManager.ChangeMonsterState(new MonsterIdleState());
            }
            else if(_monster.IsNeedRo())
            {
                _stateManager.ChangeMonsterState(new MonsterRotationState());
                return;
            }
        }
        _monster.NavMeshMatchMonsterPos();
    }
}

public class MonsterRoarState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;
    MonsterAnimationController _anime;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager, MonsterAnimationController anime)
    {
        _monster = monster;
        _stateManager = stateManager;
        _anime = anime;
    }

    public override void Exit()
    {
        
    }

    public override void Move()
    {
        
    }

    public override void Update()
    {
        
    }
}

public class MonsterChaseState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;
    MonsterAnimationController _anime;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager, MonsterAnimationController anime)
    {
        _monster = monster;
        _stateManager = stateManager;
        _anime = anime;
    }

    public override void Exit()
    {
        
    }

    public override void Move()
    {
        
    }

    public override void Update()
    {
        
    }
}

public class MonsterAttackState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;
    MonsterAnimationController _anime;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager, MonsterAnimationController anime)
    {
        _monster = monster;
        _stateManager = stateManager;
        _anime = anime;
    }

    public override void Exit()
    {
        
    }

    public override void Move()
    {
        
    }

    public override void Update()
    {
        
    }
}

public class MonsterStunState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;
    MonsterAnimationController _anime;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager, MonsterAnimationController anime)
    {
        _monster = monster;
        _stateManager = stateManager;
        _anime = anime;
    }

    public override void Exit()
    {
        
    }

    public override void Move()
    {
        
    }

    public override void Update()
    {
        
    }
}
