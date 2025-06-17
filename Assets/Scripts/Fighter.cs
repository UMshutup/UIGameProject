using DG.Tweening;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [Header("Basic info")]
    public string fighterName;

    [Header("Stats")]
    public float maxHP;
    public float meleeDamage;
    public float rangedDamage;
    public float meleeDefence;
    public float rangedDefence;
    public float speed;

    [HideInInspector] public float currentHP;

    [Header("Moves")]
    public Move[] moves;

    [HideInInspector] public Move chosenMove;

    [Header("Animator")]
    private Animator animator;
    [HideInInspector] public bool dealDamage;


    [HideInInspector] public Fighter target;

    private Transform originalTransform;
    [HideInInspector] public bool hasChosenMove = false;
    [HideInInspector] public bool hasFinishedAnimation;

    private bool hasAppendedAnimation;


    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHP = maxHP;
        originalTransform = transform;
    }

    public void ChooseMove(int _moveNumber)
    {
        for (int i = 0; i < moves.Length; i++) {
            if (i == _moveNumber)
            {
                chosenMove = moves[i];
                hasChosenMove = true;
            }
        }
    }

    public void ChooseMoveRandom()
    {
        float moveNumber = Random.Range(0, moves.Length);
        for (int i = 0; i < moves.Length; i++)
        {
            if (i == moveNumber)
            {
                chosenMove = moves[i];
                hasChosenMove = true;
            }
        }
    }

    private void Update()
    {
        if (hasChosenMove)
        {
            UseMove();
            hasAppendedAnimation = false;
        }
    }

    public void UseMove()
    {
        var sequence = DOTween.Sequence();
        
        if (chosenMove.GetMoveCategory() == MoveCategory.MELEE)
        {
            if (!hasAppendedAnimation) {
                hasFinishedAnimation = false;
                sequence.Append(transform.DOLocalMove(target.transform.localPosition, 0.5f)).
                    AppendCallback(() => animator.SetTrigger("windup")).
                    AppendCallback(() => MoveAnimation());
                hasAppendedAnimation = true;
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {
                hasAppendedAnimation = false;
                if (!hasAppendedAnimation) {
                    sequence.Append(transform.DOLocalMove(originalTransform.localPosition, 0.5f)).
                        AppendCallback(() => hasFinishedAnimation = true);
                    hasAppendedAnimation = true;
                }
            }
        }else if (chosenMove.GetMoveCategory() == MoveCategory.RANGED)
        {
            if (!hasAppendedAnimation)
            {
                hasFinishedAnimation = false;
                sequence.Append(transform.DOLocalMove(originalTransform.localPosition - new Vector3(1, 0, 0), 0.5f)).
                    AppendCallback(() => animator.SetTrigger("windup")).
                    AppendCallback(() => MoveAnimation());
                hasAppendedAnimation = true;
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {
                hasAppendedAnimation = false;
                if (!hasAppendedAnimation)
                {
                    sequence.Append(transform.DOLocalMove(originalTransform.localPosition, 0.5f)).
                        AppendCallback(() => hasFinishedAnimation = true);
                    hasAppendedAnimation = true;
                }
            }
        }
        else
        {
            Debug.Log("Error: no animation for move category");
        }
    }

    public void MoveAnimation()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("windup"))
        {
            dealDamage = true;
            dealDamage = false;
            animator.SetTrigger("attack");
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            animator.SetTrigger("idle");
        }
    }
}
