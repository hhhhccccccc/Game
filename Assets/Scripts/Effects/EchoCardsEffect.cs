using System.Collections.Generic;
using Sirenix.Serialization;

public class EchoCardsEffect : Effect
{
    [OdinSerialize] private CardConfig cardConfig;
    [OdinSerialize] private int echoAmount;
    public override GameAction GetGameAction(List<CombatantView> targets, CombatantView caster)
    {
        return new EchoCardsGA(cardConfig, echoAmount);
    }
}