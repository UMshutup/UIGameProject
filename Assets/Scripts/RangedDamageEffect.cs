using UnityEngine;

public class RangedDamageEffect : MonoBehaviour
{
    public float projectileSpeed;
    void Update()
    {
        transform.localPosition += new Vector3(0, 0, projectileSpeed * Time.deltaTime);
        Destroy(gameObject, 5);
    }
}
