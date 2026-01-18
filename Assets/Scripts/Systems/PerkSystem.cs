using System.Collections.Generic;
using UnityEngine;

public class PerkSystem : Singleton<PerkSystem>
{
    [SerializeField] private PerksUI perksUI;
    private readonly List<PerkData> perks = new();

    public void AddPerk(PerkData perkData)
    {
        perks.Add(perkData);
        perksUI.AddPerkUI(perkData);
        perkData.OnAdd();
    }

    public void RemovePerk(PerkData perkData)
    {
        perks.Remove(perkData);
        perksUI.RemovePerkUI(perkData);
        perkData.OnRemove();
    }
}