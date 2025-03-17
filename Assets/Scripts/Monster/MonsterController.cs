using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private List<Vector3> moveTargetPos;
    private NavMeshAgent agent;
    private MonsterAnimationController anime;
    private Vector3 targetPos;
    public int _currentPatrolIndex = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    //public List<Vector3> MoveTargetPos { get { return moveTargetPos; } set { moveTargetPos = value; } }

    public void SetTargetPos(Vector3 pos)
    {
        targetPos = pos;
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(pos);
        }
    }

    public Vector3 GetNextPatrolPos()
    {
        _currentPatrolIndex = (_currentPatrolIndex + 1) % moveTargetPos.Count;
        Vector3 patrolPos = moveTargetPos[_currentPatrolIndex];
        return patrolPos;
    }

    public void NavMeshMatchMonsterPos()
    {
        if(agent.enabled)
        {
            agent.nextPosition = transform.position;
        }
    }

    public bool IsRestPos()
    {
        return _currentPatrolIndex == 0 || _currentPatrolIndex == 2;
    }

    public bool IsReachTarget()
    {
        return !agent.pathPending && agent.remainingDistance < 0.1f;
    }
}
