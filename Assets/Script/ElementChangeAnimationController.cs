using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementChangeAnimationController : MonoBehaviour
{
    [SerializeField] private Animator agentAnimator;

    private void SetEnchantmentAnimation(int val)
    {
        agentAnimator.SetInteger("Enchantment", val);
    }

    public void AnimateEnchantment(int val)
    {
        SetEnchantmentAnimation(val);
    }
}
