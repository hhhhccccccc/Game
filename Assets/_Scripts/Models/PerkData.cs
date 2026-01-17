using System.Collections.Generic;
using UnityEngine;

public class PerkData
{
    public Sprite Image;
    private readonly PerkConfig config;
    private readonly PerkCondition condition;
    private readonly AutoTargetEffect AutoEF;

    public PerkData(PerkConfig perkConfig)
    {
        config = perkConfig;
        Image = config.Image;
        condition = config.PerkCondition;
        AutoEF = config.AutoTargetEffect;
    }

    public void OnAdd()
    {
        condition.SubscribeCondition(Reaction);
    }

    public void OnRemove()
    {
        condition.UnsubscribeCondition(Reaction);
    }

    private void Reaction(GameAction gameAction)
    {
        if (condition.SubConditionIsMet(gameAction))
        {
            List<CombatantView> targets = new();
            if (config.UseActionCasterAsTarget && gameAction is IHaveCaster caster)
                targets.Add(caster.Caster);
            if (config.UseAutoTarget)
                targets.AddRange(AutoEF.TargetMode.GetTargets());
            GameAction perkEffectAction = AutoEF.Effect.GetGameAction(targets, HeroSystem.Instance.HeroView);
            ActionSystem.Instance.AddReaction(perkEffectAction);
        }
    }
}