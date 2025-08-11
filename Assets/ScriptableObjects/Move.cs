using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Move", menuName = "Scriptable Objects/Move")]
public class Move : ScriptableObject
{
    [SerializeField] private string moveName;
    [SerializeField] private string moveDescription;
    [SerializeField] private float moveDamage;
    [SerializeField, Range(0f, 100f)] private float moveAccuracy;
    [SerializeField, Range(0, 10)] private int moveApCost;
    [SerializeField] private MoveCategory moveCategory;
    [SerializeField] private MoveTarget moveTarget;
    [SerializeField] private bool hitsAWholeSquad;
    [SerializeField] private StatusEffectSO statusEffectOnHit;
    [SerializeField] private AudioClip soundEffect;

    [Space]
    [SerializeField] private bool HasPreMoveVisualEffect;
    [Space]
    [SerializeField] private GameObject moveVisualEffectPrefab;
    [SerializeField] private GameObject preMoveVisualEffectPrefab;
    [SerializeField] private GameObject moveMissEffectPrefab;

    private bool hasChangedTarget = false;

    public string GetMoveName()
    {
        return moveName;
    }

    public string GetMoveDescription()
    {
        return moveDescription;
    }

    public float GetMoveDamage()
    {
        return moveDamage;
    }
    public float GetMoveAccuracy()
    {
        return moveAccuracy;
    }

    public int GetMoveApCost()
    {
        return moveApCost;
    }

    public MoveCategory GetMoveCategory()
    {
        return moveCategory;
    }

    public MoveTarget GetMoveTarget() 
    { 
        return moveTarget;
    }

    public void SetMoveTarget(MoveTarget _moveTarget)
    {
        this.moveTarget = _moveTarget;
    }

    public bool GetHitsAWholeSquad()
    {
        return hitsAWholeSquad;
    }

    public GameObject GetMoveVisualEffectPrefab()
    {
        return moveVisualEffectPrefab;
    }
    public GameObject GetPreMoveVisualEffectPrefab()
    {
        return preMoveVisualEffectPrefab;
    }

    public GameObject GetMoveMissEffectPrefab()
    {
        return moveMissEffectPrefab;
    }

    public StatusEffectSO GetStatusEffectOnHit()
    {
        return statusEffectOnHit;
    }

    public AudioClip GetSoundEffect()
    {
        return soundEffect;
    }

    public bool GetHasPreMoveVisualEffect()
    {
        return HasPreMoveVisualEffect;
    }

    public void ShowMoveVisualEffect(Vector3 _positionOfUser, Quaternion _rotationOfUser, Vector3 _positionOfTarget, Quaternion _rotationOfTarget, bool _hasMoveLanded)
    {
        var sequence = DOTween.Sequence();

        if (moveVisualEffectPrefab != null)
        {
            if (moveCategory == MoveCategory.MELEE)
            {
                if (_hasMoveLanded)
                {
                    if (HasPreMoveVisualEffect && preMoveVisualEffectPrefab != null)
                    {
                        sequence.AppendCallback(() => Instantiate(preMoveVisualEffectPrefab, _positionOfUser + new Vector3(0, 0, -0.05f), _rotationOfUser)).
                            AppendInterval(0.3f).
                            AppendCallback(() => Instantiate(moveVisualEffectPrefab, _positionOfTarget + new Vector3(0, 0, -0.05f), _rotationOfTarget));
                    }
                    else
                    {
                        Instantiate(moveVisualEffectPrefab, _positionOfTarget + new Vector3(0, 0, -0.05f), _rotationOfTarget);
                    }
                    
                }
                else
                {
                    Instantiate(moveMissEffectPrefab, _positionOfTarget + new Vector3(0, 0, -0.05f), _rotationOfTarget);
                }
            }
            else if (moveCategory == MoveCategory.RANGED)
            {
                if (_hasMoveLanded)
                {

                    if (HasPreMoveVisualEffect && preMoveVisualEffectPrefab != null)
                    {
                        sequence.AppendCallback(() => Instantiate(preMoveVisualEffectPrefab, _positionOfUser + new Vector3(0, 0, -0.05f), _rotationOfTarget)).
                            AppendInterval(0.3f).
                            AppendCallback(() => Instantiate(moveVisualEffectPrefab, _positionOfUser, _rotationOfUser));
                    }
                    else
                    {
                        Instantiate(moveVisualEffectPrefab, _positionOfUser, _rotationOfUser); ;
                    }
                    
                }
                else
                {
                    Instantiate(moveMissEffectPrefab, _positionOfUser + new Vector3(0, 0, -0.05f), _rotationOfUser);
                    Instantiate(moveVisualEffectPrefab, _positionOfUser, Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)));
                }

            }
        }
    }

    public void SetOppositeMoveTarget()
    {
        if (!hasChangedTarget)
        {
            if (GetMoveTarget() == MoveTarget.PLAYER)
            {
                SetMoveTarget(MoveTarget.ENEMY);
                hasChangedTarget = true;
            }
            if (GetMoveTarget() == MoveTarget.ENEMY)
            {
                SetMoveTarget(MoveTarget.PLAYER);
                hasChangedTarget = true;
            }
        }

    }

}