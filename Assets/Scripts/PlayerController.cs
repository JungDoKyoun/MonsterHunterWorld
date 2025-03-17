using UnityEngine;
using Photon.Pun;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerBody))]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonAnimatorView))]
[RequireComponent(typeof(PhotonRigidbodyView))]

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
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

    private static readonly float GroinDistance = 0.8f;
    private static readonly string HorizontalTag = "Horizontal";
    private static readonly string VerticalTag = "Vertical";
    private static readonly string JumpTag = "Jump";
    private static readonly string DashTag = "Dash";

#if UNITY_EDITOR
    [SerializeField]
    private Color _gizmoColor = Color.red;

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Vector3 position = transform.position;
        Gizmos.DrawRay(new Vector3(position.x, position.y + GroinDistance, position.z), Vector3.down * GroinDistance);
    }
#endif

    private void FixedUpdate()
    {
        if (photonView.IsMine == true)
        {
            Camera camera = Camera.main;
            if (camera != null)
            {
                Vector2 input = new Vector2(_horizontal, _vertical);
                Vector3 direction = camera.transform.forward;
                getPlayerBody.Move(input, direction, _dash);
                getPlayerBody.Roll(input, direction, _jump);
            }
            getPlayerBody.Attack(_attack);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("충돌");
    }

    private void OnCollisionExit(Collision collision)
    {
        //Debug.Log("해제");
    }

    private void Update()
    {
        _attack = Input.GetMouseButton(0);
        _dash = Input.GetButton(DashTag);
        _jump = Input.GetButton(JumpTag);
        _horizontal = Input.GetAxis(HorizontalTag);
        _vertical = Input.GetAxis(VerticalTag);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        _dash = false;
        _horizontal = 0;
        _vertical = 0;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //stream.SendNext(isFiring); //우리 정보도 같이 보내주세요
        }
        else
        {
            //isFiring = (bool)stream.ReceiveNext();  //상대방의 정보는 받아주세요
        }
    }
}