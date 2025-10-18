public class BattleSetSettlementUIEventModel : MessageModel
{
    public int EntityID { get; set; }
    public bool State { get; set; }
    public string AniName { get; set; }
    /// <summary>
    /// 为0就不关闭
    /// </summary>
    public float DelayClose { get; set; }
    
    public override void Recycle()
    {
        base.Recycle();
        EntityID = 0;
        State = false;
        AniName = string.Empty;
        DelayClose = 0;
    }
}
