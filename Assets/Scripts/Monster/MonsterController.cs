using GLTF.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum MonsterAttackType
{
    Bite, TaileAttack, Charge
}

public class MonsterController : MonoBehaviour
{
    [SerializeField] private List<Vector3> moveTargetPos; //패트롤 돌아야하는 위치 좌표
    [SerializeField] private float roSpeed; //부드럽게 돌때 사용하는 변수
    [SerializeField] private List<Collider> attackColliders;
    private NavMeshAgent agent; //네이메쉬
    private MonsterAnimationController anime; //애니메이션
    private List<Transform> _detectPlayers = new List<Transform>(); //감지되는 플레이어 목록
    private Vector3 _targetPlayerPos; //타깃이된 플레이어 위치 정보
    private Vector3 _targetPos; //타깃이 되는 위치 정보
    private int _currentPatrolIndex = 0; //현재 있는 좌표 인덱스 정보
    private int _nextPatrolIndex = 1; //현재 가고있는 인덱스 좌표
    private int _maxHP; //최대 HP
    private int _lastAttack;
    private float _currentHP; //현재 몬스터의 HP
    private float _damage; //몬스터 데미지
    private float _biteDamage; //몬스터 물기 데미지
    private float _taileDamage; //꼬리 데미지
    private float _chargeDamage; //돌진 데미지
    private float _flyDamage; //비행공격 데미지
    private float _detectRange; //플레이어 감지 거리
    private float _attackRange; //몬스터 공격 거리
    private float _attackCoolTime; //몬스터 공격 쿨타임
    private float _elapsedTime; //쿨타임
    private float _minAttackRange; //백무브 하는 범위
    private bool _isRo; //회전중인지?
    private bool _isDie; //죽었는지?
    private bool _isHit; //맞았는지?
    private bool _isCanGetItem; //아이템 주울수 있는 상태
    private bool _isBattle; //전투중인지?
    private bool _isAttack; //공격중인지?
    private bool _isRoar; //포효중인지?
    private bool _isBackMove; //뒤로 가는중인지
    private bool _isStun; //스턴 상태인지?

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anime = MonsterManager.Instance.AnimationController;
    }

    private void Start()
    {
        _maxHP = MonsterManager.Instance.MonsterSO.MaxHP;
        _currentHP = _maxHP;
        _biteDamage = MonsterManager.Instance.MonsterSO.BiteDamage;
        _taileDamage = MonsterManager.Instance.MonsterSO.TaileDamage;
        _taileDamage = MonsterManager.Instance.MonsterSO.FlyDamage;
        _chargeDamage = MonsterManager.Instance.MonsterSO.ChargeDamage;
        _detectRange = MonsterManager.Instance.MonsterSO.DetectRange;
        _attackRange = MonsterManager.Instance.MonsterSO.AttackRange;
        _attackCoolTime = MonsterManager.Instance.MonsterSO.AttackCoolTime;
        _minAttackRange = MonsterManager.Instance.MonsterSO.MinAttackRange;
        _damage = 0;
        _elapsedTime = _attackCoolTime;
        _isBattle = false;
        _isDie = false;
        _isRo = false;
        _isRoar = false;
        _isHit = false;
        _isAttack = false;
        _isBackMove = false;
        _isCanGetItem = false;
        _isStun = false;
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    public bool IsRo { get { return _isRo; } set { _isRo = value; } }
    public bool IsDie { get { return _isDie; } set { _isDie = value; } }
    public bool IsBattle { get { return _isBattle; } set { _isBattle = value; } }
    public bool IsRoar { get { return _isRoar; } set { _isRoar = value; } }
    public bool IsHit { get { return _isHit; } set { _isHit = value; } }
    public bool IsAttack { get { return _isAttack; } set { _isAttack = value; } }
    public bool IsBackMove { get { return _isBackMove; } set { _isBackMove = value; } }
    public bool IsCanGetItem { get { return _isCanGetItem; } set { _isCanGetItem = value; } }
    public bool IsStun { get { return _isStun; } set { _isStun = value; } }

    public void SetTargetPos(Vector3 pos) //타깃 위치 정보 네비메쉬 등록
    {
        _targetPos = pos;
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(pos);
        }
    }

    public void UpdatePatrolIndex()
    {
        _currentPatrolIndex = (_currentPatrolIndex + 1) % moveTargetPos.Count;
        _nextPatrolIndex = (_nextPatrolIndex + 1) % moveTargetPos.Count;
    }

    public Vector3 GetCurrentPatrolPos() //패트롤시 타깃이 되는 위치 정보 변경
    {
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
            dir = (_targetPlayerPos - transform.position).normalized;
        }
        else
        {
            dir = (_targetPos - transform.position).normalized;
        }

        float angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);
        IsRo = true;
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
            Debug.Log("작은회전");
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
        transform.rotation = targetRo;

        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRo, roSpeed);
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

    public bool IsTooClose()
    {
        float dis = Vector3.Distance(transform.position, _targetPlayerPos);

        if (dis <= _minAttackRange)
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

    public bool IsCanFindPlayer()
    {
        return _detectPlayers.Count > 0;
    }

    public void ResetHit()
    {
        _isHit = false;
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
        if (agent.desiredVelocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.desiredVelocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * roSpeed);
        }
    }

    public void Attack()
    {
        int attackType;
        while (true)
        {
            attackType = Random.Range(0, Enum.GetValues(typeof(MonsterAttackType)).Length);

            if(attackType != _lastAttack)
            {
                break;
            }
        }

        switch(attackType)
        {
            case (int)MonsterAttackType.Bite:
                {
                    _lastAttack = 0;
                    _damage = _biteDamage;
                    attackColliders[0].enabled = true;
                    anime.PlayMonsterBiteAnime();
                    StartCoroutine(WaitForEndAttackAnime());
                    break;
                }

            case (int)MonsterAttackType.TaileAttack:
                {
                    _lastAttack = 1;
                    agent.enabled = false;
                    _damage = _taileDamage;
                    attackColliders[1].enabled = true;
                    anime.PlayMonsterTaileAttackAnime();
                    StartCoroutine(WaitForEndAttackAnime());
                    break;
                }

            case (int)MonsterAttackType.Charge:
                {
                    _lastAttack = 2;
                    _damage = _chargeDamage;
                    attackColliders[2].enabled = true;
                    attackColliders[0].enabled = true;
                    anime.PlayMonsterChargeAnime();
                    StartCoroutine(WaitForEndAttackAnime());
                    break;
                }

        }
    }

    private IEnumerator SmoothTurn(Vector3 direct) //30도 이하 자연스럽게 애니메이션 없이 회전
    {
        Debug.Log("스무스턴 들어옴");
        Quaternion targetRo = Quaternion.LookRotation(direct);

        while (Quaternion.Angle(transform.rotation, targetRo) > 1.4f)
        {
            Debug.Log("스무스 회전");
            Debug.Log(Quaternion.Angle(transform.rotation, targetRo));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRo, roSpeed);
            Debug.Log(IsRo);
            yield return null;
        }

        agent.enabled = true;
        IsRo = false;
        Debug.Log("스무스 회전");
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
        else
        {
            agent.enabled = true;
            IsRo = false;
        }
    }

    public IEnumerator WaitForEndAttackAnime()
    {
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => anime.Anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        CheckAttackOnCollider();
        Debug.Log("애니메이션 완료");
        if(_lastAttack == 1)
        {
            agent.enabled = true;
        }
        _isAttack = false;
    }

    public IEnumerator WaitForEndRoarAnime()
    {
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => anime.Anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        IsRoar = false;
    }

    public IEnumerator WaitForEndBackMoveAnime()
    {
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => anime.Anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        _isBackMove = false;
    }

    public IEnumerator WaitForEndDieAnime()
    {
        Debug.Log("죽음 코루틴");
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => anime.Anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        _isCanGetItem = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("플레이어 공격함");
            CheckAttackOnCollider();
        }
    }

    public void CheckAttackOnCollider()
    {
        foreach(var co in attackColliders)
        {
            if(co.enabled)
            {
                co.enabled = false;
            }
        }
    }
}
