using System.Collections.Generic;
using UnityEngine;

public class CardData
{
    public int CardId { get; }
    public string Name { get; }
    public string Type { get; }
    public string Rarity { get; }
    public string Desc { get; }
    public bool IsEcho { get; }
    public int CDTurns { get; }
    public Sprite Image { get; }
    public int UpgradeId { get; }
    
    #region Excel待增字段
    public Effect ManualTargetEffect { get; private set; }
    public List<AutoTargetEffect> OtherEffects { get; private set; }
    #endregion

    public CardData(CardInfo info)
    {
        CardId = info.CardId;
        Name = info.Name;
        Type = info.Type;
        Rarity = info.Rarity;
        Desc = info.Desc;
        IsEcho = info.IsEcho;
        CDTurns = info.CDTurns;
        Image = Resources.Load<Sprite>("Sprite/" + info.Icon);
        UpgradeId = info.UpgradeId;
        // TODO: 解析手动/自动效果
    }

    #region 旧方法
    public CardData(CardConfig config)
    {
        CardId = config.CardId;
        Name = config.Name;
        Type = config.Type;
        Rarity = config.Rarity;
        Desc = config.Desc;
        IsEcho = config.IsEcho;
        CDTurns = config.CDTurns;
        Image = config.Image;
        UpgradeId = config.UpgradeId;
        ManualTargetEffect = config.ManualTargetEffect;
        OtherEffects = config.OtherEffects;
    }
    #endregion
}