using System.Collections.Generic;
using UnityEngine;

public class CardData
{
    private readonly CardConfig Config;
    public string Title { get; }
    public string Description { get; }
    public Sprite Image { get; }
    public int Mana { get; private set; }
    public Effect ManualTargetEffect { get; private set; }
    public List<AutoTargetEffect> OtherEffects { get; private set; }

    public CardData(CardConfig config)
    {
        Config = config;
        Title = config.name;
        Description = config.Description;
        Image = config.Image;
        Mana = config.Mana;
        ManualTargetEffect = config.ManualTargetEffect;
        OtherEffects = config.OtherEffects;
    }
}