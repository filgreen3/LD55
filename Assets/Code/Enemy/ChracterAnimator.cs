using UnityEngine;

public class ChracterAnimator : IEntityComponent
{
    [SerializeField] protected Animator _animator;

    protected const string _idleClip = "idle";
    protected const string _idleTakeClip = "idle_take";
    protected const string _walkingClip = "walk";
    protected const string _walkingTakeClip = "walk_take";

    public virtual void PlayTake()
    {
        _animator.Play(_idleTakeClip);
    }

    public virtual void PlayIdle()
    {
        _animator.Play(_idleClip);
    }

    public virtual void PlayWalk()
    {
        _animator.Play(_walkingClip);
    }

    public virtual void PlayWalkTake()
    {
        _animator.Play(_walkingTakeClip);
    }
}
