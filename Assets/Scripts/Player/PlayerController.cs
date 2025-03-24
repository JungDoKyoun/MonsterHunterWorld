using System;
using System.Collections;
using UnityEngine;
using Photon.Pun;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]
[RequireComponent(typeof(PlayerCostume))]

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

    private Animator getAnimator {
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
            if(_hasPlayerCostume == false)
            {
                _hasPlayerCostume = TryGetComponent(out _playerCostume);
            }
            return _playerCostume;
        }
    }

    private Coroutine _coroutine = null;

    [Header("최대 체력"), SerializeField]
    private uint _fullLife;

    public uint fullLife
    {
        get
        {
            return _fullLife;
        }
        set
        {
            _fullLife = value;
            if (_fullLife < _currentLife)
            {
                _currentLife = _fullLife;
            }
        }
    }

    [Header("현재 체력"), SerializeField]
    private uint _currentLife;

    public uint currentLife
    {
        get
        {
            return _currentLife;
        }
        set
        {
            _currentLife = value;
            if (_fullLife < _currentLife)
            {
                _currentLife = _fullLife;
            }
        }
    }

    private Action<uint, uint> _lifeAction = null;

    [Header("최대 스태미너"), SerializeField, Range(MinStamina, MaxStamina)]
    private float _fullStamina;

    public float fullStamina
    {
        get
        {
            return _fullStamina;
        }
        set
        {
            _fullStamina = Mathf.Clamp(value, MinStamina, MaxStamina);
            if (_fullStamina < _currentStamina)
            {
                _currentStamina = _fullStamina;
            }
        }
    }

    [Header("현재 스태미너"), SerializeField, Range(MinStamina, MaxStamina)]
    private float _currentStamina;

    public float currentStamina
    {
        get
        {
            return _currentStamina;
        }
        set
        {
            _currentStamina = Mathf.Clamp(value, MinStamina, MaxStamina);
            if (_fullStamina < _currentStamina)
            {
                _currentStamina = _fullStamina;
            }
        }
    }

    [SerializeField]
    private uint _damage = 0;

    private bool _swing = false;

    private const float MinStamina = 0;
    private const float MaxStamina = int.MaxValue;
    private const string IdleToLeftTag = "IdleToLeft";
    private const string IdleToRightTag = "IdleToRight";
    private const string IdleToBackTag = "IdleToBack";
    private static readonly float MinInput = -1;
    private static readonly float MaxInput = 1;
    private static readonly float RotationDamping = 10;
    private static readonly float JumpStamina = 0.2f;
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
    private static readonly string DeadTag = "Dead";

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

    private void Update()
    {
        if (photonView.IsMine == true && (_currentLife > 0 || _fullLife == _currentLife))
        {
            bool attack = Input.GetMouseButton(0);
            if (attack != getAnimator.GetBool(AttackTag))
            {
                SetAnimation(AttackTag, attack);
                if (attack == true)
                {
                    Rotate();
                    _swing = true;
                }
            }
            else if(attack == false)
            {
                if(Input.GetButton(JumpTag) == false)
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
                            Vector3 forward = Quaternion.AngleAxis(Vector2.SignedAngle(input, Vector2.up), Vector3.up) * Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up).normalized;
                            if (_coroutine == null)
                            {
                                _coroutine = StartCoroutine(DoMoveStart());
                                IEnumerator DoMoveStart()
                                {
                                    Move(forward);
                                    while (IsRunning() == false)
                                    {
                                        //Vector2 direction = new Vector2(Vector3.Dot(new Vector3(forward.z, forward.y, -forward.x), _forward), Vector3.Dot(forward, _forward));
                                        //if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                                        //{
                                        //    Move(Vector2.zero);
                                        //    if (PhotonNetwork.InRoom == true)
                                        //    {
                                        //        photonView.RPC("Move", RpcTarget.Others, Vector2.zero);
                                        //    }
                                        //    _coroutine = null;
                                        //    yield break;
                                        //}
                                        yield return null;
                                    }
                                    while (IsRunning() == true)
                                    {
                                        if (Input.GetButton(DashTag) == true)
                                        {
                                            if (_currentStamina > 0)
                                            {
                                                _currentStamina -= Time.deltaTime;
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
                                        getTransform.forward = Vector3.Lerp(getTransform.forward, forward, Time.deltaTime * RotationDamping);
                                        yield return null;
                                    }
                                    _coroutine = null;
                                }
                            }
                        }
                        else if (getAnimator.GetFloat(HorizontalTag) != 0|| getAnimator.GetFloat(VerticalTag) != 0)
                        {
                            if (_coroutine != null)
                            {
                                StopCoroutine(_coroutine);
                                _coroutine = null;
                            }
                            Move(Vector3.zero);
                        }
                    }
                }
                else if (getAnimator.GetBool(JumpTag) == false)
                {
                    SetAnimation(JumpTag, true);
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
            else if(other.tag == "MonsterHead")
            {
                Transform transform = other.transform.parent;
                while(transform.parent != null)
                {
                    transform = transform.parent;
                    if(transform.TryGetComponent(out monsterController))
                    {
                        monsterController.TakeHeadDamage(_damage);
                        StopSwing();
                        return;
                    }
                }
            }
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
            if (PhotonNetwork.InRoom == true)
            {
                photonView.RPC("Dash", RpcTarget.Others, value);
            }
        }
    }

    private void Move(Vector3 forward)
    {
        Vector2 direction = new Vector2(Vector3.Dot(getTransform.right, forward), Vector3.Dot(getTransform.forward, forward));
        Move(direction);
        if (PhotonNetwork.InRoom == true)
        {
            photonView.RPC("Move", RpcTarget.Others, direction);
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

    public void Initialize(Action<uint, uint> lifeAction)
    {
        _lifeAction = lifeAction;
    }

    public void TakeDamage(Vector3 position, uint damage)
    {
        if(photonView.IsMine == true && _currentLife > 0)
        {
            if(damage < _currentLife)
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
                    if(direction.x > 0)
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
                SetAnimation(DeadTag);
            }
            _lifeAction?.Invoke(_currentLife, _fullLife);
        }
    }

    public void Heal(uint value)
    {
    }

    [PunRPC]
    public void Move(Vector2 direction)
    {
        getAnimator.SetFloat(HorizontalTag, direction.x);
        getAnimator.SetFloat(VerticalTag, direction.y);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
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
}