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

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    {
        _monster = monster;
        _stateManager = stateManager;
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

public class MonsterPatrolState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    {
        _monster = monster;
        _stateManager = stateManager;
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

public class MonsterRoarState : IMonsterState
{
    MonsterController _monster;
    MonsterStateManager _stateManager;

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    {
        _monster = monster;
        _stateManager = stateManager;
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

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    {
        _monster = monster;
        _stateManager = stateManager;
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

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    {
        _monster = monster;
        _stateManager = stateManager;
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

    public override void Enter(MonsterController monster, MonsterStateManager stateManager)
    {
        _monster = monster;
        _stateManager = stateManager;
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
