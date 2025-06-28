using UnityEngine;

public class MoveInstance
{
    [HideInInspector] public string moveName;
    [HideInInspector] public string moveDescription;
    [HideInInspector] public float moveDamage;
    [HideInInspector] public float moveAccuracy;
    [HideInInspector] public int moveApCost;
    [HideInInspector] public MoveCategory moveCategory;
    [HideInInspector] public MoveTarget moveTarget;
    [HideInInspector] public bool hitsAWholeSquad;
    [HideInInspector] public GameObject moveVisualEffectPrefab;
    [HideInInspector] public GameObject moveMissEffectPrefab;
    [HideInInspector] public Move originalMove;

    public MoveInstance(Move move)
    {
        moveName = move.GetMoveName();
        moveDescription = move.GetMoveDescription();
        moveDamage = move.GetMoveDamage();
        moveAccuracy = move.GetMoveAccuracy();
        moveTarget = move.GetMoveTarget();
        moveAccuracy = move.GetMoveAccuracy();
        moveApCost = move.GetMoveApCost();
        moveCategory = move.GetMoveCategory();
        moveTarget = move.GetMoveTarget();
        hitsAWholeSquad = move.GetHitsAWholeSquad();
        moveVisualEffectPrefab = move.GetMoveVisualEffectPrefab();
        moveMissEffectPrefab = move.GetMoveMissEffectPrefab();
        originalMove = move;
    }

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

    public float CalculateDamage(Fighter user, Fighter target)
    {
        if (moveCategory == MoveCategory.MELEE && target.meleeDefence != 0)
        {
            return (moveDamage * user.meleeDamage / target.meleeDefence) / 2;
        }
        else if (moveCategory == MoveCategory.RANGED && target.rangedDefence != 0)
        {
            return (moveDamage * user.rangedDamage / target.rangedDefence) / 2;
        }
        else
        {
            return 0;
        }
    }

    public void ShowMoveVisualEffect(Vector3 _position, Quaternion _rotation, bool _hasMoveLanded)
    {
        originalMove.ShowMoveVisualEffect(_position, _rotation, _hasMoveLanded);
    }
}
