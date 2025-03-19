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

        _monster.NavMeshMatchMonsterPos();
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
        Debug.Log("ȸ�� ����");
        if(!_monster.IsBattle)
        {
            _targetPos = _monster.GetNextPatrolPos();
            _monster.SetTargetPos(_targetPos);
        }
        if(_monster.IsBattle)
        {
            _monster.DetectedPlayer();
            _targetPlayer = _monster.GetClosePlayer();
            Debug.Log(_targetPlayer);
            _monster.SetTargetplayer(_targetPlayer);
        }
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
        if (_monster.IsBattle && !_monster.IsHit)
        {
            _monster.IsHit = true;
            _stateManager.ChangeMonsterState(new MonsterRoarState());
            return;
        }

        if (_monster.IsRo == false)
        {
            if (_monster.IsBattle)
            {
                Debug.Log("�߰��Ϸ��� ����");
                _stateManager.ChangeMonsterState(new MonsterChaseState());
                return;
            }
            else
            {
                Debug.Log("�̵��Ϸ������ ����");
                _stateManager.ChangeMonsterState(new MonsterPatrolState());
                return;
            }
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
        if(!_monster.IsBattle)
        {
            _targetPos = _monster.GetCurrentPatrolPos();
        }
        _monster.IsBattle = false;
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
        if (_monster.IsBattle && !_monster.IsHit)
        {
            _monster.IsHit = true;
            _stateManager.ChangeMonsterState(new MonsterRoarState());
            return;
        }

        if (_monster.IsReachTarget())
        {
            if (_monster.IsRestPos())
            {
                _stateManager.ChangeMonsterState(new MonsterIdleState());
                return;
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
    Vector3 _targetPlayer;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager, MonsterAnimationController anime)
    {
        _monster = monster;
        _stateManager = stateManager;
        _anime = anime;
        _monster.DetectedPlayer();
        _targetPlayer = _monster.GetClosePlayer();
        _monster.SetTargetplayer(_targetPlayer);
        _anime.PlayMonsterRoarAnime();
        _monster.StartCoroutine(_monster.WaitForEndRoarAnime());
    }

    public override void Exit()
    {
        
    }

    public override void Move()
    {
        
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
        _monster.NavMeshMatchMonsterPos();
    }
}

public class MonsterChaseState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;
    MonsterAnimationController _anime;
    Vector3 _targetPlayer;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager, MonsterAnimationController anime)
    {
        _monster = monster;
        _stateManager = stateManager;
        _anime = anime;
        _monster.DetectedPlayer();
        _targetPlayer = _monster.GetClosePlayer();
        Debug.Log(_targetPlayer + "�÷��̾� ��ġ");
        _monster.SetTargetplayer(_targetPlayer);
        _anime.PlayMonsterChaseAnime(true);
    }

    public override void Exit()
    {
        _anime.PlayMonsterChaseAnime(false);
    }

    public override void Move()
    {
        
    }

    public override void Update()
    {
        _monster.DetectedPlayer();
        _targetPlayer = _monster.GetClosePlayer();
        _monster.SetTargetplayer(_targetPlayer);
        Debug.Log(_targetPlayer + "�÷��̾� ��ġ");
        _monster.NavMeshMatchMonsterPos();
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
