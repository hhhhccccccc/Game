using System.Collections.Generic;

/// <summary>
///     游戏动作只存放数据
/// </summary>
public abstract class GameAction
{
    public List<GameAction> PreReactions { get; private set; } = new();
    public List<GameAction> PerformReactions { get; private set; } = new();
    public List<GameAction> PostReactions { get; private set; } = new();
}