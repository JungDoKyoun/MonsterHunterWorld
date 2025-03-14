using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[DisallowMultipleComponent]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PlayerBody))]

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable, IPunOwnershipCallbacks
{
    private bool _hasPlayerBody = false;

    private PlayerBody _playerBody = null;

    private PlayerBody getPlayerBody
    {
        get
        {
            if(_hasPlayerBody == false)
            {
                _hasPlayerBody = TryGetComponent(out _playerBody);
            }
            return _playerBody;
        }
    }

    private bool dash = false;
    private float horizontal = 0;
    private float vertical = 0;

    private static readonly string HorizontalTag = "Horizontal";
    private static readonly string VerticalTag = "Vertical";
    private static readonly string DashTag = "Dash";

    private void Start()
    {
       // photonView.RequestOwnership();
    }

    private void Update()
    {
        dash = Input.GetButton(DashTag);
        horizontal = Input.GetAxis(HorizontalTag);
        vertical = Input.GetAxis(VerticalTag);
    }

    private void FixedUpdate()
    {
        if(photonView.IsMine == true)
        {
            Camera camera = Camera.main;
            if (camera != null)
            {
                getPlayerBody.Move(new Vector2(horizontal, vertical), camera.transform.forward, dash);
            }
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        dash = false;
        horizontal = 0;
        vertical = 0;
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


    public override void OnPlayerLeftRoom(Player player)
    {
        Debug.Log(photonView.Owner.ActorNumber);
        Debug.Log(player.ActorNumber);
        if (photonView.Owner == player)
        {
        }
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
    }
}