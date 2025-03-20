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

    private static readonly string LandingTag = "Landing";

    private static readonly string RunClip = "Run";
    private static readonly string RollClip = "Roll";
    private static readonly string AttackTag = "Attack";

    public void SetAnimate(string tag, bool value)
    {
        getAnimator.SetBool(tag, value);
    }

    public void SetAnimate(string tag, float value)
    {
        getAnimator.SetFloat(tag, value);
    }
    
    public bool IsRunning()
    {
        return getAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == RunClip;
    }

    public bool IsRolling()
    {
        return getAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == RollClip;
    }

    public bool IsAttacking()
    {
        return GetAnimate(AttackTag);
    }

    public bool GetAnimate(string tag)
    {
        return getAnimator.GetBool(tag);
    }
}