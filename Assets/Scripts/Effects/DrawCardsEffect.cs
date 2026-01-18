using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DrawCardsEffect : Effect
{
    [SerializeField] private int drawAmount;

    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        return new DrawCardsGA(drawAmount);
    }
}