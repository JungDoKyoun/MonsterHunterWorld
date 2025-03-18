using UnityEngine;
using Photon.Pun;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerBody))]
[RequireComponent(typeof(PhotonView))]

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
    private static readonly float SideDotProduct = Mathf.Sin(Mathf.Deg2Rad * 45);
    private static readonly string HorizontalTag = "Horizontal";
    private static readonly string VerticalTag = "Vertical";
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
                    float dot = Vector3.Dot(getTransform.forward, forward);
                    if(SideDotProduct < dot)
                    {
                        //Debug.Log("À§");
                    }
                    else if(dot < -SideDotProduct)
                    {
                        //Debug.Log("¾Æ·¡");
                    }
                    else
                    {
                        //Debug.Log(Vector3.Dot(getTransform.right, forward));
                    }
                }
            }
        }
    }

    private void Update()
    {
        _horizontal = Input.GetAxis(HorizontalTag);
        _vertical = Input.GetAxis(VerticalTag);
        _attack = Input.GetMouseButton(0);
        _dash = Input.GetButton(DashTag);
        _jump = Input.GetButton(JumpTag);
    }

    [PunRPC]
    private void MoveVertical(bool forward)
    {

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
        //if (stream.IsWriting)
        //{
        //    stream.SendNext(transform.position);
        //    stream.SendNext(transform.rotation);
        //}
        //else if (stream.IsReading && photonView.IsMine == false)
        //{
        //    transform.position = (Vector3)stream.ReceiveNext();
        //    transform.rotation = (Quaternion)stream.ReceiveNext();
        //}
    }
}