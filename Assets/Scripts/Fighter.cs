using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

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
    [HideInInspector] public int chosenMoveNumber;

    [HideInInspector] public bool hasMoveLanded;

    [Header("Positions")]
    public GameObject AimPosition;
    public GameObject HitPosition;

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
<<<<<<< HEAD
    [HideInInspector] public List<Fighter> targets;
    [HideInInspector] public bool hasChosenTarget;
=======
    [HideInInspector] public Fighter target;
>>>>>>> parent of 8a1b8ed (Added target selection and another player character)

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

        chosenMove = moves[0];
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
                chosenMoveNumber = i;
            }
        }
    }

<<<<<<< HEAD
    public void ChooseTarget(List<Fighter> _targets)
    {
        targets = _targets;
        hasChosenTarget = true;
    }

=======
>>>>>>> parent of 8a1b8ed (Added target selection and another player character)
    public void ChooseMoveRandom()
    {
        float moveNumber = Random.Range(0, moves.Length);
        for (int i = 0; i < moves.Length; i++)
        {
            if (i == moveNumber)
            {
                chosenMove = moves[i];
                chosenMoveNumber = i;
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
    }

    public void UseMove()
    {
        var sequence = DOTween.Sequence();

        if (chosenMove.GetMoveCategory() == MoveCategory.MELEE)
        {
            if (!hasAppendedAnimation) {
                sequence.Append(transform.DOMoveX(GetTargetLocalPosition().x - 0.10f, 0.5f)).
                    Join(transform.DOMoveZ(GetTargetPosition().z, 0.5f)).
                    AppendCallback(() => animator.SetTrigger("windup")).
                    AppendInterval(windupLenght / 1.5f).
                    AppendCallback(() => CheckIfMoveLands()).
                    AppendCallback(() => chosenMove.ShowMoveVisualEffect(GetHitPositionOfTargets(), targets[0].HitPosition.transform.rotation, hasMoveLanded)).
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
                    AppendCallback(() => CheckIfMoveLands()).
                    AppendCallback(() => chosenMove.ShowMoveVisualEffect(AimPosition.transform.position, AimPosition.transform.rotation, hasMoveLanded)).
                    AppendInterval(Vector3.Distance(AimPosition.transform.position, GetHitPositionOfTargets()) / chosenMove.GetMoveVisualEffectPrefab().GetComponent<RangedDamageEffect>().projectileSpeed + 0.05f).
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

    private Vector3 GetTargetLocalPosition()
    {
        if (targets.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 meanVector = Vector3.zero;

        foreach (Fighter pos in targets)
        {
            meanVector += pos.transform.localPosition;
        }

        return (meanVector / targets.Count);
    }

    private Vector3 GetTargetPosition()
    {
        if (targets.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 meanVector = Vector3.zero;

        foreach (Fighter pos in targets)
        {
            meanVector += pos.transform.position;
        }

        return (meanVector / targets.Count);
    }

    public Vector3 GetHitPositionOfTargets()
    {
        if (targets.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 meanVector = Vector3.zero;

        foreach (Fighter pos in targets)
        {
            meanVector += pos.HitPosition.transform.position;
        }

        return (meanVector / targets.Count);
    }

    private void CheckIfMoveLands()
    {
        if (Random.Range(0,100f) < chosenMove.GetMoveAccuracy())
        {
            hasMoveLanded = true;
        }
        else
        {
            hasMoveLanded = false;
        }
        
    }

    private void DealDamage()
    {
        if (hasMoveLanded)
        {
            foreach (Fighter target in targets)
            {
                target.currentHP -= chosenMove.CalculateDamage(this, target);
            }
        }
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
