using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalWallAnimationController : MonoBehaviour
{
    [SerializeField] private Animator agentAnimator;

    private void SetDamagedAnimation(bool val)
    {
        agentAnimator.SetBool("Damaged", val);
    }

    private void SetDeathAnimation(bool val)
    {
        agentAnimator.SetBool("Death", val);
    }

    public void AnimateDamaged(bool val)
    {
        SetDamagedAnimation(val);
    }

    public void AnimateDeath(bool val)
    {
        SetDeathAnimation(val);
    }
}
