using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

[Serializable]
public class DebugPlayer
{
    [LabelText("UID")]
    public int Uid;
    [FormerlySerializedAs("Characters")] 
    [LabelText("角色")]
    public List<DebugHeroData> HeroDatas;
}
