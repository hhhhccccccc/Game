using System;
using System.Collections.Generic;

[Serializable]
public abstract class TargetMode
{
    public abstract List<CombatantView> GetTargets();
}