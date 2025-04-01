//using GLTF.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Photon.Pun;

//public enum MonsterAttackType
//{
//    Bite, TaileAttack, Charge, ShootProjectile
//}

public class MonsterController : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<GameObject> moveTargetPos; //��Ʈ�� ���ƾ��ϴ� ��ġ ��ǥ
    [SerializeField] private Collider headCollider; //�Ӹ��� �¾Ҵ��� �����ϴ� �ݶ��̴�
    [SerializeField] private Transform shootPos; //����ü ��� ���
    [SerializeField] private float _groundRayDis;
    [SerializeField] private int _flyHigh;
    [SerializeField] private float _blockDis;
    [SerializeField] private int _sleepIndex;
    [SerializeField] private List<int> _restIndex = new List<int>();
    [SerializeField] private int _restTime;
    [SerializeField] private int _sleepTime;
    private List<MonsterAttackData> _monsterAttackData;
    private List<MonsterProjectileData> _monsterProjectileDatas; //���� ����ü ����
    private MonsterProjectileSpawnManager _projectileSpawnManager; //����ü ���� �Ŵ���
    private Collider _attackCollider;
    private NavMeshAgent _agent; //���̸޽�
    private Animator _anime; //�ִϸ��̼�
    private List<Transform> _detectPlayers = new List<Transform>(); //�����Ǵ� �÷��̾� ���
    private Vector3 _targetPlayerPos; //Ÿ���̵� �÷��̾� ��ġ ����
    private Vector3 _targetPos; //Ÿ���� �Ǵ� ��ġ ����
    private Vector3 _playerPos;
    private string _label; //��巹������ �ҷ��� ��
    private int _roSpeed; //�ε巴�� ���� ����ϴ� ����
    private int _currentPatrolIndex = 0; //���� �ִ� ��ǥ �ε��� ����
    private int _nextPatrolIndex = 1; //���� �����ִ� �ε��� ��ǥ
    private int _headMaxDamage; //�����ϸ� ���� �ɸ��� ��ġ
    private int _currentHeadDamage; //���� �Ӹ��� �󸶳� ������ �׿���?
    private int _maxHP; //�ִ� HP
    private int _lastAttack;
    private int _currentHP; //���� ������ HP
    private int _damage; //���� ������
    //private uint _biteDamage; //���� ���� ������
    //private uint _taileDamage; //���� ������
    //private uint _chargeDamage; //���� ������
    //private uint _flyDamage; //������� ������
    private float _detectRange; //�÷��̾� ���� �Ÿ�
    private float _attackRange; //���� ���� �Ÿ�
    private float _attackCoolTime; //���� ���� ��Ÿ��
    private float _elapsedTime; //��Ÿ��
    private float _minAttackRange; //�鹫�� �ϴ� ����
    private float _sturnTime; //���� ���� �ð�
    private float _trapTime; //Ʈ�� ���� �ð�
    private bool _isRo; //ȸ��������?
    private bool _isDie; //�׾�����?
    private bool _isHit; //�¾Ҵ���?
    private bool _isCanGetItem; //������ �ֿ�� �ִ� ����
    private bool _isBattle; //����������?
    private bool _isAttack; //����������?
    private bool _isRoar; //��ȿ������?
    private bool _isBackMove; //�ڷ� ����������
    private bool _isStun; //���� ��������?
    private bool _isTrap; //������ �ɸ� ��������?
    private bool _isPendingStun; //������ �ɸ����߿� ������ �ɷȳ�?
    private bool _isFly;
    private bool _isTakeOff;
    private bool _isLanding;
    private bool _isTooHigh;
    private bool _isGround;
    private bool _isBlock;
    private bool _isLink;
    private bool _isRun;
    private bool _isSleep;
    private bool _isalSleep;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _anime = GetComponent<Animator>();
    }

    private void Start()
    {
        _label = MonsterManager.Instance.MonsterSO.Label;
        _roSpeed = MonsterManager.Instance.MonsterSO.RoSpeed;
        _maxHP = MonsterManager.Instance.MonsterSO.MaxHP;
        _currentHP = _maxHP;
        //_biteDamage = MonsterManager.Instance.MonsterSO.BiteDamage;
        //_taileDamage = MonsterManager.Instance.MonsterSO.TaileDamage;
        //_chargeDamage = MonsterManager.Instance.MonsterSO.ChargeDamage;
        _detectRange = MonsterManager.Instance.MonsterSO.DetectRange;
        _attackRange = MonsterManager.Instance.MonsterSO.AttackRange;
        _attackCoolTime = MonsterManager.Instance.MonsterSO.AttackCoolTime;
        _minAttackRange = MonsterManager.Instance.MonsterSO.MinAttackRange;
        _headMaxDamage = MonsterManager.Instance.MonsterSO.HeadMaxDamage;
        _sturnTime = MonsterManager.Instance.MonsterSO.SturnTime;
        _monsterProjectileDatas = MonsterManager.Instance.MonsterSO.ProjectileDatas;
        _projectileSpawnManager = MonsterManager.Instance.MonsterProjectileSpawnManager;
        _monsterAttackData = MonsterManager.Instance.MonsterSO.MonsterAttackDatas;
        _trapTime = 5f;
        _currentHeadDamage = 0;
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
        _isTrap = false;
        _isPendingStun = false;
        _isFly = false;
        _isTakeOff = false;
        _isLanding = false;
        _isTooHigh = false;
        _isGround = true;
        _isBlock = false;
        _isLink = false;
        _isRun = false;
        _isSleep = false;
        _isalSleep = false;
        _agent.updatePosition = false;
        _agent.updateRotation = false;
        InitProjectile();
    }

    public int RestTime { get { return _restTime; } set { _restTime = value; } }
    public int SleepTime { get { return _sleepTime; } set { _sleepTime = value; } }
    public bool IsRo { get { return _isRo; } set { _isRo = value; } }
    public bool IsDie { get { return _isDie; } set { _isDie = value; } }
    public bool IsBattle { get { return _isBattle; } set { _isBattle = value; } }
    public bool IsRoar { get { return _isRoar; } set { _isRoar = value; } }
    public bool IsHit { get { return _isHit; } set { _isHit = value; } }
    public bool IsAttack { get { return _isAttack; } set { _isAttack = value; } }
    public bool IsBackMove { get { return _isBackMove; } set { _isBackMove = value; } }
    public bool IsCanGetItem { get { return _isCanGetItem; } set { _isCanGetItem = value; } }
    public bool IsStun { get { return _isStun; } set { _isStun = value; } }
    public bool IsTrap { get { return _isTrap; } set { _isTrap = value; } }
    public bool IsFly { get { return _isFly; } set { _isFly = value; } }
    public bool IsTakeOff { get { return _isTakeOff; } set { _isTakeOff = value; } }
    public bool IsLanding { get { return _isLanding; } set { _isLanding = value; } }
    public bool IsGround { get { return _isGround; } set { _isGround = value; } }
    public bool IsRun { get { return _isRun; } set { _isRun = value; } }
    public bool IsSleep { get { return _isSleep; } set { _isSleep = value; } }
    public bool IsalSleep { get { return _isalSleep; } set { _isalSleep = value; } }

    public void InitProjectile()
    {
        if(_projectileSpawnManager == null)
        {
            _projectileSpawnManager = MonsterManager.Instance.MonsterProjectileSpawnManager;
        }
        if(_monsterProjectileDatas == null)
        {
            _monsterProjectileDatas = MonsterManager.Instance.MonsterSO.ProjectileDatas;
        }
        _projectileSpawnManager.LoadProjectilesFromAddressable(_label, _monsterProjectileDatas);
    }

    public void SetAnime(string tag, bool value)
    {
        _anime.SetBool(tag, value);

        if(PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetBool", RpcTarget.Others);
        }
    }

    public void SetAnime(string tag)
    {
        _anime.SetTrigger(tag);

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetTrigger", RpcTarget.Others);
        }
    }

    public void SetAnime(string tag, float value)
    {
        _anime.SetFloat(tag, value);

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("SetFloat", RpcTarget.Others);
        }
    }

    [PunRPC]
    public void SetBool(string tag, bool value)
    {
        _anime.SetBool(tag, value);
    }

    [PunRPC]
    public void SetTrigger(string tag, bool value)
    {
        _anime.SetTrigger(tag);
    }

    [PunRPC]
    public void SetFloat(string tag, float value)
    {
        _anime.SetFloat(tag, value);
    }

    public void SetTargetPos(Vector3 pos) //Ÿ�� ��ġ ���� �׺�޽� ���
    {
        _targetPos = pos;
        if (_agent.isOnNavMesh)
        {
            _agent.SetDestination(pos);
        }
    }

    public void UpdatePatrolIndex()
    {
        _currentPatrolIndex = (_currentPatrolIndex + 1) % moveTargetPos.Count;
        _nextPatrolIndex = (_nextPatrolIndex + 1) % moveTargetPos.Count;
    }

    public Vector3 GetCurrentPatrolPos() //��Ʈ�ѽ� Ÿ���� �Ǵ� ��ġ ���� ����
    {
        Vector3 patrolPos = moveTargetPos[_currentPatrolIndex].transform.position;
        return patrolPos;
    }

    public Vector3 GetNextPatrolPos() //ȸ�� Ȥ�� ��� �����϶� ������ ��ǥ ����
    {
        Vector3 patrolPos = moveTargetPos[_nextPatrolIndex].transform.position;
        return patrolPos;
    }

    public Vector3 SetRunPos()
    {
        Vector3 runPos = moveTargetPos[_sleepIndex].transform.position;
        _currentPatrolIndex = _sleepIndex;
        _nextPatrolIndex = _sleepIndex++;
        return runPos;
    }

    public void NavMeshMatchMonsterPos() //�׺�޽� ��ǥ ���͸� ���󰡰���
    {
        if(_agent.enabled)
        {
            _agent.nextPosition = transform.position;
        }
    }

    public void NavMeshMatchMonsterRotation() //���͸��������� ȸ��
    {
        if (_agent.enabled)
        {
            _agent.transform.rotation = transform.rotation;
        }
    }

    public bool IsRestPos() //���� ����Ʈ ������ ���� �ٲٸ� ��
    {
        return _restIndex.Contains(_currentPatrolIndex);
    }

    public bool IsSleepPos()
    {
        return _currentPatrolIndex == _sleepIndex;
    }

    public bool IsReachTarget() //��Ʈ�� ��ǥ���� ��ǥ�� ���� �ߴ°�?
    {
        if(!_agent.enabled || !_agent.isOnNavMesh)
        {
            Vector3 currentPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 target = moveTargetPos[_nextPatrolIndex].transform.position;
            Vector3 targetPos = new Vector3(target.x, 0, target.z);

            return Vector3.Distance(currentPos, targetPos) < 5.0f;
        }
        return !_agent.pathPending && _agent.remainingDistance < 5.0f;
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
        dir.y = 0;
        dir.Normalize();

        float angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);
        _agent.enabled = false;

        if (Math.Abs(angle) < 30)
        {
            StartCoroutine(SmoothTurn(dir));
        }
        else
        {
            if(!_isFly)
            {
                SetAnime("IsRo", true);
                SetAnime("TurnAngle", angle);
            }
            else if(_isFly)
            {
                SetAnime("IsFlyRo", true);
                SetAnime("TurnAngle", angle);
            }

            StartCoroutine(WaitForEndRotateAnime());
        }
    }

    public void SmothRotateToPlayer() //�����Ҷ� �ڿ������� �÷��̾� �ٶ󺸸� �� �� �ֵ��� ȸ��
    {
        Vector3 dir = (_targetPlayerPos - transform.position).normalized;
        dir.y = 0;
        dir.Normalize();

        Quaternion targetRo = Quaternion.LookRotation(dir);
        transform.rotation = targetRo;

        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRo, roSpeed);
    }

    public void RequestRoar()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        photonView.RPC("Roar", RpcTarget.All);
    }

    [PunRPC]
    public void Roar()
    {
        IsRoar = true;
        StartCoroutine(WaitForEndRoarAnime());
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
        _isBattle = false;
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
                //_isBattle = true;
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
        if(_agent.isOnNavMesh)
        {
            _agent.SetDestination(_targetPlayerPos);
        }
    }

    public void RequestTakeDamage(int damage)
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("TakeDamage", RpcTarget.MasterClient, damage);
        }
    }

    public void RequestTakeHeadDamage(int damage)
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("TakeHeadDamage", RpcTarget.MasterClient, damage);
        }
    }

    [PunRPC]
    public void TakeDamage(int damage) //������ �ޱ�
    {
        //if (!PhotonNetwork.IsMasterClient) return;

        _currentHP -= damage;

        if(!_isBattle)
        {
            _isBattle = true;
        }

        if(_currentHP <= 0)
        {
            _currentHP = 0;
            photonView.RPC("Die", RpcTarget.All);
            return;
        }
        else
        {
            photonView.RPC("SyncHP", RpcTarget.All, _currentHP);
        }
    }

    [PunRPC]
    public void TakeHeadDamage(int damage) //�Ӹ��� ������ ������
    {
        //CheckMaster();

        if (!IsStun)
        {
            _currentHeadDamage += damage;
        }

        if (_currentHeadDamage >= _headMaxDamage)
        {
            _currentHeadDamage = 0;
            if (_isTrap)
            {
                _isPendingStun = true;
            }
            else
            {
                photonView.RPC("Stun", RpcTarget.All);
            }
        }
        else
        {
            photonView.RPC("SyncHeadDamage", RpcTarget.All, _currentHeadDamage);
        }
    }

    [PunRPC]
    public void Die() //�׾�����
    {
        _isDie = true;
        StartCoroutine(WaitForEndDieAnime());
    }

    [PunRPC]
    public void Stun()
    {
        _isStun = true;
        StartCoroutine(WaitForEndSturnAnime());
    }

    [PunRPC]
    public void SyncHP(int HP)
    {
        _currentHP = HP;
    }

    [PunRPC]
    public void SyncHeadDamage(int currentHeadDamage)
    {
        _currentHeadDamage = currentHeadDamage;
    }

    public void ApplyRootMotionMovement() //��Ʈ��� ��ǥ ���� �̵�
    {
        //if (!PhotonNetwork.IsMasterClient) return;

        Vector3 rootMotionMove = _anime.deltaPosition;

        if (_isFly) 
        {
            transform.position += rootMotionMove;
        }
        else
        {
            Vector3 navMeshMove = _agent.desiredVelocity.normalized * rootMotionMove.magnitude;

            Vector3 finalMove = Vector3.Lerp(rootMotionMove, navMeshMove, 0.5f);

            Vector3 move = transform.position + finalMove;
            move.y = GetGroundHight();

            transform.position = move;
            if (_agent.desiredVelocity.sqrMagnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_agent.desiredVelocity.normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _roSpeed);
            }
        }
    }

    public float GetGroundHight()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position + Vector3.up * 5f, Vector3.down, out hit, 20f, LayerMask.GetMask("Ground")))
        {
            return hit.point.y;
        }
        return transform.position.y;
    }

    public void Link()
    {
        if (_agent.isOnOffMeshLink && !_isLink)
        {
            StartCoroutine(WaitForEndLink());
        }
    }

    public void ChooseAttackType()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        int attackType;
        while (true)
        {
            attackType = Random.Range(0, _monsterAttackData.Count);

            if (attackType != _lastAttack)
            {
                _lastAttack = attackType;
                break;
            }
        }
        attackType = 3;
        photonView.RPC("Attack", RpcTarget.All, attackType);
    }

    [PunRPC]
    public void Attack(int attackType)
    {
        //int attackType;
        //while (true)
        //{
        //    attackType = Random.Range(0, _monsterAttackData.Count);

        //    if(attackType != _lastAttack)
        //    {
        //        _lastAttack = attackType;
        //        break;
        //    }
        //}

        SetAnime(_monsterAttackData[attackType].AnimeName);
        _damage = _monsterAttackData[attackType].Damage;
        //var obj = transform.GetComponentsInChildren<Collider>(true);
        //_attackCollider = null;
        //if(obj != null)
        //{
        //    _attackCollider = obj.GetComponent<Collider>();
        //    if(_attackCollider != null)
        //    {
        //        _attackCollider.enabled = true;
        //    }
        //}

        var obj = transform.GetComponentsInChildren<Collider>(true);
        foreach (var col in obj)
        {
            if (col.name == _monsterAttackData[attackType].AttackColliderName)
            {
                _attackCollider = col;
                _attackCollider.enabled = true;
                break;
            }
        }

        if (_monsterAttackData[attackType].NeedRo)
        {
            _agent.enabled = false;
        }

        if (_monsterAttackData[attackType].NeedFly)
        {
            StartCoroutine(WaitForEndFlyAttackAnime(_attackCollider));
        }
        else
        {
            StartCoroutine(WaitForEndAttackAnime(_attackCollider, attackType));
        }

        //switch(attackType)
        //{
        //    case (int)MonsterAttackType.Bite:
        //        {
        //            _lastAttack = 0;
        //            _damage = _biteDamage;
        //            attackColliders[0].enabled = true;
        //            _anime.PlayMonsterBiteAnime();
        //            StartCoroutine(WaitForEndAttackAnime());
        //            break;
        //        }

        //    case (int)MonsterAttackType.TaileAttack:
        //        {
        //            _lastAttack = 1;
        //            _agent.enabled = false;
        //            _damage = _taileDamage;
        //            attackColliders[1].enabled = true;
        //            _anime.PlayMonsterTaileAttackAnime();
        //            StartCoroutine(WaitForEndAttackAnime());
        //            break;
        //        }

        //    case (int)MonsterAttackType.Charge:
        //        {
        //            _lastAttack = 2;
        //            _damage = _chargeDamage;
        //            attackColliders[2].enabled = true;
        //            attackColliders[0].enabled = true;
        //            _anime.PlayMonsterChargeAnime();
        //            StartCoroutine(WaitForEndAttackAnime());
        //            break;
        //        }

        //    case (int)MonsterAttackType.ShootProjectile:
        //        {
        //            _lastAttack = 3;
        //            _damage = 0;
        //            _anime.PlayMonsterShootProjectileAnime();
        //            StartCoroutine(WaitForEndAttackAnime());
        //            break;
        //        }

        //}
    }
    
    public void RequestShootProjectileAnimationEvent()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        photonView.RPC("ShootProjectile", RpcTarget.All, _lastAttack);
    }

    [PunRPC]
    public void ShootProjectile(int index)
    {
        var temp = _monsterAttackData[index].ProjectileType;
        var projectile = _projectileSpawnManager.GetProjectiles(temp);

        if(projectile != null)
        {
            var match = _monsterProjectileDatas.Find(x => x.ProjectileType == projectile.ProjectileType);
            if (match != null)
            {
                projectile.SetData(match);
                projectile.InitShooter(this.gameObject, _projectileSpawnManager);
            }
            projectile.transform.position = shootPos.position;
            projectile.transform.forward = shootPos.forward;
        }
    }

    public void RequestBackMove()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        
        photonView.RPC("BackMove", RpcTarget.All);
    }

    [PunRPC]
    public void BackMove()
    {
        _isBackMove = true;
        StartCoroutine(WaitForEndBackMoveAnime());
    }

    public void RequestTrap()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        photonView.RPC("Trap", RpcTarget.All, _trapTime);
    }

    [PunRPC]
    public void Trap(float time)
    {
        Debug.Log("Ʈ��");
        StartCoroutine(WaitForEndTrap(time));
    }

    public void CheckTooHigh()
    {
        if(transform.position.y >= _flyHigh)
        {
            _isTooHigh = true;
        }
    }

    public void CheckGround()
    {
        if(_isFly)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, Vector3.down, out hit, _groundRayDis, LayerMask.GetMask("Ground")))
            {
                _isGround = true;
            }
        }
    }

    public void SnapToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 5f, Vector3.down, out hit, 20f, LayerMask.GetMask("Ground")))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y;
            Debug.Log(hit.point.y);
            transform.position = pos;
        }
    }

    public void IsBlock()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position + Vector3.up * 2f, transform.forward, out hit, _blockDis, LayerMask.GetMask("Ground")))
        {
            _isBlock = true;
        }
        else
        {
            _playerPos = transform.position;
            _isBlock = false;
        }
    }

    public void BlockMove()
    {
        if(_isBlock)
        {
            transform.position = _playerPos;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position + transform.forward * _blockDis, 1f);
    }

    public void TakeOff()
    {
        StartCoroutine(WaitForEndTakeOffAnime());
    }

    public void Patrol()
    {
        StartCoroutine(WaitForEndPatrolAnime());
    }

    public void Landing()
    {
        StartCoroutine(WaitForEndLandingAnime());
    }

    private IEnumerator SmoothTurn(Vector3 direct) //30�� ���� �ڿ������� �ִϸ��̼� ���� ȸ��
    {
        Quaternion targetRo = Quaternion.LookRotation(direct);

        while (Quaternion.Angle(transform.rotation, targetRo) > 1.4f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRo, _roSpeed);
            yield return null;
        }

        _agent.enabled = true;
        IsRo = false;
        transform.rotation = targetRo;
    }


    private IEnumerator WaitForEndRotateAnime() //ȸ�� �ִϸ��̼��� ���������� ���
    {
        yield return new WaitForSeconds(0.3f);

        yield return new WaitUntil(() => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        Vector3 dir;
        if (IsBattle)
        {
            dir = (_targetPlayerPos - transform.position).normalized;
        }
        else
        {
            dir = (_targetPos - transform.position).normalized;
        }
        dir.y = 0;
        dir.Normalize();

        float angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);

        if (Math.Abs(angle) < 30)
        {
            StartCoroutine(SmoothTurn(dir));
        }
        else
        {
            _agent.enabled = true;
            IsRo = false;
        }
    }

    public IEnumerator WaitForEndAttackAnime(Collider co, int index)
    {
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        CheckAttackOnCollider(co);
        if (_monsterAttackData[index].NeedRo)
        {
            _agent.enabled = true;
        }
        _isAttack = false;
    }

    public IEnumerator WaitForEndFlyAttackAnime(Collider co)
    {
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        SetAnime("FlyAttack");
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        SetAnime("FlyAttack2");
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        SetAnime("FlyAttack3");
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        CheckAttackOnCollider(co);
        _isAttack = false;
    }

    public IEnumerator WaitForEndRoarAnime()
    {
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        IsRoar = false;
    }

    public IEnumerator WaitForEndBackMoveAnime()
    {
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        _isBackMove = false;
    }

    public IEnumerator WaitForEndDieAnime()
    {
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        _isCanGetItem = true;
    }

    public IEnumerator WaitForEndSturnAnime()
    {
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        SetAnime("IsSturn2");
        yield return new WaitForSeconds(0.3f);
        yield return new WaitForSeconds(_sturnTime);
        SetAnime("IsSturn3");
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        _isStun = false;
    }

    public IEnumerator WaitForEndTrap(float time)
    {
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        SetAnime("IsTrap2");
        yield return new WaitForSeconds(0.3f);
        yield return new WaitForSeconds(time);
        SetAnime("IsTrap3");
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        _isTrap = false;

        if (_isPendingStun && _currentHeadDamage >= _headMaxDamage)
        {
            _isPendingStun = false;

            _currentHeadDamage = 0;

            photonView.RPC("Stun", RpcTarget.All);
        }
    }

    public IEnumerator WaitForEndTakeOffAnime()
    {
        SetAnime("TakeOff");
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil( () => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        _isFly = true;
        _isGround = false;
        SetAnime("TakeOff2");
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        SetAnime("TakeOff3");
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        _isTakeOff = false;
    }

    public IEnumerator WaitForEndPatrolAnime()
    {
        SetAnime("Patrol");
        yield return new WaitForSeconds(0.3f);
        while (!_isTooHigh)
        {
            CheckTooHigh();
            yield return null;
        }
        SetAnime("Patrol2", true);
    }

    public IEnumerator WaitForEndLandingAnime()
    {
        SetAnime("Landing");
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        SetAnime("Landing2");
        yield return new WaitForSeconds(0.3f);
        while(!_isGround)
        {
            CheckGround();
            yield return null;
        }
        SetAnime("Landing3");
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        SetAnime("Landing4");
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        SnapToGround();
        _isLanding = false;
        _isTooHigh = false;
    }

    public IEnumerator WaitForEndLink()
    {
        _isLink = true;
        TurnOffAgent();

        OffMeshLinkData linkData = _agent.currentOffMeshLinkData;
        Vector3 endPos = linkData.endPos + Vector3.up * _agent.baseOffset;

        SetAnime("IsLink", true);
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => _anime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);
        SetAnime("Link");

        while (Vector3.Distance(transform.position, endPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * 5f);
            yield return null;
        }
        SetAnime("IsLink", false);
        _agent.CompleteOffMeshLink();
        TurnOnAgent();
        _isLink = false;
        SetAnime("IsChase", true);
    }

    public void OnTriggerEnter(Collider other)
    {
        //CheckMaster();

        if (other.CompareTag("Player"))
        {
            CheckAttackOnCollider(_attackCollider);

            Vector3 contactPoint = other.ClosestPoint(transform.position);
            Transform current = other.transform;

            while (current != null)
            {
                PlayerController player = current.GetComponent<PlayerController>();

                if (player != null)
                {
                    player.GetComponent<PlayerController>().TakeDamage(contactPoint, _damage);
                    return;
                }

                current = current.parent;
            }

        }

        if (other.CompareTag("Trap"))
        {
            _isTrap = true;
        }
    }

    public void CheckAttackOnCollider(Collider co)
    {
        if(co != null)
        {
            if (co.enabled)
            {
                co.enabled = false;
            }
        }
    }

    public void TurnOnAgent()
    {
        _agent.updatePosition = true;
        _agent.updateRotation = true;
    }

    public void TurnOffAgent()
    {
        _agent.updatePosition = false;
        _agent.updateRotation = false;
    }
}
