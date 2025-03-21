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
    [SerializeField] private List<Vector3> moveTargetPos; //��Ʈ�� ���ƾ��ϴ� ��ġ ��ǥ
    [SerializeField] private float roSpeed; //�ε巴�� ���� ����ϴ� ����
    [SerializeField] private List<Collider> attackColliders;
    private NavMeshAgent agent; //���̸޽�
    private MonsterAnimationController anime; //�ִϸ��̼�
    private List<Transform> _detectPlayers = new List<Transform>(); //�����Ǵ� �÷��̾� ���
    private Vector3 _targetPlayerPos; //Ÿ���̵� �÷��̾� ��ġ ����
    private Vector3 _targetPos; //Ÿ���� �Ǵ� ��ġ ����
    private int _currentPatrolIndex = 0; //���� �ִ� ��ǥ �ε��� ����
    private int _nextPatrolIndex = 1; //���� �����ִ� �ε��� ��ǥ
    private int _maxHP; //�ִ� HP
    private int _lastAttack;
    private float _currentHP; //���� ������ HP
    private float _damage; //���� ������
    private float _biteDamage; //���� ���� ������
    private float _taileDamage; //���� ������
    private float _chargeDamage; //���� ������
    private float _flyDamage; //������� ������
    private float _detectRange; //�÷��̾� ���� �Ÿ�
    private float _attackRange; //���� ���� �Ÿ�
    private float _attackCoolTime; //���� ���� ��Ÿ��
    private float _elapsedTime; //��Ÿ��
    private float _minAttackRange; //�鹫�� �ϴ� ����
    private bool _isRo; //ȸ��������?
    private bool _isDie; //�׾�����?
    private bool _isHit; //�¾Ҵ���?
    private bool _isCanGetItem; //������ �ֿ�� �ִ� ����
    private bool _isBattle; //����������?
    private bool _isAttack; //����������?
    private bool _isRoar; //��ȿ������?
    private bool _isBackMove; //�ڷ� ����������
    private bool _isStun; //���� ��������?

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

    public void SetTargetPos(Vector3 pos) //Ÿ�� ��ġ ���� �׺�޽� ���
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

    public Vector3 GetCurrentPatrolPos() //��Ʈ�ѽ� Ÿ���� �Ǵ� ��ġ ���� ����
    {
        Vector3 patrolPos = moveTargetPos[_currentPatrolIndex];
        return patrolPos;
    }

    public Vector3 GetNextPatrolPos() //ȸ�� Ȥ�� ��� �����϶� ������ ��ǥ ����
    {
        Vector3 patrolPos = moveTargetPos[_nextPatrolIndex];
        return patrolPos;
    }

    public void NavMeshMatchMonsterPos() //�׺�޽� ��ǥ ���͸� ���󰡰���
    {
        if(agent.enabled)
        {
            agent.nextPosition = transform.position;
        }
    }

    public void NavMeshMatchMonsterRotation() //���͸��������� ȸ��
    {
        if (agent.enabled)
        {
            agent.transform.rotation = transform.rotation;
        }
    }

    public bool IsRestPos() //���� ����Ʈ ������ ���� �ٲٸ� ��
    {
        return _currentPatrolIndex == 0 || _currentPatrolIndex == 2;
    }

    public bool IsReachTarget() //��Ʈ�� ��ǥ���� ��ǥ�� ���� �ߴ°�?
    {
        return !agent.pathPending && agent.remainingDistance < 5.0f;
    }

    public bool IsNeedRo() //Ÿ����� ���� ��� �� ȸ���� �ʿ�����?
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

    public void RotateToTarget() //Ÿ���� ���� ȸ��
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
            Debug.Log("����ȸ��");
            StartCoroutine(SmoothTurn(dir));
        }
        else
        {
            anime.PlayMonsterRotateAnime(true);
            anime.SetRoAngle(angle);
            StartCoroutine(WaitForEndRotateAnime());
        }
    }

    public void SmothRotateToPlayer() //�����Ҷ� �ڿ������� �÷��̾� �ٶ󺸸� �� �� �ֵ��� ȸ��
    {
        Vector3 dir = (_targetPlayerPos - transform.position).normalized;

        Quaternion targetRo = Quaternion.LookRotation(dir);
        transform.rotation = targetRo;

        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRo, roSpeed);
    }

    public void OnHit(Transform player) //���� �������� (���� �Ⱦ�����)
    {
        _isBattle = true;
        _targetPlayerPos = player.position;
    }

    public Vector3 GetFirstAttckPlayer() //���� �������� (���� �Ⱦ�����)
    {
        return _targetPlayerPos;
    }

    public void CheckPlayer() //�÷��̾� ���� �� ��ǥ ����
    {
        DetectedPlayer();
        if(_detectPlayers.Count > 0)
        {
            _targetPlayerPos = GetClosePlayer();
            SetTargetplayer();
        }
    }

    public bool IsCanAttackPlayer() //���� ����?
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

    public void UpdateAttackCoolTime() //��Ÿ�� ���
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

    public void DetectedPlayer() //�÷��̾� ����
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

    public Vector3 GetClosePlayer() //���� ������ �ִ� �÷��̾� ã��
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

    public void TakeDamage(float damage) //������ �ޱ�
    {
        _currentHP -= damage;

        if(_currentHP <= 0)
        {
            _currentHP = 0;
            Die();
        }
    }

    public void Die() //�׾�����
    {
        _isDie = true;
    }

    public void ApplyRootMotionMovement() //��Ʈ��� ��ǥ ���� �̵�
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

    private IEnumerator SmoothTurn(Vector3 direct) //30�� ���� �ڿ������� �ִϸ��̼� ���� ȸ��
    {
        Debug.Log("�������� ����");
        Quaternion targetRo = Quaternion.LookRotation(direct);

        while (Quaternion.Angle(transform.rotation, targetRo) > 1.4f)
        {
            Debug.Log("������ ȸ��");
            Debug.Log(Quaternion.Angle(transform.rotation, targetRo));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRo, roSpeed);
            Debug.Log(IsRo);
            yield return null;
        }

        agent.enabled = true;
        IsRo = false;
        Debug.Log("������ ȸ��");
        transform.rotation = targetRo;
    }


    private IEnumerator WaitForEndRotateAnime() //ȸ�� �ִϸ��̼��� ���������� ���
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
        Debug.Log("�ִϸ��̼� �Ϸ�");
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
        Debug.Log("���� �ڷ�ƾ");
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => anime.Anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        _isCanGetItem = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾� ������");
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
