using GLTF.Schema;
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

    private static readonly string AttackTag = "Attack";
    private static readonly string LandingTag = "Landing";

    private static readonly string RunClip = "Run";
    private static readonly string RollClip = "Roll";


    public void SetAnimate(string tag, bool value)
    {
        getAnimator.SetBool(tag, value);
    }

    public void SetAnimate(string tag, float value)
    {
        getAnimator.SetFloat(tag, value);
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

    public bool IsRunning()
    {
        return getAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == RunClip;
    }

    public bool IsRolling()
    {
        return getAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == RollClip;
    }

    public bool GetAnimate(string tag)
    {
        return getAnimator.GetBool(tag);
    }
}