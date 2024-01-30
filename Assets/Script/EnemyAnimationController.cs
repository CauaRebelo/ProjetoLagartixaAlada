using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator agentAnimator;

    private void SetAttackAnimation(bool val)
    {
        agentAnimator.SetBool("Attack", val);
    }

    private void SetAttackSpeed(float val)
    {
        agentAnimator.SetFloat("attackSpeed", val);
    }

    private void SetDamagedAnimation(bool val)
    {
        agentAnimator.SetBool("Damaged", val);
    }

    private void SetDeathAnimation(bool val)
    {
        agentAnimator.SetBool("Death", val);
    }

    private void SetTypeAnimation(int val)
    {
        agentAnimator.SetInteger("Type", val);
    }

    public void AnimateAttack(bool val)
    {
        SetAttackAnimation(val);
    }

    public void AttackSpeed(float val)
    {
        SetAttackSpeed(val);
    }

    public void AnimateDamaged(bool val)
    {
        SetDamagedAnimation(val);
    }

    public void AnimateDeath(bool val)
    {
        SetDeathAnimation(val);
    }

    public void AnimateType(int val)
    {
        SetTypeAnimation(val);
    }
}
