using System.Collections.Generic;

/// <summary>
/// 战斗一轮息的扳机计算
/// </summary>
public class BattleOneActionWheelMomentCalculateEventModel : MessageModel
{
    /// <summary>
    /// 当前行动息的角色
    /// </summary>
    public List<int> ActionWheelUnit { get; set; }
}
