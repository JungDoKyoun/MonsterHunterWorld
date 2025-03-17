using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]

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

    private static readonly float DashMultiply = 3;
    private static readonly float RotationDamping = 10;
    private static readonly string SpeedTag = "Speed";
    private static readonly string LandingTag = "Landing";
    private static readonly string AttackTag = "Attack";

    public void Move(Vector2 input, Vector3 direction, bool dash)
    {
        if (getAnimator.GetBool(LandingTag) == true && getAnimator.GetBool(AttackTag) == true)
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

    public void Alight(bool landing)
    {
        getAnimator.SetBool(LandingTag, landing);
    }

    public void Attack(bool value)
    {
        if (getAnimator.GetBool(LandingTag) == true)
        {
            getAnimator.SetBool(AttackTag, value);
        }
        else
        {
            getAnimator.SetBool(AttackTag, false);
        }
    }
}