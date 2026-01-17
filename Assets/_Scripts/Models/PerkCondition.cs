using System;
using Sirenix.Serialization;

public abstract class PerkCondition
{
    [field: OdinSerialize] protected ReactionTiming reactionTiming;
    public abstract void SubscribeCondition(Action<GameAction> reaction);
    public abstract void UnsubscribeCondition(Action<GameAction> reaction);
    public abstract bool SubConditionIsMet(GameAction gameAction);
}