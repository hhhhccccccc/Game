using cfg;

public class BattleSetSettlementDamageRateValueEventModel : MessageModel
{
    public int EntityID { get; set; }
    public bool IsSet { get; set; } //设置或者增减
    public float DamageRate { get; set; }
}
