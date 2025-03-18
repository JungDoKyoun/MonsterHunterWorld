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

    private Coroutine _coroutine = null;

    private static readonly float MinInput = -1;
    private static readonly float MaxInput = 1;
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
                    Vector3 forward = Quaternion.AngleAxis(Vector2.SignedAngle(input, Vector2.up), Vector3.up) * Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
                    if(getPlayerBody.IsMoving() == true)
                    {
                       // getPlayerBody.MoveRotation(forward);
                    }
                    else if(_coroutine == null)
                    {
                        _coroutine = StartCoroutine(DoMoveStart(forward));
                    }
                }
                else
                {
                    if(_coroutine != null)
                    {
                        StopCoroutine(_coroutine);
                        _coroutine = null;
                    }
                    Move(Vector2.zero);
                    if (PhotonNetwork.InRoom == true)
                    {
                        photonView.RPC("Move", RpcTarget.Others, Vector2.zero);
                    }
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

    [PunRPC]
    private void Move(Vector2 direction)
    {
        getPlayerBody.SetAnimate(HorizontalTag, direction.x);
        getPlayerBody.SetAnimate(VerticalTag, direction.y);
    }

    private IEnumerator DoMoveStart(Vector3 forward)
    {
        Vector2 direction = new Vector2(Vector3.Dot(getTransform.right, forward), Vector3.Dot(getTransform.forward, forward));
        Move(direction);
        if (PhotonNetwork.InRoom == true)
        {
            photonView.RPC("Move", RpcTarget.Others, direction);
        }
        yield return new WaitUntil(predicate: () => getPlayerBody.IsEnd());
        getTransform.rotation = Quaternion.LookRotation(forward, Vector3.up);
        _coroutine = null;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        _horizontal = 0;
        _vertical = 0;
        _dash = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}