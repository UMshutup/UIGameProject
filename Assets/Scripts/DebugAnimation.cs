using UnityEngine;

public class DebugAnimation : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        InvokeRepeating("AltIdle", 1f, 1f);
    }
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("alt_idle"))
        {
            animator.SetTrigger("idle");
        }

        if (Input.GetKeyDown("1"))
        {
            animator.SetTrigger("windup");
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("windup"))
        {
            animator.SetTrigger("attack");
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            animator.SetTrigger("idle");
        }

        if (Input.GetKeyDown("2"))
        {
            animator.SetTrigger("hurt");
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("hurt"))
        {
            animator.SetTrigger("idle");
        }


    }

    private void AltIdle()
    {
        if (Random.Range(0, 10) == 0)
        {
            animator.SetTrigger("alt_idle");
            Debug.Log("played alt_idle");
        }
    }
}
