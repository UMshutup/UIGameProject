using UnityEngine;

public class Fighter : MonoBehaviour
{
    [Header("Basic info")]
    public string fighterName;

    [Header("Stats")]
    public float maxHP;
    public float meleeDamage;
    public float rangedDamage;
    public float meleeDefence;
    public float rangedDefence;
    public float speed;

    [HideInInspector] public float currentHP;
}
