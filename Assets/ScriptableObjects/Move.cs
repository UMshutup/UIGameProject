using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Scriptable Objects/Move")]
public class Move : ScriptableObject
{
    [SerializeField] private string moveName;
    [SerializeField] private string moveDescription;
    [SerializeField] private float moveDamage;
    [SerializeField, Range(0f, 100f)] private float moveAccuracy;
    [SerializeField] private MoveCategory moveCategory;
    [SerializeField] private GameObject moveVisualEffectPrefab;
    [SerializeField] private GameObject moveMissEffectPrefab;

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

    public MoveCategory GetMoveCategory() 
    {
        return moveCategory;
    }

    public GameObject GetMoveVisualEffectPrefab()
    {
        return moveVisualEffectPrefab;
    }

    public float CalculateDamage(Fighter user, Fighter target)
    {
        if (moveCategory == MoveCategory.MELEE && target.meleeDefence != 0)
        {
            return (moveDamage * user.meleeDamage / target.meleeDefence) / 2;
        }else if (moveCategory == MoveCategory.RANGED && target.rangedDefence != 0)
        {
            return (moveDamage * user.rangedDamage / target.rangedDefence) / 2;
        }
        else
        {
            return 0;
        }
    }

    public void ShowMoveVisualEffect(Transform _transform, bool _hasMoveLanded)
    {
        if (moveVisualEffectPrefab != null)
        {
            if (moveCategory == MoveCategory.MELEE)
            {
                if (_hasMoveLanded) 
                {
                    Instantiate(moveVisualEffectPrefab, _transform.position + new Vector3(0, 0, -0.05f), _transform.rotation);
                }
                else
                {
                    Instantiate(moveMissEffectPrefab, _transform.position + new Vector3(0, 0, -0.05f), _transform.rotation);
                }
            }
            else if (moveCategory == MoveCategory.RANGED)
            {
                if (_hasMoveLanded)
                {
                    Instantiate(moveVisualEffectPrefab, _transform);
                }
                else
                {
                    Instantiate(moveMissEffectPrefab, _transform.position + new Vector3(0, 0, -0.05f), _transform.rotation);
                    Instantiate(moveVisualEffectPrefab, _transform.position, Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)));
                }
                
            }
        }
    }

}
