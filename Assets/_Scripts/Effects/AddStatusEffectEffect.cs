using System.Collections.Generic;
using Sirenix.Serialization;

public class AddStatusEffectEffect : Effect
{
    [OdinSerialize] private StatusEffectType statusEffectType;
    [OdinSerialize] private int stackCount;

    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        return new AddStatusEffectGA(statusEffectType, stackCount, targets);
    }
}