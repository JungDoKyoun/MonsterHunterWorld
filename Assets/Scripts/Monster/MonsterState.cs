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
        _monster.CheckPlayer();
    }

    public override void Update()
    {
        _tmepTime += Time.deltaTime;

        if(_monster.IsBattle && !_monster.IsHit)
        {
            _monster.IsHit = true;
            _stateManager.ChangeMonsterState(new MonsterRoarState());
            return;
        }

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
    Vector3 _targetPlayer;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager, MonsterAnimationController anime)
    {
        _monster = monster;
        _stateManager = stateManager;
        _anime = anime;
        if(!_monster.IsBattle)
        {
            _targetPos = _monster.GetNextPatrolPos();
            _monster.SetTargetPos(_targetPos);
        }
        if(_monster.IsBattle)
        {
            _monster.CheckPlayer();
        }
        _monster.RotateToTarget();
        Debug.Log(_monster.IsHit);
        Debug.Log(_monster.IsBattle);
        Debug.Log(_monster.IsRo);
    }

    public override void Exit()
    {
        _anime.PlayMonsterRotateAnime(false);
    }

    public override void Move()
    {
        _monster.CheckPlayer();
    }

    public override void Update()
    {
        if (_monster.IsBattle && !_monster.IsHit)
        {
            _monster.IsHit = true;
            _stateManager.ChangeMonsterState(new MonsterRoarState());
            return;
        }

        if (_monster.IsRo == false)
        {
            Debug.Log("들어옴");
            if (_monster.IsBattle)
            {
                _stateManager.ChangeMonsterState(new MonsterChaseState());
                return;
            }
            else
            {
                _stateManager.ChangeMonsterState(new MonsterPatrolState());
                return;
            }
        }
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

        _monster.IsBattle = false;
        _targetPos = _monster.GetNextPatrolPos();
        _monster.SetTargetPos(_targetPos);
        _anime.PlayMonsterMoveAnime(true);
        Debug.Log("순찰 들어옴");
    }

    public override void Exit()
    {
        _anime.PlayMonsterMoveAnime(false);
    }

    public override void Move()
    {
        _monster.CheckPlayer();
        _monster.ApplyRootMotionMovement();
    }

    public override void Update()
    {
        if (_monster.IsBattle && !_monster.IsHit && _monster.IsCanFindPlayer())
        {
            _monster.IsHit = true;
            _stateManager.ChangeMonsterState(new MonsterRoarState());
            return;
        }

        if (_monster.IsReachTarget())
        {
            if (_monster.IsRestPos())
            {
                _monster.UpdatePatrolIndex();
                _stateManager.ChangeMonsterState(new MonsterIdleState());
                return;
            }
            else if(_monster.IsNeedRo())
            {
                _monster.UpdatePatrolIndex();
                _stateManager.ChangeMonsterState(new MonsterRotationState());
                return;
            }
        }

        if(_monster.IsNeedRo())
        {
            _stateManager.ChangeMonsterState(new MonsterRotationState());
            return;
        }
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
        _monster.IsRoar = true;
        _anime.PlayMonsterRoarAnime();
        _monster.StartCoroutine(_monster.WaitForEndRoarAnime());
        Debug.Log("포효 다시 들어옴");
    }

    public override void Exit()
    {
        
    }

    public override void Move()
    {
        _monster.CheckPlayer();
    }

    public override void Update()
    {
        if(!_monster.IsRoar)
        {
            if(_monster.IsNeedRo())
            {
                _stateManager.ChangeMonsterState(new MonsterRotationState());
                return;
            }
            else
            {
                _stateManager.ChangeMonsterState(new MonsterChaseState());
                return;
            }
        }
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
        _monster.CheckPlayer();
        _anime.PlayMonsterChaseAnime(true);
        Debug.Log("다시 추격");
    }

    public override void Exit()
    {
        _anime.PlayMonsterChaseAnime(false);
    }

    public override void Move()
    {
        _monster.CheckPlayer();
        _monster.ApplyRootMotionMovement();

    }

    public override void Update()
    {
        _monster.SmothRotateToPlayer();
        if (!_monster.IsCanFindPlayer())
        {
            _monster.ResetHit();
            _stateManager.ChangeMonsterState(new MonsterPatrolState());
            return;
        }

        if (_monster.IsCanAttackPlayer())
        {
            _stateManager.ChangeMonsterState(new MonsterAttackIdleState());
            return;
        }
    }
}

public class MonsterAttackIdleState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;
    MonsterAnimationController _anime;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager, MonsterAnimationController anime)
    {
        _monster = monster;
        _stateManager = stateManager;
        _anime = anime;
        Debug.Log("공격 대기상태 들어옴");
        _anime.PlayMonsterAttackIdleAnime(true);
    }

    public override void Exit()
    {
        Debug.Log("나감");
        _anime.PlayMonsterAttackIdleAnime(false);
    }

    public override void Move()
    {
        _monster.CheckPlayer();
        _monster.SmothRotateToPlayer();
    }

    public override void Update()
    {
        if (!_monster.IsCanFindPlayer())
        {
            _monster.ResetHit();
            _stateManager.ChangeMonsterState(new MonsterPatrolState());
            return;
        }

        if (!_monster.IsCanAttackPlayer())
        {
            _stateManager.ChangeMonsterState(new MonsterChaseState());
            return;
        }

        if(_monster.IsCoolTimeEnd())
        {
            _stateManager.ChangeMonsterState(new MonsterAttackState());
            return;
        }

        if (_monster.IsNeedRo())
        {
            _stateManager.ChangeMonsterState(new MonsterRotationState());
            return;
        }
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
        Debug.Log("공격 들어옴");
        _monster.IsAttack = true;
        _monster.Attack();
    }

    public override void Exit()
    {
        _monster.ResetCoolTime();
    }

    public override void Move()
    {
        
    }

    public override void Update()
    {
        _monster.CheckPlayer();
        if (!_monster.IsAttack)
        {
            if (!_monster.IsCanFindPlayer())
            {
                _monster.ResetHit();
                _stateManager.ChangeMonsterState(new MonsterPatrolState());
                return;
            }
            if (_monster.IsNeedRo())
            {
                _stateManager.ChangeMonsterState(new MonsterRotationState());
                return;
            }
            _stateManager.ChangeMonsterState(new MonsterAttackIdleState());
            return;
        }
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
