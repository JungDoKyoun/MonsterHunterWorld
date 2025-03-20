using Unity.VisualScripting;
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

    private static readonly string RunClip = "Run";

    public void SetAnimate(string tag, bool value)
    {
        getAnimator.SetBool(tag, value);
    }

    public void SetAnimate(string tag, float value)
    {
        getAnimator.SetFloat(tag, value);
    }

    public bool GetAnimate(string tag)
    {
        return getAnimator.GetBool(tag);
    }

    public bool IsRunning()
    {
        AnimatorClipInfo[] animatorClipInfos = getAnimator.GetCurrentAnimatorClipInfo(0);
        if(animatorClipInfos.Length > 0)
        {
            AnimationClip animationClip = animatorClipInfos[0].clip;
            if(animationClip != null && animationClip.name == RunClip)
            {
                return true;
            }
        }
        return false;
    }
}