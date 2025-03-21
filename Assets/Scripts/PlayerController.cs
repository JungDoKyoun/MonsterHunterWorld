using System.Collections;
using UnityEngine;
using Photon.Pun;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonTransformView))]

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
    private Coroutine _coroutine = null;

    private Vector3 _forward = Vector3.zero;

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

    private const float MinStamina = 0;
    private const float MaxStamina = int.MaxValue;
    private static readonly float MinInput = -1;
    private static readonly float MaxInput = 1;
    private static readonly float RotationDamping = 10;
    private static readonly string VerticalTag = "Vertical";
    private static readonly string HorizontalTag = "Horizontal";
    private static readonly string JumpTag = "Jump";
    private static readonly string AttackTag = "Attack";
    private static readonly string RunClip = "Run";

#if UNITY_EDITOR
    private void OnValidate()
    {
        
    }
#endif

    private void Update()
    {
        if (photonView.IsMine == true && (_currentLife > 0 || _fullLife == _currentLife))
        {
            bool attack = Input.GetMouseButton(0);
            if (attack != getAnimator.GetBool(AttackTag))
            {
#if UNITY_EDITOR
                Debug.Log("공격:" + attack);
#endif
                Attack(attack);
                if (PhotonNetwork.InRoom == true)
                {
                    photonView.RPC("Attack", RpcTarget.Others, attack);
                }
                if(attack == true)
                {

                }
            }
            else
            {
                bool jump = Input.GetButton(JumpTag);
                if (jump == false && getAnimator.GetBool(JumpTag) == true)
                {
#if UNITY_EDITOR
                    Debug.Log("점프 해제");
#endif
                    Jump(false);
                    if (PhotonNetwork.InRoom == true)
                    {
                        photonView.RPC("Jump", RpcTarget.Others, false);
                    }
                }
                else
                {
                    float vertical = Input.GetAxis(VerticalTag);
                    float horizontal = Input.GetAxis(HorizontalTag);
                    Camera camera = Camera.main;
                    if (camera != null)
                    {
                        Vector2 input = new Vector2(Mathf.Clamp(horizontal, MinInput, MaxInput), Mathf.Clamp(vertical, MinInput, MaxInput));
                        Vector3 forward = camera.transform.forward;
                        if(jump == false)
                        {
                            if (input != Vector2.zero)
                            {
                                _forward = Quaternion.AngleAxis(Vector2.SignedAngle(input, Vector2.up), Vector3.up) * Vector3.ProjectOnPlane(forward, Vector3.up).normalized;
                                if (_coroutine == null)
                                {
                                    _coroutine = StartCoroutine(DoMoveStart());
                                    IEnumerator DoMoveStart()
                                    {
#if UNITY_EDITOR
                                        Debug.Log("이동 시작");
#endif
                                        Move();
                                        Vector3 forward = _forward;
                                        while (IsRunning() == false)
                                        {
                                            yield return null;
                                            Vector2 a = new Vector2(Vector3.Dot(new Vector3(forward.z, forward.y, -forward.x), _forward), Vector3.Dot(forward, _forward));
                                            Vector2 b = new Vector2(Vector3.Dot(getTransform.right, _forward), Vector3.Dot(getTransform.forward, _forward));
                                            if (Mathf.Abs(a.x) > Mathf.Abs(a.y) && (Mathf.Sign(a.x) != Mathf.Sign(b.x) || Mathf.Sign(a.y) != Mathf.Sign(b.y)))
                                            {
                                                Debug.Log(getAnimator.GetFloat(HorizontalTag) + " " + getAnimator.GetFloat(VerticalTag));
                                                //Debug.Log(a + " " + b);
                                                //Move(a);
                                                //if (PhotonNetwork.InRoom == true)
                                                //{
                                                //    photonView.RPC("Move", RpcTarget.Others, a);
                                                //}
                                                forward = _forward;
                                            }
                                        }
                                        while (IsRunning() == true)
                                        {
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
#if UNITY_EDITOR
                                Debug.Log("이동 멈춤");
#endif
                                _forward = Vector3.zero;
                                Move();
                            }
                        }
                        else if (getAnimator.GetBool(JumpTag) == false)
                        {
                            if (_coroutine != null)
                            {
                                StopCoroutine(_coroutine);
                                _coroutine = null;
                            }
                            if (input != Vector2.zero)
                            {
                                _forward = Quaternion.AngleAxis(Vector2.SignedAngle(input, Vector2.up), Vector3.up) * Vector3.ProjectOnPlane(forward, Vector3.up).normalized;
                            }
                            else
                            {
                                _forward = getTransform.forward;
                            }
#if UNITY_EDITOR
                            Debug.Log("구르기");
#endif
                            Move();
                            Jump(true);
                            if (PhotonNetwork.InRoom == true)
                            {
                                photonView.RPC("Jump", RpcTarget.Others, true);
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

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

    private void Move()
    {
        Vector2 direction = new Vector2(Vector3.Dot(getTransform.right, _forward), Vector3.Dot(getTransform.forward, _forward));
        Move(direction);
        if (PhotonNetwork.InRoom == true)
        {
            photonView.RPC("Move", RpcTarget.Others, direction);
        }
    }

    [PunRPC]
    private void Move(Vector2 direction)
    {
        getAnimator.SetFloat(HorizontalTag, direction.x);
        getAnimator.SetFloat(VerticalTag, direction.y);
    }

    [PunRPC]
    private void Jump(bool value)
    {
        getAnimator.SetBool(JumpTag, value);
    }

    [PunRPC]
    private void Attack(bool value)
    {
        getAnimator.SetBool(AttackTag, value);
    }

    private bool IsRunning()
    {
        AnimatorClipInfo[] animatorClipInfos = getAnimator.GetCurrentAnimatorClipInfo(0);
        if (animatorClipInfos.Length > 0)
        {
            AnimationClip animationClip = animatorClipInfos[0].clip;
            if (animationClip != null && animationClip.name == RunClip)
            {
                return true;
            }
        }
        return false;
    }

    public void TakeDamage(Vector3 position, uint damage)
    {

    }

    public void Heal(uint value)
    {

    }

    public override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
        _coroutine = null;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}