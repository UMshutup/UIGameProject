using UnityEngine;

public class StatusEffectInstance
{
    public Sprite statusEffectSprite;
    public int statusEffectDuration;
    public int affectedMeleeDamage;
    public int affectedRangedDamage;
    public int affectedMeleeDefence;
    public int affectedRangedDefence;
    public int affectedSpeed;
    public int affectedAccuracy;
    public int affectedEvasion;
    public int affectedActionPointRegeneration;


    public bool hasAffectedStats = false;

    public bool hasRevertedStats = false;

    public StatusEffectInstance(StatusEffectSO _statusEffect) 
    { 
        statusEffectSprite = _statusEffect.statusEffectSprite;
        statusEffectDuration = _statusEffect.statusEffectStartDuration;
        affectedMeleeDamage = _statusEffect.affectedMeleeDamage;
        affectedMeleeDefence = _statusEffect.affectedMeleeDefence;
        affectedRangedDamage = _statusEffect.affectedRangedDamage;
        affectedRangedDefence = _statusEffect.affectedRangedDefence;
        affectedSpeed = _statusEffect.affectedSpeed;
        affectedAccuracy = _statusEffect.affectedAccuracy;
        affectedEvasion = _statusEffect.affectedEvasion;
        affectedActionPointRegeneration = _statusEffect.affectedActionPointRegeneration;
    }

    public void ReduceDuration()
    {
        statusEffectDuration -= 1;
    }

    public void ChangeStats(Fighter _fighter)
    {
        _fighter.meleeDamage += (_fighter.meleeDamage * 50 / 100) * affectedMeleeDamage;
        _fighter.rangedDamage += (_fighter.rangedDamage * 50 / 100) * affectedRangedDamage;
        _fighter.meleeDefence += (_fighter.meleeDefence * 50 / 100) * affectedMeleeDefence;
        _fighter.rangedDefence += (_fighter.rangedDefence * 50 / 100) * affectedRangedDefence;
        _fighter.speed += (_fighter.speed * 25 / 100) * affectedSpeed;
        _fighter.accuracy += (_fighter.accuracy * 25 / 100) * affectedAccuracy;
        _fighter.evasion += (_fighter.evasion * 25 / 100) * affectedEvasion;
        _fighter.evasion += (_fighter.evasion * 25 / 100) * affectedEvasion;
        _fighter.actionPointRegeneration += affectedActionPointRegeneration;

        hasAffectedStats = true;
    }

    public void RevertStatChange(Fighter _fighter)
    {
        _fighter.meleeDamage -= ((_fighter.meleeDamage * 50 / 100) * affectedMeleeDamage);
        _fighter.rangedDamage -= (_fighter.rangedDamage * 50 / 100) * affectedRangedDamage;
        _fighter.meleeDefence -= (_fighter.meleeDefence * 50 / 100) * affectedMeleeDefence;
        _fighter.rangedDefence -= (_fighter.rangedDefence * 50 / 100) * affectedRangedDefence;
        _fighter.speed -= (_fighter.speed * 25 / 100) * affectedSpeed;
        _fighter.accuracy -= (_fighter.accuracy * 25 / 100) * affectedAccuracy;
        _fighter.evasion -= (_fighter.evasion * 25 / 100) * affectedEvasion;
        _fighter.evasion -= (_fighter.evasion * 25 / 100) * affectedEvasion;
        _fighter.actionPointRegeneration -= affectedActionPointRegeneration;

        hasAffectedStats = true;
    }

    public override int GetHashCode()
    {
        return this.statusEffectSprite.GetHashCode();
    }

    override
    public bool Equals(object other)
    {
        if ((object)other == null)
            return false;

        return this.statusEffectSprite == ((StatusEffectInstance)other).statusEffectSprite;
    }


}
