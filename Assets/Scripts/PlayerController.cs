using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

[DisallowMultipleComponent]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PlayerBody))]

public class PlayerController : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks, IPunObservable
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

    private bool _dash = false;
    private float _horizontal = 0;
    private float _vertical = 0;

    public int ActorNumber
    {
        get
        {
            return photonView.OwnerActorNr;
        }
        set
        {
            photonView.TransferOwnership(value);
        }
    }
    
    private static readonly string HorizontalTag = "Horizontal";
    private static readonly string VerticalTag = "Vertical";
    private static readonly string DashTag = "Dash";

    private void Update()
    {
        _dash = Input.GetButton(DashTag);
        _horizontal = Input.GetAxis(HorizontalTag);
        _vertical = Input.GetAxis(VerticalTag);
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine == true)
        {
            Camera camera = Camera.main;
            if (camera != null)
            {
                getPlayerBody.Move(new Vector2(_horizontal, _vertical), camera.transform.forward, _dash);
            }
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        _dash = false;
        _horizontal = 0;
        _vertical = 0;
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        if (photonView.AmOwner == true)
        {
            FindAnyObjectByType<CinemachineFreeLook>().Set(transform);
        }
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
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