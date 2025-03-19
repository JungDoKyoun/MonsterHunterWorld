using GLTF.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private List<Vector3> moveTargetPos; //패트롤 돌아야하는 위치 좌표
    [SerializeField] private float roSpeed; //부드럽게 돌때 사용하는 변수
    private NavMeshAgent agent; //네이메쉬
    private MonsterAnimationController anime; //애니메이션
    private List<Transform> _detectPlayers = new List<Transform>(); //감지되는 플레이어 목록
    private Vector3 _targetPlayerPos; //타깃이된 플레이어 위치 정보
    private Vector3 _targetPos; //타깃이 되는 위치 정보
    private int _currentPatrolIndex = 0; //현재 있는 좌표 인덱스 정보
    private int _nextPatrolIndex = 1; //현재 가고있는 인덱스 좌표
    private int _maxHP; //최대 HP
    private float _currentHP; //현재 몬스터의 HP
    private float _detectRange; //플레이어 감지 거리
    private float _attackRange; //몬스터 공격 거리
    private float _attackCoolTime; //몬스터 공격 쿨타임
    private float _elapsedTime;
    private bool _isRo; //회전중인지?
    private bool _isDie; //죽었는지?
    private bool _isHit; //맞았는지?
    [SerializeField] private bool _isBattle; //전투중인지?
    private bool _isAttack; //공격중인지?
    private bool _isRoar;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anime = MonsterManager.Instance.AnimationController;
    }

    private void Start()
    {
        _maxHP = MonsterManager.Instance.MonsterSO.MaxHP;
        _currentHP = _maxHP;
        _detectRange = MonsterManager.Instance.MonsterSO.DetectRange;
        _attackRange = MonsterManager.Instance.MonsterSO.AttackRange;
        _attackCoolTime = MonsterManager.Instance.MonsterSO.AttackCoolTime;
        _elapsedTime = _attackCoolTime;
        _isBattle = false;
        _isDie = false;
        _isRo = false;
        _isRoar = false;
        _isHit = false;
        agent.updatePosition = false;
        agent.updateRotation = false;
        Debug.Log(agent.transform.rotation);
    }

    //public List<Vector3> MoveTargetPos { get { return moveTargetPos; } set { moveTargetPos = value; } }
    public bool IsRo { get { return _isRo; } set { _isRo = value; } }
    public bool IsDie { get { return _isDie; } set { _isDie = value; } }
    public bool IsBattle { get { return _isBattle; } set { _isBattle = value; } }
    public bool IsRoar { get { return _isRoar; } set { _isRoar = value; } }
    public bool IsHit { get { return _isHit; } set { _isHit = value; } }

    public void SetTargetPos(Vector3 pos) //타깃 위치 정보 네비메쉬 등록
    {
        _targetPos = pos;
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(pos);
        }
    }

    public Vector3 GetCurrentPatrolPos() //패트롤시 타깃이 되는 위치 정보 변경
    {
        _currentPatrolIndex = (_currentPatrolIndex + 1) % moveTargetPos.Count;
        _nextPatrolIndex = (_nextPatrolIndex + 1) % moveTargetPos.Count;
        Vector3 patrolPos = moveTargetPos[_currentPatrolIndex];
        return patrolPos;
    }

    public Vector3 GetNextPatrolPos() //회전 혹은 대기 상태일때 가야할 좌표 변경
    {
        Vector3 patrolPos = moveTargetPos[_nextPatrolIndex];
        return patrolPos;
    }

    public void NavMeshMatchMonsterPos() //네비메쉬 좌표 몬스터를 따라가게함
    {
        if(agent.enabled)
        {
            agent.nextPosition = transform.position;
        }
    }

    public void NavMeshMatchMonsterRotation() //위와마찬가지로 회전
    {
        if (agent.enabled)
        {
            agent.transform.rotation = transform.rotation;
        }
    }

    public bool IsRestPos() //쉬는 포인트 지정한 숫자 바꾸면 됨
    {
        return _currentPatrolIndex == 0 || _currentPatrolIndex == 2;
    }

    public bool IsReachTarget() //패트롤 목표로한 좌표에 도달 했는가?
    {
        return !agent.pathPending && agent.remainingDistance < 5.0f;
    }

    public bool IsNeedRo() //타깃과의 각도 계산 후 회전이 필요한지?
    {
        Vector3 dir;
        if (IsBattle)
        {
            Debug.Log("전투중 각도계산");
            dir = (_targetPlayerPos - transform.position).normalized;
        }
        else
        {
            Debug.Log("전투아닐때 각도계산");
            dir = (_targetPos - transform.position).normalized;
        }

        float angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);
        IsRo = true;
        Debug.Log(angle);
        return Math.Abs(angle) > 10f;
    }

    public void RotateToTarget() //타깃을 향해 회전
    {
        Vector3 dir;
        if (IsBattle)
        {
            dir = (_targetPlayerPos - transform.position).normalized;
        }
        else
        {
            dir = (_targetPos - transform.position).normalized;
        }

        float angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);
        agent.enabled = false;

        if (Math.Abs(angle) < 30)
        {
            StartCoroutine(SmoothTurn(dir));
        }
        else
        {
            anime.PlayMonsterRotateAnime(true);
            anime.SetRoAngle(angle);
            StartCoroutine(WaitForEndRotateAnime());
        }
    }

    public void SmothRotateToPlayer() //추적할때 자연스럽게 플레이어 바라보며 갈 수 있도록 회전
    {
        Vector3 dir = (_targetPlayerPos - transform.position).normalized;

        Quaternion targetRo = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRo, roSpeed);
    }

    private IEnumerator SmoothTurn(Vector3 direct) //30도 이하 자연스럽게 애니메이션 없이 회전
    {
        Quaternion targetRo = Quaternion.LookRotation(direct);

        while(Quaternion.Angle(transform.rotation, targetRo) > 0.5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRo, roSpeed);
            yield return null;
        }

        transform.rotation = targetRo;
    }
    

    private IEnumerator WaitForEndRotateAnime() //회전 애니메이션이 끝날때까지 대기
    {
        yield return new WaitForSeconds(0.3f);

        yield return new WaitUntil(() => anime.Anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        Vector3 dir;
        if (IsBattle)
        {
            dir = (_targetPlayerPos - transform.position).normalized;
        }
        else
        {
            dir = (_targetPos - transform.position).normalized;
        }

        float angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);

        if (Math.Abs(angle) < 30)
        {
            StartCoroutine(SmoothTurn(dir));
        }
        agent.enabled = true;
        IsRo = false;
    }

    public void OnHit(Transform player) //선공 당했을때 (아직 안쓰는중)
    {
        _isBattle = true;
        _targetPlayerPos = player.position;
    }

    public Vector3 GetFirstAttckPlayer() //선공 당했을때 (아직 안쓰는중)
    {
        return _targetPlayerPos;
    }

    public void CheckPlayer() //플레이어 감지 및 좌표 설정
    {
        DetectedPlayer();
        if(_detectPlayers.Count > 0)
        {
            _targetPlayerPos = GetClosePlayer();
            SetTargetplayer();
        }
    }

    public bool IsCanAttackPlayer() //공격 가능?
    {
        float dis = Vector3.Distance(transform.position, _targetPlayerPos);

        if(dis <= _attackRange)
        {
            return true;
        }
        return false;
    }

    public void UpdateAttackCoolTime() //쿨타임 재기
    {
        _elapsedTime -= Time.deltaTime;
        if(_elapsedTime <= 0)
        {
            _elapsedTime = 0;
        }
    }

    public bool IsCoolTimeEnd()
    {
        return _elapsedTime <= 0;
    }

    public void ResetCoolTime()
    {
        _elapsedTime = _attackCoolTime;
    }

    public void DetectedPlayer() //플레이어 감지
    {
        _detectPlayers.Clear();

        Collider[] players = Physics.OverlapSphere(transform.position, _detectRange);

        foreach(Collider collider in players)
        {
            if(collider.CompareTag("Player"))
            {
                _detectPlayers.Add(collider.transform);
                _isBattle = true;
            }
        }
    }

    public Vector3 GetClosePlayer() //가장 가까이 있는 플레이어 찾기
    {
        Transform closePlayer = null;
        float minDis = Mathf.Infinity;

        foreach(var player in _detectPlayers)
        {
            float dis = Vector3.Distance(transform.position, player.position);

            if(dis < minDis)
            {
                minDis = dis;
                closePlayer = player;
            }
        }

        return closePlayer.position;
    }

    public void SetTargetplayer()
    {
        if(agent.isOnNavMesh)
        {
            agent.SetDestination(_targetPlayerPos);
        }
    }

    public IEnumerator WaitForEndRoarAnime()
    {
        IsRoar = true;
        Debug.Log(anime.Anime.GetCurrentAnimatorStateInfo(0).normalizedTime);
        yield return new WaitUntil(() => anime.Anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        IsRoar = false;
    }

    public void TakeDamage(float damage) //데미지 받기
    {
        _currentHP -= damage;

        if(_currentHP <= 0)
        {
            _currentHP = 0;
            Die();
        }
    }

    public void Die() //죽었을때
    {
        _isDie = true;
    }

    public void ApplyRootMotionMovement() //루트모션 좌표 강제 이동
    {
        Vector3 rootMotionMove = anime.Anime.deltaPosition;
        Vector3 navMeshMove = agent.desiredVelocity.normalized * rootMotionMove.magnitude;

        Vector3 finalMove = Vector3.Lerp(rootMotionMove, navMeshMove, 0.5f);

        transform.position += finalMove;
        transform.rotation = anime.Anime.rootRotation;
    }
}
