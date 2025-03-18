using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private List<Vector3> moveTargetPos;
    [SerializeField] private float roSpeed;
    private NavMeshAgent agent;
    private MonsterAnimationController anime;
    private Vector3 targetPos;
    private int _currentPatrolIndex = 0;
    private int _nextPatrolIndex = 1;
    private bool _isRo;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anime = MonsterManager.Instance.AnimationController;
    }

    private void Start()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    //public List<Vector3> MoveTargetPos { get { return moveTargetPos; } set { moveTargetPos = value; } }
    public bool IsRo { get { return _isRo; } set { _isRo = value; } }

    public void SetTargetPos(Vector3 pos)
    {
        targetPos = pos;
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(pos);
        }
    }

    public Vector3 GetCurrentPatrolPos()
    {
        _currentPatrolIndex = (_currentPatrolIndex + 1) % moveTargetPos.Count;
        _nextPatrolIndex = (_nextPatrolIndex + 1) % moveTargetPos.Count;
        Vector3 patrolPos = moveTargetPos[_currentPatrolIndex];
        return patrolPos;
    }

    public Vector3 GetNextPatrolPos()
    {
        Vector3 patrolPos = moveTargetPos[_nextPatrolIndex];
        return patrolPos;
    }

    public void NavMeshMatchMonsterPos()
    {
        if(agent.enabled)
        {
            agent.nextPosition = transform.position;
        }
    }

    public void NavMeshMatchMonsterRotation()
    {
        if (agent.enabled)
        {
            agent.transform.rotation = transform.rotation;
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

    public bool IsNeedRo()
    {
        Vector3 dir = (targetPos - transform.position).normalized;
        float angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);
        IsRo = true;
        return Math.Abs(angle) > 10f;
    }

    public void RotateToTarget()
    {
        Vector3 dir = (targetPos - transform.position).normalized;
        float angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);
        Debug.Log(angle);
        if(Math.Abs(angle) < 30)
        {
            Debug.Log("들어옴ㅁㅁ");
            StartCoroutine(SmoothTurn(dir));
        }
        else
        {
            Debug.Log("애니메이션 들어옴");
            anime.PlayMonsterRotateAnime(true);
            anime.SetRoAngle(angle);
            StartCoroutine(WaitForEndAnime());
        }
    }

    private IEnumerator SmoothTurn(Vector3 direct)
    {
        Quaternion targetRo = Quaternion.LookRotation(direct);

        while(Quaternion.Angle(transform.rotation, targetRo) > 0.5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRo, roSpeed);
            yield return null;
        }

        transform.rotation = targetRo;
    }
    

    private IEnumerator WaitForEndAnime()
    {
        Debug.Log(anime.Anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //yield return new WaitUntil(() => anime.Anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        yield return new WaitForSeconds(3f);
        IsRo = false;
    }
}
