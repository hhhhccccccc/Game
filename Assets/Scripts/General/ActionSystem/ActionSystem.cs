using System;
using System.Collections;
using System.Collections.Generic;

public enum ReactionTiming
{
    PRE,
    POST
}

public class ActionSystem : Singleton<ActionSystem>
{
    private static readonly Dictionary<Type, List<Action<GameAction>>> PreSubReactions = new();
    private static readonly Dictionary<Type, List<Action<GameAction>>> PostSubReactions = new();
    /// <summary>
    ///     执行者存放动作的逻辑 每种游戏动作类型附加一个执行者
    /// </summary>
    private static readonly Dictionary<Type, Func<GameAction, IEnumerator>> Performers = new();
    private List<GameAction> reactions;
    public bool IsPerforming { get; private set; }

    public void Perform(GameAction action, Action OnPerformFinished = null)
    {
        if (IsPerforming) return;
        IsPerforming = true;
        StartCoroutine(Flow(action, () =>
        {
            IsPerforming = false;
            OnPerformFinished?.Invoke();
        }));
    }

    public void AddReaction(GameAction gameAction)
    {
        reactions?.Add(gameAction);
    }

    private IEnumerator Flow(GameAction action, Action OnFlowFinished = null)
    {
        reactions = action.PreReactions;
        PerformSubscribers(action, PreSubReactions);
        yield return PerformReactions();

        reactions = action.PerformReactions;
        yield return PerformPerformer(action);
        yield return PerformReactions();

        reactions = action.PostReactions;
        PerformSubscribers(action, PostSubReactions);
        yield return PerformReactions();

        OnFlowFinished?.Invoke();
    }

    private IEnumerator PerformPerformer(GameAction action)
    {
        Type type = action.GetType();
        if (Performers.ContainsKey(type))
            yield return Performers[type](action);
    }

    private void PerformSubscribers(GameAction action, Dictionary<Type, List<Action<GameAction>>> subs)
    {
        Type type = action.GetType();
        if (subs.ContainsKey(type))
            foreach (Action<GameAction> sub in subs[type])
                sub(action);
    }

    private IEnumerator PerformReactions()
    {
        foreach (GameAction reaction in reactions)
            yield return Flow(reaction);
    }

    public static void AttachPerformer<T>(Func<T, IEnumerator> performer) where T : GameAction
    {
        Type type = typeof(T);

        IEnumerator wrapperPerformer(GameAction action)
        {
            return performer((T)action);
        }

        if (Performers.ContainsKey(type)) Performers[type] = wrapperPerformer;
        else Performers.Add(type, wrapperPerformer);
    }

    public static void DetachPerformer<T>() where T : GameAction
    {
        Type type = typeof(T);
        if (Performers.ContainsKey(type)) Performers.Remove(type);
    }

    public static void SubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? PreSubReactions : PostSubReactions;

        void wrappedReaction(GameAction action)
        {
            reaction((T)action);
        }

        if (subs.ContainsKey(typeof(T)))
            subs[typeof(T)].Add(wrappedReaction);
        else
        {
            subs.Add(typeof(T), new List<Action<GameAction>>());
            subs[typeof(T)].Add(wrappedReaction);
        }
    }

    public static void UnsubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? PreSubReactions : PostSubReactions;
        if (subs.ContainsKey(typeof(T)))
        {
            void wrappedReaction(GameAction action)
            {
                reaction((T)action);
            }

            subs[typeof(T)].Remove(wrappedReaction);
        }
    }
}