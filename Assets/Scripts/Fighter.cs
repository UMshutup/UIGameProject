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

    //Animation lenghts
    [HideInInspector] public float idleLenght;
    [HideInInspector] public float altIdleLenght;
    [HideInInspector] public float windupLenght;
    [HideInInspector] public float attackLenght;
    [HideInInspector] public float hurtLenght;
    [HideInInspector] public float sleepIdleLenght;

    //Misc variables
    [HideInInspector] public Fighter target;

    private Transform originalTransform;
    [HideInInspector] public bool hasFinishedAnimation = false;

    [HideInInspector] public bool hasAppendedAnimation;


    private void Start()
    {
        animator = GetComponent<Animator>();
        UpdateAnimClipTimes();
        currentHP = maxHP;
        originalTransform = transform;
    }

    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "idle":
                    idleLenght = clip.length;
                    break;
                case "alt_idle":
                    altIdleLenght = clip.length;
                    break;
                case "windup":
                    windupLenght = clip.length;
                    break;
                case "attack":
                    attackLenght = clip.length;
                    break;
                case "hurt":
                    hurtLenght = clip.length;
                    break;
                case "sleep_idle":
                    sleepIdleLenght = clip.length;
                    break;
            }
        }
    }

    public void ChooseMove(int _moveNumber)
    {
        for (int i = 0; i < moves.Length; i++) {
            if (i == _moveNumber)
            {
                chosenMove = moves[i];
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
            }
        }
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("windup"))
        {
            animator.SetTrigger("attack");
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            animator.SetTrigger("idle");
        }


    }

    public void UseMove()
    {
        var sequence = DOTween.Sequence();
        
        if (chosenMove.GetMoveCategory() == MoveCategory.MELEE)
        {
            if (!hasAppendedAnimation) {
                sequence.Append(transform.DOMove(target.transform.position, 0.5f)).
                    AppendCallback(() => animator.SetTrigger("windup")).
                    AppendInterval(windupLenght + attackLenght).
                    Append(transform.DOMove(originalTransform.position, 0.5f)).
                    AppendCallback(() => hasFinishedAnimation = true);
                hasAppendedAnimation = true;
            }
        }else if (chosenMove.GetMoveCategory() == MoveCategory.RANGED)
        {
            if (!hasAppendedAnimation)
            {
                sequence.Append(transform.DOLocalMove(originalTransform.localPosition - new Vector3(1, 0, 0), 0.5f)).
                    AppendCallback(() => animator.SetTrigger("windup")).
                    AppendInterval(windupLenght + attackLenght).
                    Append(transform.DOLocalMove(originalTransform.localPosition, 0.5f)).
                    AppendCallback(() => hasFinishedAnimation = true);
                hasAppendedAnimation = true;
            }
        }
        else
        {
            Debug.Log("Error: no animation for move category");
        }
        
        

    }

    public void MoveAnimation()
    {
        
        
    }
}
