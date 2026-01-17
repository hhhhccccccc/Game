using System;
using System.Collections.Generic;

[Serializable]
public abstract class Effect
{
    public abstract GameAction GetGameAction(List<CombatantView> targets, CombatantView caster);
}