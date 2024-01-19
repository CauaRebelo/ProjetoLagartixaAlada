using UnityEngine;

public class BonfireAnimationController : MonoBehaviour
{
    [SerializeField] private Animator agentAnimator;

    private void SetLitAnimation(bool val)
    {
        agentAnimator.SetBool("Activated", val);
    }

    public void AnimateLit(bool val)
    {
        SetLitAnimation(val);
    }
}
