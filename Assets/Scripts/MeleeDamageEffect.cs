using UnityEngine;

public class MeleeDamageEffect : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();   
    }

    private void Update()
    {
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
