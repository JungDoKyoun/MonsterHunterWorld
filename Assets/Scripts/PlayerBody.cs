using UnityEngine;
using Photon.Pun;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(PhotonRigidbodyView))]
public class PlayerBody : MonoBehaviour
{
    private bool _hasAnimator = false;

    private Animator _animator = null;

    private Animator getAnimator
    {
        get
        {
            if(_hasAnimator == false)
            {
                _hasAnimator = TryGetComponent(out _animator);
            }
            return _animator;
        }
    }

    private bool _hasRigidbody = false;

    private Rigidbody _rigidbody = null;

    private Rigidbody getRigidbody
    {
        get
        {
            if (_hasRigidbody == false)
            {
                _hasRigidbody = TryGetComponent(out _rigidbody);
            }
            return _rigidbody;
        }
    }

    private bool _hasCollider = false;

    private Collider _collider = null;

    private Collider getCollider {
        get
        {
            if(_hasCollider == false)
            {
                _hasCollider = TryGetComponent(out _collider);
            }
            return _collider;
        }
    }

    private bool _isLanding = false;

    private static readonly float DashMultiply = 3;
    private static readonly float RotationDamping = 10;
    private static readonly float GroundDistance = 0.2f;
    private static readonly string SpeedTag = "Speed";
    private static readonly string LandingTag = "Landing";

    private void OnCollisionStay(Collision collision)
    {
        SetLanding(true);
    }

    private void OnCollisionExit(Collision collision)
    {
        SetLanding(false);
    }

    private void SetLanding(bool landing)
    {
        if (_isLanding == !landing)
        {
            Bounds bounds = getCollider.bounds;
            if (Physics.Raycast(new Vector3(bounds.center.x, bounds.min.y + GroundDistance, bounds.center.z), Vector3.down, GroundDistance) == landing)
            {
                _isLanding = landing;
                getAnimator.SetBool(LandingTag, _isLanding);
            }
        }
    }

    public void Move(Vector2 input, Vector3 direction, bool dash)
    {
        if (_isLanding == true)
        {
            if (input != Vector2.zero)
            {
                input.x = Mathf.Clamp(input.x, -1, 1);
                input.y = Mathf.Clamp(input.y, -1, 1);
                Vector3 forward = Quaternion.AngleAxis(Vector2.SignedAngle(input, Vector2.up), Vector3.up) * Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
                Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);
                getRigidbody.MoveRotation(Quaternion.Slerp(getRigidbody.rotation, rotation, Time.deltaTime * RotationDamping));
            }
            float speed = input.magnitude;
            getAnimator.SetFloat(SpeedTag, speed);
            if (dash == true)
            {
                speed *= DashMultiply;
            }
            getRigidbody.velocity = getRigidbody.rotation * Vector3.forward * speed;
        }
    }
}