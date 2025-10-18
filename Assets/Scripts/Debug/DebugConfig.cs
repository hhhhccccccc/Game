using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "DebugConfig", menuName = "Battle/DebugConfig")]
public class DebugConfig : ScriptableObject
{
    [LabelText("玩家")]
    public List<DebugPlayer> Players;
}
