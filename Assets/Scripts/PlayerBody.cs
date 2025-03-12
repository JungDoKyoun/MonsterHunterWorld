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

    private static readonly string SpeedTag = "Speed";
    private static readonly string DirectionTag = "Direction";
       
    public void Move(Vector2 input, Vector3 direction, bool dash)
    {
        if (input != Vector2.zero)
        {
            float angleOffset = Vector2.SignedAngle(input, Vector2.up);
            Vector3 forward = Quaternion.AngleAxis(angleOffset, Vector3.up) * Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
            Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);
            getRigidbody.MoveRotation(rotation);
        }
    }
}