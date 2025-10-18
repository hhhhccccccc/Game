using System.Collections.Generic;

/// <summary>
/// 战斗一轮息的逻辑计算
/// </summary>
public class BattleOneActionWheelLogicCalculateEventModel : MessageModel
{
    /// <summary>
    /// 当前行动息的角色
    /// </summary>
    public List<int> ActionWheelUnit { get; set; }
}
