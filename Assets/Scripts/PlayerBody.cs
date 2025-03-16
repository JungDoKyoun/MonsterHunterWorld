using UnityEngine;
using Photon.Pun;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(PhotonRigidbodyView))]
public class PlayerBody : MonoBehaviour
{
    private bool _hasTransform = false;

    private Transform _transform = null;

    private Transform getTransform {
        get
        {
            if(_hasTransform == false)
            {
                _transform = transform;
                _hasTransform = true;
            }
            return _transform;
        }
    }

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
            Vector3 position = getTransform.position;
            if (Physics.Raycast(new Vector3(position.x, position.y + GroundDistance, position.z), Vector3.down, GroundDistance) == landing)
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
            float speed = Mathf.Clamp01(input.magnitude);
            if (dash == true)
            {
                speed *= DashMultiply;
            }
            getAnimator.SetFloat(SpeedTag, speed);
            getRigidbody.velocity = getRigidbody.rotation * Vector3.forward * speed;
        }
    }

    public void Attack()
    {

    }
}