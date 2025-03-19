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

    private bool _attack = false;
    private bool _dash = false;
    private bool _jump = false;
    private float _horizontal = 0;
    private float _vertical = 0;
    private Vector3 _forward = Vector3.zero;

    private Coroutine _coroutine = null;

    private static readonly float MinInput = -1;
    private static readonly float MaxInput = 1;
    private static readonly float RotationDamping = 10;
    private static readonly string VerticalTag = "Vertical";
    private static readonly string HorizontalTag = "Horizontal";
    private static readonly string JumpTag = "Jump";
    private static readonly string DashTag = "Dash";

    private void FixedUpdate()
    {
        if (photonView.IsMine == true)
        {
            Camera camera = Camera.main;
            if (camera != null)
            {
                Vector2 input = new Vector2(_horizontal, _vertical);
                Vector3 direction = camera.transform.forward;
                if (input != Vector2.zero)
                {
                    input.x = Mathf.Clamp(input.x, MinInput, MaxInput);
                    input.y = Mathf.Clamp(input.y, MinInput, MaxInput);
                    if (getPlayerBody.IsKeepMoving() == true)
                    {
                        _forward = Quaternion.AngleAxis(Vector2.SignedAngle(input, Vector2.up), Vector3.up) * Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
                    }
                    else if (_coroutine == null)
                    {
                        _forward = Quaternion.AngleAxis(Vector2.SignedAngle(input, Vector2.up), Vector3.up) * Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
                        _coroutine = StartCoroutine(DoMoveStart());
                    }
                }
                else if(_forward != Vector3.zero)
                {
                    if (_coroutine != null)
                    {
                        StopCoroutine(_coroutine);
                        _coroutine = null;
                    }
                    if(getPlayerBody.IsStartMoving() == true)
                    {
                        Debug.Log(_forward);
                        Rotate();
                    }
                    _forward = Vector3.zero;
                    Move();
                }
            }
        }
    }

    private void Update()
    {
        _vertical = Input.GetAxis(VerticalTag);
        _horizontal = Input.GetAxis(HorizontalTag);
        _attack = Input.GetMouseButton(0);
        _dash = Input.GetButton(DashTag);
        _jump = Input.GetButton(JumpTag);
    }

    private void RotateLeft()
    {

    }

    private void RotateRIght()
    {

    }

    private void RotateBack()
    {

    }

    private void Rotate()
    {
        Vector2 direction = new Vector2(Vector3.Dot(getTransform.right, _forward), Vector3.Dot(getTransform.forward, _forward));
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            getTransform.forward = (direction.x > 0) ? getTransform.right : -getTransform.right; // 좌우 방향 결정
        }
        else
        {
            getTransform.forward = (direction.y > 0) ? getTransform.forward : -getTransform.forward; // 전후 방향 결정
        }
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

    private IEnumerator DoMoveStart()
    {
        Move();
        yield return new WaitUntil(predicate: () => getPlayerBody.IsStartMoving());
        yield return new WaitWhile(predicate: () => getPlayerBody.IsStartMoving());
        while (getPlayerBody.IsKeepMoving() == true)
        {
            getTransform.forward = Vector3.Lerp(getTransform.forward, _forward, Time.deltaTime * RotationDamping);
            yield return null;
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        _horizontal = 0;
        _vertical = 0;
        _dash = false;
        StopAllCoroutines();
        _coroutine = null;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}