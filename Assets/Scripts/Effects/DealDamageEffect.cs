using System.Collections.Generic;
using Sirenix.Serialization;

public class DealDamageEffect : Effect
{
    [OdinSerialize] private int damageAmount;

    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        DealDamageGA dealDamage = new(damageAmount, targets, caster);
        return dealDamage;
    }
}