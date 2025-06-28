using UnityEngine;

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

    [Space]
    [SerializeField] private GameObject moveVisualEffectPrefab;
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

    public GameObject GetMoveMissEffectPrefab()
    {
        return moveMissEffectPrefab;
    }

    public void ShowMoveVisualEffect(Vector3 _position, Quaternion _rotation, bool _hasMoveLanded)
    {
        if (moveVisualEffectPrefab != null)
        {
            if (moveCategory == MoveCategory.MELEE)
            {
                if (_hasMoveLanded)
                {
                    Instantiate(moveVisualEffectPrefab, _position + new Vector3(0, 0, -0.05f), _rotation);
                }
                else
                {
                    Instantiate(moveMissEffectPrefab, _position + new Vector3(0, 0, -0.05f), _rotation);
                }
            }
            else if (moveCategory == MoveCategory.RANGED)
            {
                if (_hasMoveLanded)
                {
                    Instantiate(moveVisualEffectPrefab, _position, _rotation);
                }
                else
                {
                    Instantiate(moveMissEffectPrefab, _position + new Vector3(0, 0, -0.05f), _rotation);
                    Instantiate(moveVisualEffectPrefab, _position, Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)));
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