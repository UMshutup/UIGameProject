using UnityEngine;

public class AimToTarget : MonoBehaviour
{
    private Fighter fighter;
    void Start()
    {
        fighter = GetComponentInParent<Fighter>();
    }
    void Update()
    {
        transform.LookAt(fighter.target.HitPosition.transform);
    }
}
