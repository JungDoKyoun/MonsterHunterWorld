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

    private static readonly string HorizontalTag = "Horizontal";
    private static readonly string VerticalTag = "Vertical";
    private static readonly string JumpTag = "Jump";
    private static readonly string DashTag = "Dash";

    private void FixedUpdate()
    {
        if (photonView.IsMine == true)
        {
            getPlayerBody.Attack(_attack);
            Camera camera = Camera.main;
            if (camera != null)
            {
                Vector2 input = new Vector2(_horizontal, _vertical);
                Vector3 direction = camera.transform.forward;
                getPlayerBody.Roll(input, direction, _jump);
                getPlayerBody.Move(input, direction, _dash);
            }
        }
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
            //stream.SendNext(isFiring); //�츮 ������ ���� �����ּ���
        }
        else
        {
            //isFiring = (bool)stream.ReceiveNext();  //������ ������ �޾��ּ���
        }
    }
}