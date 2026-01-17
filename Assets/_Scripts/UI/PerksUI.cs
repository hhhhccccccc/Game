using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerksUI : MonoBehaviour
{
    [SerializeField] private PerkUI perkUIPrefab;
    private readonly List<PerkUI> perkUIs = new();

    public void AddPerkUI(PerkData perk)
    {
        PerkUI perkUI = Instantiate(perkUIPrefab, transform);
        perkUI.Setup(perk);
        perkUIs.Add(perkUI);
    }

    public void RemovePerkUI(PerkData perk)
    {
        PerkUI perkUI = perkUIs.FirstOrDefault(pui => pui.PerkData == perk);
        if (perkUI != null)
        {
            perkUIs.Remove(perkUI);
            Destroy(perkUI.gameObject);
        }
    }
}