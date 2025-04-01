using Photon.Pun.Demo.Procedural;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IMonsterState
{
    public abstract void Enter(MonsterController monster, MonsterStateManager stateManager);
    public abstract void Exit();
    public abstract void Move();
    public abstract void Update();
}

public class MonsterIdleState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;
    float _tempTime;
    Vector3 _targetPos;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    {
        _monster = monster;
        _stateManager = stateManager;
        _monster.SetAnime("IsIdle", true);
        _tempTime = 0;
        _targetPos = _monster.GetNextPatrolPos();
        _monster.SetTargetPos(_targetPos);
        Debug.Log("������");
    }

    public override void Exit()
    {
        _monster.SetAnime("IsIdle", false);
    }

    public override void Move()
    {
        _monster.CheckPlayer();
    }

    public override void Update()
    {
        _tempTime += Time.deltaTime;

        if(_monster.IsDie)
        {
            _stateManager.ChangeMonsterState(new MonsterDieState());
            return;
        }

        if(_monster.IsTrap)
        {
            _stateManager.ChangeMonsterState(new MonsterTrapState());
            return;
        }

        if(_monster.IsStun)
        {
            _stateManager.ChangeMonsterState(new MonsterStunState());
            return;
        }

        if(_monster.IsBattle && !_monster.IsHit)
        {
            _stateManager.ChangeMonsterState(new MonsterRoarState());
            return;
        }

        if(_monster.IsBattle && _monster.IsHit)
        {
            _stateManager.ChangeMonsterState(new MonsterChaseState());
            return;
        }

        if (_monster.IsSleepPos() && !_monster.IsalSleep)
        {
            _stateManager.ChangeMonsterState(new MonsterSleepState());
            return;
        }

        if (_monster.IsRestPos())
        {
            if(_tempTime >= _monster.RestTime)
            {
                if (_monster.IsNeedRo())
                {
                    _stateManager.ChangeMonsterState(new MonsterRotationState());
                    return;
                }
                else
                {
                    _stateManager.ChangeMonsterState(new MonsterTakeOffState());
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
            _stateManager.ChangeMonsterState(new MonsterTakeOffState());
            return;
        }
    }
}

public class MonsterSleepState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    {
        _monster = monster;
        _stateManager = stateManager;
        _monster.IsSleep = true;
        _monster.IsalSleep = true;
        _monster.SetAnime("IsSleep");
        _monster.Sleep();
        Debug.Log("�ڴ���");
    }

    public override void Exit()
    {
        if(_monster.IsSleep)
        {
            _monster.IsSleep = false;
        }
    }

    public override void Move()
    {
        
    }

    public override void Update()
    {
        if (_monster.IsDie)
        {
            _stateManager.ChangeMonsterState(new MonsterDieState());
            return;
        }

        if (_monster.IsTrap)
        {
            _stateManager.ChangeMonsterState(new MonsterTrapState());
            return;
        }

        if (_monster.IsStun)
        {
            _stateManager.ChangeMonsterState(new MonsterStunState());
            return;
        }

        if (_monster.IsBattle && !_monster.IsHit)
        {
            _monster.IsNeedRoar = true;
            _stateManager.ChangeMonsterState(new MonsterWakeUpState());
            return;
        }

        if (!_monster.IsSleep)
        {
            _stateManager.ChangeMonsterState(new MonsterWakeUpState());
            return;
        }
    }
}

public class MonsterWakeUpState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    {
        _monster = monster;
        _stateManager = stateManager;
        _monster.IsWakeUp = true;
        _monster.SetAnime("IsSleep3");
        _monster.WakeUP();
    }

    public override void Exit()
    {
        
    }

    public override void Move()
    {
        
    }

    public override void Update()
    {
        if (_monster.IsDie)
        {
            _stateManager.ChangeMonsterState(new MonsterDieState());
            return;
        }

        if (_monster.IsTrap)
        {
            _stateManager.ChangeMonsterState(new MonsterTrapState());
            return;
        }

        if (_monster.IsStun)
        {
            _stateManager.ChangeMonsterState(new MonsterStunState());
            return;
        }

        if(!_monster.IsWakeUp)
        {
            if (_monster.IsBattle && _monster.IsNeedRoar)
            {
                _monster.IsNeedRoar = false;
                _stateManager.ChangeMonsterState(new MonsterRoarState());
                return;
            }

            else
            {
                _stateManager.ChangeMonsterState(new MonsterIdleState());
                return;
            }
        }
    }
}

public class MonsterRotationState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;
    Vector3 _targetPos;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    {
        _monster = monster;
        _stateManager = stateManager;
        _monster.IsRo = true;
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
        Debug.Log("ȸ�� ����");
    }

    public override void Exit()
    {
        if(!_monster.IsFly)
        {
            _monster.SetAnime("IsRo", false);
        }
        else if (_monster.IsFly)
        {
            _monster.SetAnime("IsFlyRo", false);
        }
    }

    public override void Move()
    {
        _monster.CheckPlayer();
    }

    public override void Update()
    {
        if (_monster.IsDie)
        {
            _stateManager.ChangeMonsterState(new MonsterDieState());
            return;
        }

        if (_monster.IsTrap)
        {
            _stateManager.ChangeMonsterState(new MonsterTrapState());
            return;
        }

        if (_monster.IsStun)
        {
            _stateManager.ChangeMonsterState(new MonsterStunState());
            return;
        }

        if (_monster.IsBattle && !_monster.IsHit)
        {
            _stateManager.ChangeMonsterState(new MonsterRoarState());
            return;
        }

        if (_monster.IsRo == false)
        {
            if (_monster.IsBattle)
            {
                _stateManager.ChangeMonsterState(new MonsterChaseState());
                return;
            }
            else
            {
                if(_monster.IsFly)
                {
                    _stateManager.ChangeMonsterState(new MonsterPatrolState());
                    return;
                }
                else if(!_monster.IsFly)
                {
                    _stateManager.ChangeMonsterState(new MonsterTakeOffState());
                    return;
                }
            }
        }
    }
}

public class MonsterTakeOffState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    {
        _monster = monster;
        _stateManager = stateManager;
        _monster.IsTakeOff = true;
        _monster.TakeOff();
    }

    public override void Exit()
    {
        
    }

    public override void Move()
    {
        //_monster.ApplyRootMotionMovement();
    }

    public override void Update()
    {
        if (_monster.IsDie)
        {
            _stateManager.ChangeMonsterState(new MonsterDieState());
            return;
        }

        if(!_monster.IsFly)
        {
            if (_monster.IsTrap)
            {
                _stateManager.ChangeMonsterState(new MonsterTrapState());
                return;
            }

            if (_monster.IsStun)
            {
                _stateManager.ChangeMonsterState(new MonsterStunState());
                return;
            }

            if (_monster.IsBattle && !_monster.IsHit && _monster.IsCanFindPlayer())
            {
                _stateManager.ChangeMonsterState(new MonsterRoarState());
                return;
            }
        }

        if(_monster.IsFly && !_monster.IsTakeOff)
        {
            _stateManager.ChangeMonsterState(new MonsterPatrolState());
            return;
        }
    }
}

public class MonsterPatrolState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;
    Vector3 _targetPos;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    {
        _monster = monster;
        _stateManager = stateManager;

        _monster.IsBattle = false;
        _targetPos = _monster.GetNextPatrolPos();
        _monster.SetTargetPos(_targetPos);
        _monster.Patrol();
        Debug.Log("���� ����");
    }

    public override void Exit()
    {
        _monster.SetAnime("Patrol2", false);
    }

    public override void Move()
    {
        _monster.CheckPlayer();
        //_monster.ApplyRootMotionMovement();
    }

    public override void Update()
    {
        if (_monster.IsDie)
        {
            _stateManager.ChangeMonsterState(new MonsterDieState());
            return;
        }

        if (_monster.IsReachTarget())
        {
            _monster.UpdatePatrolIndex();
            if (_monster.IsRestPos())
            {
                _stateManager.ChangeMonsterState(new MonsterLandingState());
                return;
            }
            else if(_monster.IsNeedRo())
            {
                _stateManager.ChangeMonsterState(new MonsterRotationState());
                return;
            }
        }
    }
}

public class MonsterLandingState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    {
        _monster = monster;
        _stateManager = stateManager;
        _monster.IsLanding = true;
        _monster.Landing();
    }

    public override void Exit()
    {
        _monster.IsFly = false;
    }

    public override void Move()
    {
        
    }

    public override void Update()
    {
        if (_monster.IsDie)
        {
            _stateManager.ChangeMonsterState(new MonsterDieState());
            return;
        }

        if(!_monster.IsLanding)
        {
            _stateManager.ChangeMonsterState(new MonsterIdleState());
            return;
        }
    }
}

public class MonsterRoarState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    {
        _monster = monster;
        _stateManager = stateManager;
        _monster.IsHit = true;
        _monster.SetAnime("Roar");
        _monster.RequestRoar();
        Debug.Log("��ȿ �ٽ� ����");
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
        if (_monster.IsDie)
        {
            _stateManager.ChangeMonsterState(new MonsterDieState());
            return;
        }

        if (_monster.IsTrap)
        {
            _stateManager.ChangeMonsterState(new MonsterTrapState());
            return;
        }

        if (_monster.IsStun)
        {
            _stateManager.ChangeMonsterState(new MonsterStunState());
            return;
        }

        if (!_monster.IsRoar)
        {
            if (!_monster.IsCanFindPlayer())
            {
                _monster.ResetHit();
                _stateManager.ChangeMonsterState(new MonsterIdleState());
                return;
            }

            if (_monster.IsNeedRo())
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

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    { 
        _monster = monster;
        _stateManager = stateManager;
        _monster.CheckPlayer();
        _monster.TurnOnAgent();
        _monster.SetAnime("IsChase", true);
        Debug.Log("�ٽ� �߰�");
    }

    public override void Exit()
    {
        _monster.TurnOffAgent();
        _monster.SetAnime("IsChase", false);
    }

    public override void Move()
    {
        _monster.CheckPlayer();

    }

    public override void Update()
    {
        _monster.Link();
        if (_monster.IsDie)
        {
            _stateManager.ChangeMonsterState(new MonsterDieState());
            return;
        }

        if (_monster.IsTrap)
        {
            _stateManager.ChangeMonsterState(new MonsterTrapState());
            return;
        }

        if (_monster.IsStun)
        {
            _stateManager.ChangeMonsterState(new MonsterStunState());
            return;
        }

        if (!_monster.IsCanFindPlayer())
        {
            _monster.ResetHit();
            _stateManager.ChangeMonsterState(new MonsterIdleState());
            return;
        }

        if (_monster.IsCanAttackPlayer() && !_monster.IsTooClose())
        {
            _stateManager.ChangeMonsterState(new MonsterAttackIdleState());
            return;
        }

        if(_monster.IsTooClose())
        {
            _stateManager.ChangeMonsterState(new MonsterBackMoveState());
            return;
        }
    }
}

public class MonsterAttackIdleState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    {
        _monster = monster;
        _stateManager = stateManager;
        Debug.Log("���� ������ ����");
        _monster.SetAnime("IsAttackIdle", true);
    }

    public override void Exit()
    {
        _monster.SetAnime("IsAttackIdle", false);
    }

    public override void Move()
    {
        _monster.CheckPlayer();
        _monster.SmothRotateToPlayer();
    }

    public override void Update()
    {
        if (_monster.IsDie)
        {
            _stateManager.ChangeMonsterState(new MonsterDieState());
            return;
        }

        if (_monster.IsTrap)
        {
            _stateManager.ChangeMonsterState(new MonsterTrapState());
            return;
        }

        if (_monster.IsStun)
        {
            _stateManager.ChangeMonsterState(new MonsterStunState());
            return;
        }

        if (!_monster.IsCanFindPlayer())
        {
            _monster.ResetHit();
            _stateManager.ChangeMonsterState(new MonsterIdleState());
            return;
        }

        if (!_monster.IsCanAttackPlayer())
        {
            _stateManager.ChangeMonsterState(new MonsterChaseState());
            return;
        }

        if (_monster.IsTooClose())
        {
            _stateManager.ChangeMonsterState(new MonsterBackMoveState());
            return;
        }

        if (_monster.IsCoolTimeEnd())
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

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    {
        _monster = monster;
        _stateManager = stateManager;
        Debug.Log("���� ����");
        _monster.IsAttack = true;
        _monster.ChooseAttackType();
    }

    public override void Exit()
    {
        _monster.ResetCoolTime();
        //_monster.ApplyRootMotionMovement();
    }

    public override void Move()
    {
        _monster.CheckPlayer();
        _monster.IsBlock();
        _monster.BlockMove();
    }

    public override void Update()
    {
        if (_monster.IsDie)
        {
            _stateManager.ChangeMonsterState(new MonsterDieState());
            return;
        }

        if (_monster.IsTrap)
        {
            _stateManager.ChangeMonsterState(new MonsterTrapState());
            return;
        }

        if (_monster.IsStun)
        {
            _stateManager.ChangeMonsterState(new MonsterStunState());
            return;
        }

        if (!_monster.IsAttack)
        {
            if (!_monster.IsCanFindPlayer())
            {
                _monster.ResetHit();
                _stateManager.ChangeMonsterState(new MonsterIdleState());
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

public class MonsterBackMoveState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    {
        _monster = monster;
        _stateManager = stateManager;
        Debug.Log("�ʹ� ������� �ڷΰ�");
        _monster.SetAnime("BackMove");
        _monster.RequestBackMove();
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
        if (_monster.IsDie)
        {
            _stateManager.ChangeMonsterState(new MonsterDieState());
            return;
        }

        if (_monster.IsTrap)
        {
            _stateManager.ChangeMonsterState(new MonsterTrapState());
            return;
        }

        if (_monster.IsStun)
        {
            _stateManager.ChangeMonsterState(new MonsterStunState());
            return;
        }

        if (!_monster.IsBackMove)
        {
            if (_monster.IsTooClose())
            {
                _stateManager.ChangeMonsterState(new MonsterAttackIdleState());
                return;
            }
            if (!_monster.IsCanFindPlayer())
            {
                _monster.ResetHit();
                _stateManager.ChangeMonsterState(new MonsterIdleState());
                return;
            }

            if (!_monster.IsCanAttackPlayer())
            {
                _stateManager.ChangeMonsterState(new MonsterChaseState());
                return;
            }

            if (!_monster.IsTooClose() && _monster.IsCanAttackPlayer())
            {
                _stateManager.ChangeMonsterState(new MonsterAttackIdleState());
                return;
            }

            if (_monster.IsNeedRo())
            {
                _stateManager.ChangeMonsterState(new MonsterRotationState());
                return;
            }
        }
    }
}

public class MonsterStunState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    { 
        _monster = monster;
        _stateManager = stateManager;
        _monster.SetAnime("IsSturn", true);
    }

    public override void Exit()
    {
        _monster.SetAnime("IsSturn", false);
        _monster.CheckPlayer();
    }

    public override void Move()
    {
        
    }

    public override void Update()
    {
        if (_monster.IsDie)
        {
            _stateManager.ChangeMonsterState(new MonsterDieState());
            return;
        }

        if (_monster.IsTrap)
        {
            _stateManager.ChangeMonsterState(new MonsterTrapState());
            return;
        }

        if (!_monster.IsStun)
        {
            if (!_monster.IsCanFindPlayer())
            {
                _monster.ResetHit();
                _stateManager.ChangeMonsterState(new MonsterIdleState());
                return;
            }
            if (_monster.IsNeedRo())
            {
                _stateManager.ChangeMonsterState(new MonsterRotationState());
                return;
            }
            else
            {
                _stateManager.ChangeMonsterState(new MonsterIdleState());
                return;
            }
        }
    }
}

public class MonsterTrapState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    {
        _monster = monster;
        _stateManager = stateManager;
        _monster.RequestTrap();
        _monster.SetAnime("IsTrap", true);
    }

    public override void Exit()
    {
        _monster.SetAnime("IsTrap", false);
    }

    public override void Move()
    {
        
    }

    public override void Update()
    {
        if (_monster.IsDie)
        {
            _stateManager.ChangeMonsterState(new MonsterDieState());
            return;
        }

        if(!_monster.IsTrap)
        {
            if (_monster.IsStun)
            {
                _stateManager.ChangeMonsterState(new MonsterStunState());
                return;
            }

            if (!_monster.IsCanFindPlayer())
            {
                _monster.ResetHit();
                _stateManager.ChangeMonsterState(new MonsterIdleState());
                return;
            }

            if (_monster.IsNeedRo())
            {
                _stateManager.ChangeMonsterState(new MonsterRotationState());
                return;
            }
            else
            {
                _stateManager.ChangeMonsterState(new MonsterIdleState());
                return;
            }
        }
    }
}

public class MonsterDieState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    {
        _monster = monster;
        _stateManager = stateManager;
        _monster.SetAnime("IsDie", true);
    }

    public override void Exit()
    {
        _monster.SetAnime("IsDie", false);
    }

    public override void Move()
    {
        
    }

    public override void Update()
    {
        if(!_monster.IsDie)
        {
            _stateManager.ChangeMonsterState(new MonsterIdleState());
            return;
        }
    }
}
