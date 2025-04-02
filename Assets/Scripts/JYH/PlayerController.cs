using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
[RequireComponent(typeof(PlayerCostume))]
[RequireComponent(typeof(PlayerInteraction))]
public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    private bool _hasTransform = false;

    private Transform _transform = null;

    private Transform getTransform
    {
        get
        {
            if (_hasTransform == false)
            {
                _transform = transform;
                _hasTransform = true;
            }
            return _transform;
        }
    }

    private bool _hasAnimator = false;

    private Animator _animator = null;

    private Animator getAnimator
    {
        get
        {
            if (_hasAnimator == false)
            {
                _hasAnimator = TryGetComponent(out _animator);
            }
            return _animator;
        }
    }

    private bool _hasPlayerCostume = false;

    private PlayerCostume _playerCostume = null;

    private PlayerCostume getPlayerCostume
    {
        get
        {
            if (_hasPlayerCostume == false)
            {
                _hasPlayerCostume = TryGetComponent(out _playerCostume);
            }
            return _playerCostume;
        }
    }
    private bool _hasPlayerInteraction = false;

    private PlayerInteraction _playerInteraction = null;

    private PlayerInteraction getPlayerInteraction
    {
        get
        {
            if (_hasPlayerInteraction == false)
            {
                _hasPlayerInteraction = TryGetComponent(out _playerInteraction);
            }
            return _playerInteraction;
        }
    }

    private Coroutine _coroutine = null;

    [Header("최대 체력"), SerializeField, Range(MinValue, MaxLife)]
    private int _fullLife;

    public int fullLife
    {
        get
        {
            return _fullLife;
        }
        set
        {
            _fullLife = Mathf.Clamp(value, MinValue, MaxLife);
            if (_fullLife < _currentLife)
            {
                _currentLife = _fullLife;
            }
        }
    }

    [Header("현재 체력"), SerializeField, Range(MinValue, MaxLife)]
    private int _currentLife;

    public int currentLife
    {
        get
        {
            return _currentLife;
        }
        set
        {
            _currentLife = Mathf.Clamp(value, MinValue, MaxLife);
            if (_fullLife < _currentLife)
            {
                _currentLife = _fullLife;
            }
        }
    }

    private Vector3 _forward = Vector3.zero;

    private Action<int, int> _lifeAction = null;

    [Header("최대 스태미너"), SerializeField, Range(MinValue, MaxStamina)]
    private float _fullStamina;

    public float fullStamina
    {
        get
        {
            return _fullStamina;
        }
        set
        {
            _fullStamina = Mathf.Clamp(value, MinValue, MaxStamina);
            if (_fullStamina < _currentStamina)
            {
                _currentStamina = _fullStamina;
            }
        }
    }

    [Header("현재 스태미너"), SerializeField, Range(MinValue, MaxStamina)]
    private float _currentStamina;

    public float currentStamina
    {
        get
        {
            return _currentStamina;
        }
        set
        {
            _currentStamina = Mathf.Clamp(value, MinValue, MaxStamina);
            if (_fullStamina < _currentStamina)
            {
                _currentStamina = _fullStamina;
            }
        }
    }

    public bool attackable
    {
        private get;
        set;
    }
    public Player player
    {
        get
        {
            return photonView.Owner;
        }
    }

    [SerializeField]
    private int _damage = 0;

    private bool _swing = false;

    private const int MinValue = 0;
    private const int MaxLife = 200;
    private const float MaxStamina = 250;
    private const string IdleToLeftTag = "IdleToLeft";
    private const string IdleToRightTag = "IdleToRight";
    private const string IdleToBackTag = "IdleToBack";
    private static readonly float MinInput = -1;
    private static readonly float MaxInput = 1;
    private static readonly float RotationDamping = 10;
    private static readonly float DashStamina = 5;
    private static readonly float JumpStamina = 20f;
    private static readonly float RecoverTime = 2.3f;
    private static readonly float RecoverStamina = 5f;
    private static readonly string VerticalTag = "Vertical";
    private static readonly string HorizontalTag = "Horizontal";
    private static readonly string RunTag = "Run";
    private static readonly string DashTag = "Dash";
    private static readonly string JumpTag = "Jump";
    private static readonly string AttackTag = "Attack";
    private static readonly string HitFrontTag = "HitFront";
    private static readonly string HitBackTag = "HitBack";
    private static readonly string HitLeftTag = "HitLeft";
    private static readonly string HitRightTag = "HitRight";
    private static readonly string RecoverTag = "Recover";
    private static readonly string DeadTag = "Dead";

    public static Action<PlayerController, string> playerAction = null;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_fullLife < _currentLife)
        {
            _currentLife = _fullLife;
        }
        if (_fullStamina < _currentStamina)
        {
            _currentStamina = _fullStamina;
        }
    }
#endif

    private void Roll()
    {
        if (_currentStamina >= JumpStamina)
        {
            _currentStamina -= JumpStamina;
        }
        else
        {
            _currentStamina = 0;
        }
        SoundManager.Instance.PlaySFX(SoundManager.HunterSfxType.HunterDodge);
    }

    private void Swing()
    {
        _swing = true;
        SoundManager.Instance.PlaySFX((SoundManager.HunterSfxType)UnityEngine.Random.Range(0, 2));
    }

    private void OnEquipChanged(ItemName itemKey)
    {
        int index = (int)itemKey;
        Equip(index); // 내 장비 적용
        if (PhotonNetwork.InRoom) // 네트워크로 다른 유저들에게도 장비 적용
        {
            photonView.RPC("Equip", RpcTarget.OthersBuffered, index);
        }
    }

    private void Start()
    {
        if (photonView.IsMine == true)
        {
            InvenToryCtrl.Instance.OnEquippedChanged += OnEquipChanged;
            Player player = PhotonNetwork.LocalPlayer;
            if (player != null)
            {
                IEnumerable<int> itemArray = PlayerCostume.GetEquipList(player.CustomProperties);
                if (itemArray != null)
                {
                    foreach(int value in itemArray)
                    {
                        OnEquipChanged((ItemName)value);
                    }
                }
            }
        }
        else
        {
            playerAction?.Invoke(this, photonView.Owner.NickName);
        }
    }

    private void Update()
    {
        if (photonView.IsMine == true && (_currentLife > 0 || _fullLife == _currentLife))
        {
            bool attack = Input.GetMouseButton(0);
            if (attack != getAnimator.GetBool(AttackTag))
            {
                if (attackable == false && attack == true)
                {
                    return;
                }
                SetAnimation(AttackTag, attack);
                if (attack == true)
                {
                    Rotate();
                    Swing();
                }
            }
            else if (attack == false)
            {
                if (Input.GetButton(JumpTag) == false)
                {
                    if (getAnimator.GetBool(JumpTag) == true)
                    {
                        SetAnimation(JumpTag, false);
                    }
                    Camera camera = Camera.main;
                    if (camera != null)
                    {
                        Vector2 input = new Vector2(Mathf.Clamp(Input.GetAxis(HorizontalTag), MinInput, MaxInput), Mathf.Clamp(Input.GetAxis(VerticalTag), MinInput, MaxInput));
                        if (input != Vector2.zero)
                        {
                            _forward = Quaternion.AngleAxis(Vector2.SignedAngle(input, Vector2.up), Vector3.up) * Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up).normalized;
                            if (_coroutine == null)
                            {
                                _coroutine = StartCoroutine(DoMoveStart());
                                IEnumerator DoMoveStart()
                                {
                                    Vector2 previousDirection = new Vector2(Vector3.Dot(getTransform.right, _forward), Vector3.Dot(getTransform.forward, _forward));
                                    Move(previousDirection);
                                    while (IsRunning() == false)
                                    {
                                        Vector2 currentDirection = new Vector2(Vector3.Dot(getTransform.right, _forward), Vector3.Dot(getTransform.forward, _forward));
                                        if (Mathf.Sign(previousDirection.x) != Mathf.Sign(currentDirection.x) || Mathf.Sign(previousDirection.y) != Mathf.Sign(currentDirection.y))
                                        {
                                            Move(currentDirection);
                                            previousDirection = currentDirection;
                                        }
                                        yield return null;
                                    }
                                    while (IsRunning() == true)
                                    {
                                        if (Input.GetButton(DashTag) == true)
                                        {
                                            if (_currentStamina > 0)
                                            {
                                                _currentStamina -= Time.deltaTime * DashStamina;
                                                Dash(true);
                                                if (_currentStamina < 0)
                                                {
                                                    _currentStamina = 0;
                                                }
                                            }
                                            else
                                            {
                                                Dash(false);
                                            }
                                        }
                                        else
                                        {
                                            Dash(false);
                                        }
                                        getTransform.forward = Vector3.Lerp(getTransform.forward, _forward, Time.deltaTime * RotationDamping);
                                        yield return null;
                                    }
                                    _coroutine = null;
                                }
                            }
                        }
                        else if (_forward != Vector3.zero)
                        {
                            if (_coroutine != null)
                            {
                                StopCoroutine(_coroutine);
                                _coroutine = null;
                            }
                            _forward = Vector3.zero;
                            Move(Vector2.zero);
                        }
                    }
                }
                else if (getAnimator.GetBool(JumpTag) == false && _currentStamina >= JumpStamina)
                {
                    SetAnimation(JumpTag, true);
                }
            }
            if (getAnimator.GetBool(DashTag) == false && _fullStamina > _currentStamina)
            {
                _currentStamina += Time.deltaTime * RecoverStamina;
                if (_currentStamina > _fullStamina)
                {
                    _currentStamina = _fullStamina;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine == true && _swing == true)
        {
            if (other.TryGetComponent(out MonsterController monsterController))
            {
                monsterController.TakeDamage(_damage);
                StopSwing();
            }
            else if (other.tag == "MonsterHead")
            {
                Transform transform = other.transform.parent;
                while (transform.parent != null)
                {
                    transform = transform.parent;
                    if (transform.TryGetComponent(out monsterController))
                    {
                        monsterController.TakeHeadDamage(_damage);
                        StopSwing();
                        return;
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (photonView.IsMine == true)
        {
            PhotonNetwork.RemoveBufferedRPCs();
        }
    }

    private void Rotate()
    {
        string name = GetAnimationName();
        switch (name)
        {
            case IdleToLeftTag:
                RotateLeft();
                break;
            case IdleToRightTag:
                RotateRight();
                break;
            case IdleToBackTag:
                RotateBack();
                break;
        }
    }

    private void RotateLeft()
    {
        getTransform.forward = -getTransform.right;
    }

    private void RotateRight()
    {
        getTransform.forward = getTransform.right;
    }

    private void RotateBack()
    {
        getTransform.forward = -getTransform.forward;
    }

    private void StopSwing()
    {
        _swing = false;
    }

    private void Dash(bool value)
    {
        if (getAnimator.GetBool(DashTag) == !value)
        {
            SetAnimation(DashTag, value);
        }
    }

    private void SetAnimation(string tag)
    {
        SetTrigger(tag);
        if (PhotonNetwork.InRoom == true)
        {
            photonView.RPC("SetTrigger", RpcTarget.Others, tag);
        }
    }

    private void SetAnimation(string tag, bool value)
    {
        SetBool(tag, value);
        if (PhotonNetwork.InRoom == true)
        {
            photonView.RPC("SetBool", RpcTarget.Others, tag, value);
        }
    }

    private void SetAnimation(string tag, float value)
    {
        SetFloat(tag, value);
        if (PhotonNetwork.InRoom == true)
        {
            photonView.RPC("SetFloat", RpcTarget.Others, tag, value);
        }
    }

    [PunRPC]
    private void Equip(int index)
    {
        getPlayerCostume.Equip((ItemName)index);
    }

    [PunRPC]
    private void Rebind()
    {
        getAnimator.Rebind();
    }

    [PunRPC]
    private void SetTrigger(string tag)
    {
        getAnimator.SetTrigger(tag);
    }

    [PunRPC]
    private void SetBool(string tag, bool value)
    {
        getAnimator.SetBool(tag, value);
    }

    [PunRPC]
    private void SetFloat(string tag, float value)
    {
        getAnimator.SetFloat(tag, value);
    }

    [PunRPC]
    private void SetLife(int current, int full)
    {
        _fullLife = Mathf.Clamp(full, MinValue, MaxLife);
        if (_fullLife < current)
        {
            _currentLife = _fullLife;
        }
        else
        {
            _currentLife = current;
        }
        _lifeAction?.Invoke(_currentLife, _fullLife);
    }

    [PunRPC]
    private void SetStamina(float full)
    {
        _fullStamina = Mathf.Clamp(full, MinValue, MaxStamina);
        if (_fullStamina < _currentStamina)
        {
            _currentStamina = _fullStamina;
        }
    }

    private bool IsRunning()
    {
        string name = GetAnimationName();
        return name == RunTag || name == DashTag;
    }

    private string GetAnimationName()
    {
        AnimatorClipInfo[] animatorClipInfos = getAnimator.GetCurrentAnimatorClipInfo(0);
        if (animatorClipInfos.Length > 0)
        {
            AnimationClip animationClip = animatorClipInfos[0].clip;
            if (animationClip != null)
            {
                return animationClip.name;
            }
        }
        return null;
    }

    public void Initialize(Action<int, int> lifeAction)
    {
        _lifeAction = lifeAction;
        _lifeAction?.Invoke(_currentLife, _fullLife);
    }

    public void TakeDamage(Vector3 position, int damage, bool knockback = false)
    {
        if (photonView.IsMine == true && _currentLife > 0)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            getPlayerCostume.SetWeapon(true);
            SoundManager.Instance.PlaySFX(SoundManager.HunterSfxType.HunterHit);
            if (damage < _currentLife)
            {
                _currentLife -= damage;
                Vector3 point = getTransform.position - position;
                Vector2 direction = new Vector2(Vector3.Dot(getTransform.right, point), Vector3.Dot(getTransform.forward, point));
                if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
                {
                    if (direction.y <= 0)
                    {
                        SetAnimation(HitFrontTag);
                    }
                    else
                    {
                        SetAnimation(HitBackTag);
                    }
                }
                else
                {
                    if (direction.x > 0)
                    {
                        SetAnimation(HitLeftTag);
                    }
                    else
                    {
                        SetAnimation(HitRightTag);
                    }
                }
            }
            else
            {
                _currentLife = 0;
                _currentStamina = 0;
                SetAnimation(DeadTag);
            }
            SetLife(_currentLife, _fullLife);
            if (PhotonNetwork.InRoom == true)
            {
                photonView.RPC("SetLife", RpcTarget.Others, _currentLife, _fullLife);
            }
        }
    }

    public void Show(PlayerInteraction.State state)
    {
        getPlayerInteraction.Show(state);
    }

    public void Move(Vector2 direction)
    {
        SetAnimation(HorizontalTag, direction.x);
        SetAnimation(VerticalTag, direction.y);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
        Move(Vector2.zero);
        SetAnimation(AttackTag, false);
        SetAnimation(JumpTag, false);
        _coroutine = null;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_currentStamina);
        }
        else
        {
            _currentStamina = (float)stream.ReceiveNext();
        }
    }

    public void Revive(Vector3 position)
    {
        if (photonView.IsMine == true)
        {
            _currentStamina = _fullStamina;
            _currentLife = _fullLife;
            SetLife(_currentLife, _fullLife);
            if (PhotonNetwork.InRoom == true)
            {
                photonView.RPC("SetLife", RpcTarget.Others, _currentLife, _fullLife);
            }
            Rebind();
            if (PhotonNetwork.InRoom == true)
            {
                photonView.RPC("Rebind", RpcTarget.Others);
            }
            getTransform.position = position;
        }
    }

    public bool TryRecover(int value, int extend = 0)
    {
        if (photonView.IsMine == true && (_currentLife > 0 || _fullLife == _currentLife) && _coroutine == null)
        {
            _coroutine = StartCoroutine(DoRecoverStart());
            IEnumerator DoRecoverStart()
            {
                getPlayerCostume.SetWeapon(false);
                SetAnimation(RecoverTag);
                yield return new WaitForSeconds(RecoverTime);
                getPlayerCostume.SetWeapon(true);
                bool change = false;
                if (extend > 0 && _fullLife + extend <= MaxLife)
                {
                    bool full = _fullLife == _currentLife;
                    _fullLife += extend;
                    if (full == true)
                    {
                        _currentLife = _fullLife;
                    }
                    change = true;
                }
                if (value > 0 && _currentLife + value <= _fullLife)
                {
                    _currentLife += value;
                    change = true;
                }
                if (change == true)
                {
                    SetLife(_currentLife, _fullLife);
                    if (PhotonNetwork.InRoom == true)
                    {
                        photonView.RPC("SetLife", RpcTarget.Others, _currentLife, _fullLife);
                    }
                }
                _coroutine = null;
            }
            return true;
        }
        return false;
    }

    public bool TryRecover(float value)
    {
        if (photonView.IsMine == true && (_currentLife > 0 || _fullLife == _currentLife) && _coroutine == null)
        {
            _coroutine = StartCoroutine(DoRecoverStart());
            IEnumerator DoRecoverStart()
            {
                getPlayerCostume.SetWeapon(false);
                SetAnimation(RecoverTag);
                yield return new WaitForSeconds(RecoverTime);
                getPlayerCostume.SetWeapon(true);
                if (value > 0 && _fullStamina + value <= MaxStamina)
                {
                    _fullStamina += value;
                    SetStamina(_fullStamina);
                    if (PhotonNetwork.InRoom == true)
                    {
                        photonView.RPC("SetStamina", RpcTarget.Others, _fullStamina);
                    }
                }
                _coroutine = null;
            }
            return true;
        }
        return false;
    }
}