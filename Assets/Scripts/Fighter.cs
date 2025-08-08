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
    [TextArea] public string fighterDescription;

    [Header("Stats")]
    public float maxHP;
    public float meleeDamage;
    public float rangedDamage;
    public float meleeDefence;
    public float rangedDefence;
    public float speed;
    public float accuracy;
    public float evasion;
    public int maxActionPoints;

    [HideInInspector] public float currentHP;
    [HideInInspector] public float previousHP;

    [HideInInspector] public float minHP;

    [HideInInspector] public int currentActionPoints;
    [HideInInspector] public int actionPointRegeneration = 2;

    [Header("State")]
    public FighterState fighterState;

    [Header("Moves")]
    [SerializeField] private List<Move> moves;
    [SerializeField] private Move defaultMove;

    [SerializeField] private MoveInstance defaultMoveInstance;

    [HideInInspector] public List<MoveInstance> currentMoves;

    [HideInInspector] public MoveInstance chosenMove;
    [HideInInspector] public int chosenMoveNumber;

    [HideInInspector] public bool isGoingToBeHit;
     public bool hasChosenMove;

    [Header("ID")]
    public int id;

    [Header("Positions")]
    public GameObject AimPosition;
    public GameObject HitPosition;

    [Header("Positions")]
    public List<StatusEffectInstance> statusEffectInstances;

    // useful
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private AudioManager audioManager;

    //Animation lenghts
    [HideInInspector] public float idleLenght;
    [HideInInspector] public float altIdleLenght;
    [HideInInspector] public float windupLenght;
    [HideInInspector] public float attackLenght;
    [HideInInspector] public float hurtLenght;
    [HideInInspector] public float sleepIdleLenght;

    //Misc variables
    [HideInInspector] public List<Fighter> targets;
    public bool hasChosenTarget;

    private Transform originalTransform;
    public bool hasFinishedAnimation = false;

    public bool isBeingSwapped = false;

    [HideInInspector] public bool hasAppendedAnimation;
    [HideInInspector] public bool hasAppendedDeathAnimation;


    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateAnimClipTimes();

        minHP = -(int)(maxHP / 3);
        currentHP = maxHP;
        previousHP = currentHP;

        originalTransform = transform;

        statusEffectInstances = new List<StatusEffectInstance>();

        InvokeRepeating("PlayAltIdleAnimation", 1, 1);

        MoveSetup();

        chosenMove = defaultMoveInstance;

        fighterState = FighterState.NORMAL;
    }

    private void MoveSetup()
    {
        currentMoves = new List<MoveInstance>();

        for (int i = 0; i< moves.Count; i++)
        {
            currentMoves.Add(new MoveInstance(moves[i]));
        }

        defaultMoveInstance = new MoveInstance(defaultMove);
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
        for (int i = 0; i < currentMoves.Count; i++)
        {
            if (i == _moveNumber)
            {
                chosenMove = currentMoves[i];
                chosenMoveNumber = i;
                hasChosenMove = true;
            }
        }
    }

    public void ChooseMoveNothing()
    {
        chosenMove = defaultMoveInstance;
        chosenMoveNumber = 0;
        hasChosenMove = true;
    }

    public void ChooseMoveRandom()
    {
        List<MoveInstance> usableMoves = new List<MoveInstance>();
        for (int i = 0; i < currentMoves.Count; i++)
        {
            if (currentMoves[i].GetMoveApCost() <= currentActionPoints)
            {
                usableMoves.Add(currentMoves[i]);
            }
        }

        if (usableMoves.Count <= 0)
        {
            chosenMove = defaultMoveInstance;
            hasChosenMove = true;
            return;
        }


        int moveNumber = Random.Range(0, usableMoves.Count);
        for (int i = 0; i < usableMoves.Count; i++)
        {
            if (i == moveNumber)
            {
                chosenMove = usableMoves[moveNumber];
                chosenMoveNumber = i;
                hasChosenMove = true;
            }
        }
    }

    public void ChooseMoveAI(List<Fighter> _currentTeam, List<Fighter> _opposingTeam)
    {
        
    }

    public void ChooseTarget(List<Fighter> _targets)
    {
        targets = _targets;
        hasChosenTarget = true;
    }

    public void ChooseTargetNone()
    {
        targets = null;
        hasChosenTarget = true;
    }

    public void ChooseTargetAI(List<Fighter> _currentTeam, List<Fighter> _opposingTeam)
    {
        if (chosenMove != null)
        {
            if (chosenMove.GetMoveTarget() == MoveTarget.BOTH || chosenMove.GetMoveTarget() == MoveTarget.ENEMY)
            {
                if (chosenMove.GetHitsAWholeSquad())
                {
                    targets = _opposingTeam;
                    hasChosenTarget = true;
                    return;
                }
                else
                {
                    Fighter randomFighter = _opposingTeam[Random.Range(0, _opposingTeam.Count)];

                    

                    targets = new List<Fighter> { randomFighter };
                    hasChosenTarget = true;
                    return;
                }
            }
            else if (chosenMove.GetMoveTarget() == MoveTarget.PLAYER)
            {
                if (chosenMove.GetHitsAWholeSquad())
                {
                    targets = _currentTeam;
                    hasChosenTarget = true;
                    return;
                }
                else
                {
                    targets = new List<Fighter> { _currentTeam[Random.Range(0, _currentTeam.Count)] };
                    hasChosenTarget = true;
                    return;
                }
            }
            else if (chosenMove.GetMoveTarget() == MoveTarget.SELF)
            {
                targets = new List<Fighter> { this };
                hasChosenTarget = true;
                return;
            }
        }
    }

    public void RemoveAP()
    {
        if (currentActionPoints - chosenMove.GetMoveApCost() >= 0)
        {
            currentActionPoints -= chosenMove.GetMoveApCost();
        }
        else
        {
            currentActionPoints = 0;
        }
    }

    public void RegenerateAP()
    {
        if (fighterState == FighterState.NORMAL)
        {
            if (currentActionPoints + actionPointRegeneration <= maxActionPoints)
            {
                currentActionPoints += actionPointRegeneration;
            }
            else
            {
                currentActionPoints = maxActionPoints;
            }
        }
    }

    public void ResetAP()
    {
        currentActionPoints = 0;
    }

    private void Update()
    {
        StatusEffectLogic();

        KnockoutAnimation();
        DeathAnimation();

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
            audioManager.PlaySFX(audioManager.hurt);
            animator.SetTrigger("hurt");
            previousHP = currentHP;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("hurt") && currentHP > 0)
        {
            animator.SetTrigger("idle");
        }

        if (fighterState != FighterState.NORMAL)
        {
            currentMoves = new List<MoveInstance> { defaultMoveInstance };
        }
    }

    public void StatusEffectLogic()
    {
        if (statusEffectInstances != null && statusEffectInstances.Count > 0)
        {
            for (int i = 0; i < statusEffectInstances.Count; i++)
            {
                if (!statusEffectInstances[i].hasAffectedStats)
                {
                    statusEffectInstances[i].ChangeStats(this);
                }

                if (!statusEffectInstances[i].hasRevertedStats && statusEffectInstances[i].statusEffectDuration <= 0)
                {
                    statusEffectInstances[i].RevertStatChange(this);
                    statusEffectInstances.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public void ReduceDurationOfStatusEffects()
    {
        if (statusEffectInstances != null && statusEffectInstances.Count > 0)
        {
            for (int i = 0; i < statusEffectInstances.Count; i++)
            {
                statusEffectInstances[i].ReduceDuration();
            }
        }
    }

    public void UseMove()
    {
        var sequence = DOTween.Sequence();

        if (fighterState == FighterState.NORMAL)
        {

            if (chosenMove.Equals(defaultMoveInstance) || targets == null)
            {
                if (!hasAppendedAnimation)
                {
                    sequence.Append(transform.DOMoveX(originalTransform.position.x + 0.1f, 0.1f)).
                        Append(transform.DOMoveX(originalTransform.position.x - 0.1f, 0.1f)).
                        Append(transform.DOMoveX(originalTransform.position.x, 0.1f)).
                        AppendCallback(() => hasFinishedAnimation = true);

                    hasAppendedAnimation = true;
                }
            }
            else if (chosenMove.GetMoveCategory() == MoveCategory.MELEE)
            {
                if (!hasAppendedAnimation)
                {
                    sequence.AppendCallback(() => RemoveAP()).
                        Append(transform.DOMoveX(GetTargetLocalPosition().x - 0.10f, 0.5f)).
                        Join(transform.DOMoveZ(GetTargetPosition().z, 0.5f)).
                        AppendCallback(() => animator.SetTrigger("windup")).
                        AppendInterval(windupLenght / 1.5f).
                        AppendCallback(() => CheckIfMoveLands()).
                        AppendCallback(() => HitVisualEffectMeelee()).
                        AppendCallback(() => DealDamage()).
                        AppendCallback(() => audioManager.PlaySFX(chosenMove.soundEffect)).
                        AppendInterval(attackLenght).
                        Append(transform.DOMove(originalTransform.position, 0.5f)).
                        AppendCallback(() => hasFinishedAnimation = true);
                    hasAppendedAnimation = true;
                }
            }
            else if (chosenMove.GetMoveCategory() == MoveCategory.RANGED)
            {
                if (!hasAppendedAnimation)
                {
                    sequence.AppendCallback(() => RemoveAP()).
                        Append(transform.DOLocalMove(originalTransform.localPosition - new Vector3(0.25f, 0, 0), 0.5f)).
                        AppendCallback(() => animator.SetTrigger("windup")).
                        AppendInterval(windupLenght).
                        AppendCallback(() => CheckIfMoveLands()).
                        AppendCallback(() => HitVisualEffectRanged()).
                        AppendCallback(() => audioManager.PlaySFX(chosenMove.soundEffect)).
                        AppendInterval(Vector3.Distance(AimPosition.transform.position, GetHitPositionOfTargets()) / chosenMove.GetMoveVisualEffectPrefab().GetComponent<RangedDamageEffect>().projectileSpeed + 0.05f).
                        AppendCallback(() => DealDamage()).
                        AppendInterval(attackLenght).
                        Append(transform.DOLocalMove(originalTransform.localPosition, 0.5f)).
                        AppendCallback(() => hasFinishedAnimation = true);
                    hasAppendedAnimation = true;
                }
            }
        }
        else
        {
            if (!hasAppendedAnimation)
            {
                sequence.Append(transform.DOMoveX(originalTransform.position.x + 0.1f, 0.1f)).
                    Append(transform.DOMoveX(originalTransform.position.x - 0.1f, 0.1f)).
                    Append(transform.DOMoveX(originalTransform.position.x, 0.1f)).
                    AppendCallback(() => hasFinishedAnimation = true);

                hasAppendedAnimation = true;
            }
        }



    }

    public void KnockoutAnimation()
    {
        if (currentHP <= 0)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("hurt"))
            {
                animator.SetTrigger("knockout");
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("knockout"))
            {
                animator.SetTrigger("knockout_idle");
            }
            if (currentHP > minHP)
            {
                fighterState = FighterState.KNOCKOUT;
            }
        }

    }

    public void DeathAnimation()
    {

        var sequence = DOTween.Sequence();

        if (currentHP <= minHP)
        {
            if (!hasAppendedDeathAnimation)
            {
                sequence.Append(transform.DOLocalMoveX(originalTransform.position.x - 1, 0.5f)).
                    Join(GetComponent<SpriteRenderer>().DOFade(0, 0.5f)).
                    AppendCallback(() => GetComponent<SpriteRenderer>().material.color = new Color(1,1,1,0));
                fighterState = FighterState.DEAD;
                hasAppendedDeathAnimation = true;
            }
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

    private void HitVisualEffectMeelee()
    {
        foreach (Fighter fighter in targets)
        {
            chosenMove.ShowMoveVisualEffect(fighter.HitPosition.transform.position, fighter.HitPosition.transform.rotation, fighter.isGoingToBeHit);
        }
    }

    private void HitVisualEffectRanged()
    {
        foreach (Fighter fighter in targets)
        {
            AimPosition.GetComponent<AimToTarget>().Aim(fighter.HitPosition.transform.position);
            chosenMove.ShowMoveVisualEffect(AimPosition.transform.position, AimPosition.transform.rotation, fighter.isGoingToBeHit);
        }
    }

    private void CheckIfMoveLands()
    {
        for (int i = 0; i<targets.Count; i++)
        {
            if (Random.Range(0, 100f) - (this.accuracy - 100) < chosenMove.GetMoveAccuracy() - (targets[i].evasion - 100))
            {
                targets[i].isGoingToBeHit = true;
            }
            else
            {
                targets[i].isGoingToBeHit = false;
            }
        }

    }

    private void DealDamage()
    {
        
        foreach (Fighter target in targets)
        {
            if (target.isGoingToBeHit)
            {
                target.currentHP -= chosenMove.CalculateDamage(this, target);
                chosenMove.GiveStatusEffectToTargets(target);
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
        if (fighterState == FighterState.NORMAL)
        {
            if (Random.Range(0, 8) == 0)
            {
                animator.SetTrigger("alt_idle");
            }
        }

    }

    public void ResetValues()
    {
        hasFinishedAnimation = false;
        hasAppendedAnimation = false;
    }

    public void SwapPosition(Fighter _swap)
    {
        var sequence = DOTween.Sequence();

        Vector3 temp1 = transform.position;
        Vector3 temp2 = _swap.gameObject.transform.position;

        sequence.Append(transform.DOMove(temp2, 0.5f)).
                Join(_swap.gameObject.transform.DOMove(temp1, 0.5f)).
                AppendCallback(() => _swap.originalTransform.position = _swap.transform.position).
                AppendCallback(() => originalTransform.position = transform.position);
    }

    public bool EqualsId(Fighter _fighter)
    {
        return id == _fighter.id;
    }
    
}