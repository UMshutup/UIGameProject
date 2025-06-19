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
    [HideInInspector] public float previousHP;

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

    [HideInInspector] public AimToTarget aimtToTarget;

    private Transform originalTransform;
    [HideInInspector] public bool hasFinishedAnimation = false;

    [HideInInspector] public bool hasAppendedAnimation;


    private void Start()
    {
        animator = GetComponent<Animator>();
        UpdateAnimClipTimes();

        currentHP = maxHP;
        previousHP = currentHP;

        originalTransform = transform;

        InvokeRepeating("PlayAltIdleAnimation", 1, 1);
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
        

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("alt_idle"))
        {
            animator.SetTrigger("idle");
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("windup"))
        {
            animator.SetTrigger("attack");
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
        {
            animator.SetTrigger("idle");
        }

        if (HasTakenDamage())
        {
            animator.SetTrigger("hurt");
            previousHP = currentHP;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("hurt"))
        {
            animator.SetTrigger("idle");
        }


        aimtToTarget = GetComponentInChildren<AimToTarget>();
    }

    public void UseMove()
    {
        var sequence = DOTween.Sequence();

        if (chosenMove.GetMoveCategory() == MoveCategory.MELEE)
        {
            if (!hasAppendedAnimation) {
                sequence.Append(transform.DOMoveX(target.transform.localPosition.x - 0.10f, 0.5f)).
                    Join(transform.DOMoveZ(target.transform.position.z, 0.5f)).
                    AppendCallback(() => animator.SetTrigger("windup")).
                    AppendInterval(windupLenght / 1.5f).
                    AppendCallback(() => chosenMove.ShowMoveVisualEffect(target.transform)).
                    AppendCallback(() => DealDamage()).
                    AppendInterval(attackLenght).
                    Append(transform.DOMove(originalTransform.position, 0.5f)).
                    AppendCallback(() => hasFinishedAnimation = true);
                hasAppendedAnimation = true;
            }
        }else if (chosenMove.GetMoveCategory() == MoveCategory.RANGED)
        {
            if (!hasAppendedAnimation)
            {
                sequence.Append(transform.DOLocalMove(originalTransform.localPosition - new Vector3(0.25f, 0, 0), 0.5f)).
                    AppendCallback(() => animator.SetTrigger("windup")).
                    AppendInterval(windupLenght).
                    AppendCallback(() => chosenMove.ShowMoveVisualEffect(aimtToTarget.transform)).
                    AppendInterval(Vector3.Distance(aimtToTarget.transform.position, target.transform.position) / chosenMove.GetMoveVisualEffectPrefab().GetComponent<RangedDamageEffect>().projectileSpeed + 0.05f).
                    AppendCallback(() => DealDamage()).
                    AppendInterval(attackLenght).
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

    private void DealDamage()
    {
        target.currentHP -= chosenMove.CalculateDamage(this, target);
    }

    private bool HasTakenDamage()
    {
        bool hasTakenDamage;

        hasTakenDamage = previousHP > currentHP;

        return hasTakenDamage;
    }

    public void PlayAltIdleAnimation()
    {
        if (Random.Range(0,8) == 0)
        {
            animator.SetTrigger("alt_idle");
        }
        
    }

    public void ResetValues()
    {
        hasFinishedAnimation = false;
        hasAppendedAnimation = false;

    }
}
