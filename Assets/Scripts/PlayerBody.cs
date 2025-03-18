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

    private static readonly float MinInput = -1;
    private static readonly float MaxInput = 1;
    private static readonly float RotationDamping = 10;
    private static readonly string RollTag = "Roll";
    private static readonly string SpeedTag = "Speed";
    private static readonly string AttackTag = "Attack";
    private static readonly string LandingTag = "Landing";

    public void SetAnimate(string tag, bool value)
    {
        getAnimator.SetBool(tag, value);
    }

    public void SetAnimate(string tag, float value)
    {
        getAnimator.SetFloat(tag, value);
    }

    public void Move(Vector2 input, Vector3 direction, bool dash)
    {
        if(getAnimator.GetBool(LandingTag) == true && getAnimator.GetBool(AttackTag) == false && getAnimator.GetBool(RollTag) == false)
        {
            string name = getAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            if (name.Contains(AttackTag) == false && name.Equals(RollTag) == false)
            {
                if (input != Vector2.zero)
                {
                    input.x = Mathf.Clamp(input.x, MinInput, MaxInput);
                    input.y = Mathf.Clamp(input.y, MinInput, MaxInput);
                    Vector3 forward = Quaternion.AngleAxis(Vector2.SignedAngle(input, Vector2.up), Vector3.up) * Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
                    Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);
                    getRigidbody.MoveRotation(Quaternion.Slerp(getRigidbody.rotation, rotation, Time.deltaTime * RotationDamping));
                }
                float speed = Mathf.Clamp01(input.magnitude);
                //if (dash == true)
                //{
                //    speed *= DashMultiply;
                //}
                getAnimator.SetFloat(SpeedTag, speed);
            }
        }
    }

    public void Roll(Vector2 input, Vector3 direction, bool jump)
    {
        if(jump == false)
        {
            getAnimator.SetBool(RollTag, false);
        }
        else if (getAnimator.GetBool(LandingTag) == true)
        {
            string name = getAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            if (name.Equals(RollTag) == false)
            {
                if (input != Vector2.zero)
                {
                    input.x = Mathf.Clamp(input.x, MinInput, MaxInput);
                    input.y = Mathf.Clamp(input.y, MinInput, MaxInput);
                    Vector3 forward = Quaternion.AngleAxis(Vector2.SignedAngle(input, Vector2.up), Vector3.up) * Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
                    getRigidbody.MoveRotation(Quaternion.LookRotation(forward, Vector3.up));
                }
                else
                {
                    //getRigidbody.MoveRotation(Quaternion.LookRotation(getTransform.forward, Vector3.up));
                }
                getAnimator.SetBool(RollTag, true);
            }
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