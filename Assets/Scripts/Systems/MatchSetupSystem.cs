using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    [SerializeField] private HeroConfig heroConfig;
    [SerializeField] private PerkConfig perkConfig;
    [SerializeField] private List<EnemyConfig> enemyConfigs;

    private void Start()
    {
        HeroSystem.Instance.Setup(heroConfig);
        EnemySystem.Instance.Setup(enemyConfigs);
        CardSystem.Instance.Setup(heroConfig);
        // CardSystem.Instance.Setup(BinaryDataMgr.Instance.GetTable<CardInfoContainer>());
        PerkSystem.Instance.AddPerk(new PerkData(perkConfig));
        DrawCardsGA drawCardsGA = new(5);
        ActionSystem.Instance.Perform(drawCardsGA);
    }
}