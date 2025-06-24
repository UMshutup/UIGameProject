using UnityEngine;

public class AimToTarget : MonoBehaviour
{
    public Fighter fighter;

    public void Aim(Vector3 _aimPosition)
    {
        transform.LookAt(_aimPosition);
    }
}