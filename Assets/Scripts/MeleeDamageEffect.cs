using UnityEngine;

public class MeleeDamageEffect : MonoBehaviour
{
    [HideInInspector] public Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();   
    }

    private void Update()
    {
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
