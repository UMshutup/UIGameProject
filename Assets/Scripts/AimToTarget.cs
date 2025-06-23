using UnityEngine;

public class AimToTarget : MonoBehaviour
{
    public Fighter fighter;

    void Update()
    {
        transform.LookAt(fighter.target.HitPosition.transform);
    }
}
