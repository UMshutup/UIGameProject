using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Scriptable Objects/Move")]
public class Move : ScriptableObject
{
    [SerializeField] private string moveName;
    [SerializeField] private string moveDescription;
    [SerializeField] private float moveDamage;
    [SerializeField] private MoveCategory moveCategory;

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

    public MoveCategory GetMoveCategory() 
    {
        return moveCategory;
    }

    public float CalculateDamage(Fighter user, Fighter target)
    {
        if (moveCategory == MoveCategory.MELEE)
        {
            return (moveDamage * user.meleeDamage / target.meleeDefence) / 2;
        }else if (moveCategory == MoveCategory.RANGED)
        {
            return (moveDamage * user.rangedDamage / target.rangedDefence) / 2;
        }
        else
        {
            return 0;
        }
    }

}
