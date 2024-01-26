using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator agentAnimator;

    private void SetWalkAnimation(bool val)
    {
        agentAnimator.SetBool("Walk", val);
    }

    private void SetDashAnimation(bool val)
    {
        agentAnimator.SetBool("Dash", val);
    }

    private void SetJumpAnimation(bool val)
    {
        agentAnimator.SetBool("Grounded", val);
    }

    private void SetAttackAnimation(bool val)
    {
        agentAnimator.SetBool("Attack", val);
    }

    private void SetLongAttackAnimation(bool val)
    {
        agentAnimator.SetBool("LongAttack", val);
    }

    private void SetVerticalAttackAnimation(bool val)
    {
        agentAnimator.SetBool("VerticalAttack", val);
    }

    private void SetReflectAnimation(bool val)
    {
        agentAnimator.SetBool("Reflect", val);
    }

    private void SetRipositeAnimation(bool val)
    {
        agentAnimator.SetBool("Riposite", val);
    }

    private void SetDamagedAnimation(bool val)
    {
        agentAnimator.SetBool("Damaged", val);
    }

    private void SetDeadAnimation(bool val)
    {
        agentAnimator.SetBool("Dead", val);
    }

    public void AnimatePlayerWalk(float velocity)
    {
        if (velocity > 0)
        {
            SetWalkAnimation(true);
        }
        else if (velocity < 0)
        {
            SetWalkAnimation(true);
        }
        else
        {
            SetWalkAnimation(false);
        }
    }

    public void AnimatePlayerJump(bool grounded)
    {
        SetJumpAnimation(grounded);
    }

    public void AnimatePlayerDash(bool val)
    {
        SetDashAnimation(val);
    }

    public void AnimatePlayerAttack(bool val)
    {
        SetAttackAnimation(val);
    }

    public void AnimatePlayerLongAttack(bool val)
    {
        SetLongAttackAnimation(val);
    }

    public void AnimatePlayerVerticalAttack(bool val)
    {
        SetVerticalAttackAnimation(val);
    }

    public void AnimatePlayerReflect(bool val)
    {
        SetReflectAnimation(val);
    }

    public void AnimatePlayerRiposite(bool val)
    {
        SetRipositeAnimation(val);
    }

    public void AnimatePlayerDamaged(bool val)
    {
        SetDamagedAnimation(val);
    }

    public void AnimatePlayerDead(bool val)
    {
        SetDeadAnimation(val);
    }

}
