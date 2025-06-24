using UnityEngine;
using UnityEngine.EventSystems;

public class RangedDamageEffect : MonoBehaviour
{
    public float projectileSpeed;
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * projectileSpeed;
        Destroy(gameObject, 5);
    }
}
