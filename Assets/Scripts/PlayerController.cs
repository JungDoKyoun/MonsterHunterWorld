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

    private Vector3 _forward = Vector3.zero;

    private Coroutine _coroutine = null;

    private static readonly float MinInput = -1;
    private static readonly float MaxInput = 1;
    private static readonly float RotationDamping = 10;
    private static readonly string VerticalTag = "Vertical";
    private static readonly string HorizontalTag = "Horizontal";
    private static readonly string JumpTag = "Jump";
    private static readonly string AttackTag = "Attack";
    private static readonly string RunClip = "Run";

    private void Update()
    {
        if (photonView.IsMine == true)
        {
            bool attack = Input.GetMouseButton(0);
            if (attack != getAnimator.GetBool(AttackTag))
            {
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
                                        Move();
                                        Vector3 forward = _forward;
                                        while (IsRunning() == false)
                                        {
                                            Vector2 direction = new Vector2(Vector3.Dot(new Vector3(forward.z, forward.y, -forward.x), _forward), Vector3.Dot(forward, _forward));
                                            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                                            {
                                                Move(direction);
                                                if (PhotonNetwork.InRoom == true)
                                                {
                                                    photonView.RPC("Move", RpcTarget.Others, direction);
                                                }
                                                _coroutine = null;
                                                yield break;
                                            }
                                            yield return null;
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
                            //Debug.Log(new Vector2(Vector3.Dot(getTransform.right, _forward), Vector3.Dot(getTransform.forward, _forward)));
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