using UnityEngine;

public class AimToTarget : MonoBehaviour
{
    public Fighter fighter;

    void Update()
    {
        transform.LookAt(fighter.GetHitPositionOfTargets());
    }
}
