using System.Collections.Generic;

/// <summary>
/// 设置技能
/// </summary>
public class BattleSetUnitSkillEventModel : MessageModel
{
    /// <summary>
    /// 当前行动息的角色
    /// </summary>
    public List<int> SetSkillUnitList { get; set; }
}
