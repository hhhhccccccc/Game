using System.Collections.Generic;
using UnityEngine;

public class ExcelSystem : Singleton<ExcelSystem>
{
    [SerializeField] private bool DebugInfo;
    protected override void Awake()
    {
        base.Awake();
        BinaryDataMgr.Instance.InitData();
    }
    
    private void Start()
    {
        if (!DebugInfo)
            return;
        CardInfoContainer container = BinaryDataMgr.Instance.GetTable<CardInfoContainer>();
        foreach (KeyValuePair<int, CardInfo> card in container.dataDic)
        {
            CardInfo cardInfo = card.Value;
            print(
                $"{cardInfo.CardId}_{cardInfo.Name}_{cardInfo.Type}_{cardInfo.Rarity}_{cardInfo.Desc}_{cardInfo.IsEcho}_{cardInfo.CDTurns}_{cardInfo.Icon}_{cardInfo.UpgradeId}_{cardInfo.CardScript}_{cardInfo.CardParam}\n");
        }
    }
}