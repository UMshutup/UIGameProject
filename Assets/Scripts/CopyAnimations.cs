using UnityEngine;

public class CopyAnimations : MonoBehaviour
{
    public Animator animator;
    public Animator parentAnimator;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer parentSpriteRenderer;


    void Update()
    {
        spriteRenderer.color = parentSpriteRenderer.material.color;

        if (parentAnimator.GetBool("idle"))
        {
            animator.SetTrigger("idle");
        }
        if (parentAnimator.GetBool("alt_idle"))
        {
            animator.SetTrigger("alt_idle");
        }
        if (parentAnimator.GetBool("windup"))
        {
            animator.SetTrigger("windup");
        }
        if (parentAnimator.GetBool("attack"))
        {
            animator.SetTrigger("attack");
        }
        if (parentAnimator.GetBool("hurt"))
        {
            animator.SetTrigger("hurt");
        }
        if (parentAnimator.GetBool("sleep_idle"))
        {
            animator.SetTrigger("sleep_idle");
        }
        if (parentAnimator.GetBool("sleep_alt_idle"))
        {
            animator.SetTrigger("sleep_alt_idle");
        }
        if (parentAnimator.GetBool("knockout"))
        {
            animator.SetTrigger("knockout");
        }
        if (parentAnimator.GetBool("knockout_idle"))
        {
            animator.SetTrigger("knockout_idle");
        }
    }
}
