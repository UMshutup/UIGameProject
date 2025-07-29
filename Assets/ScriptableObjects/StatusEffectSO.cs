using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectSO", menuName = "Scriptable Objects/StatusEffectSO")]
public class StatusEffectSO : ScriptableObject
{
    public Sprite statusEffectSprite;
    public int statusEffectStartDuration;
    [Range(-1, 1)] public int affectedMeleeDamage;
    [Range(-1, 1)] public int affectedRangedDamage;
    [Range(-1, 1)] public int affectedMeleeDefence;
    [Range(-1, 1)] public int affectedRangedDefence;
    [Range(-1, 1)] public int affectedSpeed;
    [Range(-1, 1)] public int affectedAccuracy;
    [Range(-1, 1)] public int affectedEvasion;
    [Range(-1, 1)] public int affectedActionPointRegeneration;


}
