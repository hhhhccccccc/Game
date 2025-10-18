using cfg;

public class BattleShowMomentRecordEventModel : MessageModel
{
    public int EntityID { get; set; }
    //public DamageType DamageType { get; set; }
    public BattleMomentType BattleMomentType { get; set; }
    public BattleSource BattleSource { get; set; }
    public int ConfigID { get; set; }
}
