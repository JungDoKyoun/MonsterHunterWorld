using System.Collections;
using UnityEngine;
using Photon.Pun;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerBody))]
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

    private bool _hasPlayerBody = false;

    private PlayerBody _playerBody = null;

    private PlayerBody getPlayerBody
    {
        get
        {
            if (_hasPlayerBody == false)
            {
                _hasPlayerBody = TryGetComponent(out _playerBody);
            }
            return _playerBody;
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

    private void Update()
    {
        if (photonView.IsMine == true)
        {
            bool attack = Input.GetMouseButton(0);
            if (attack != getPlayerBody.GetAnimate(AttackTag))
            {
                Attack(attack);
                if (PhotonNetwork.InRoom == true)
                {
                    photonView.RPC("Attack", RpcTarget.Others, attack);
                }
            }
            //if(getPlayerBody.GetAnimate(AttackTag) == false && getPlayerBody.IsAttacking() == false)
            {
                float vertical = Input.GetAxis(VerticalTag);
                float horizontal = Input.GetAxis(HorizontalTag);
                bool jump = Input.GetButton(JumpTag);
                Camera camera = Camera.main;
                if (camera != null)
                {
                    Vector2 input = new Vector2(Mathf.Clamp(horizontal, MinInput, MaxInput), Mathf.Clamp(vertical, MinInput, MaxInput));
                    Vector3 forward = camera.transform.forward;
                    if (getPlayerBody.GetAnimate(JumpTag) == true)
                    {
                        Jump(false);
                        if (PhotonNetwork.InRoom == true)
                        {
                            photonView.RPC("Jump", RpcTarget.Others, false);
                        }
                    }
                    else if (jump == true)
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
                        Move();
                        Jump(true);
                        if (PhotonNetwork.InRoom == true)
                        {
                            photonView.RPC("Jump", RpcTarget.Others, true);
                        }
                    }
                    else
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
                                    yield return new WaitUntil(() => getPlayerBody.IsRunning());
                                    while (getPlayerBody.IsRunning() == true)
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
                }
            }
        }
    }

    private void RotateLeft()
    {
        getTransform.forward = -getTransform.right;
    }

    private void RotateRIght()
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
        getPlayerBody.SetAnimate(HorizontalTag, direction.x);
        getPlayerBody.SetAnimate(VerticalTag, direction.y);
    }

    [PunRPC]
    private void Jump(bool value)
    {
        getPlayerBody.SetAnimate(JumpTag, value);
    }

    [PunRPC]
    private void Attack(bool value)
    {
        getPlayerBody.SetAnimate(AttackTag, value);
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