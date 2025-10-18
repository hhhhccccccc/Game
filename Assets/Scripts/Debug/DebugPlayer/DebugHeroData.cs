using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public class DebugHeroData
{
    public int HeroID;

    public int SlotIndex;

    public int Level;

    public List<int> WearSkill;

    public List<int> WearHeartMethod;
    public List<int> WearTreasure;
}
