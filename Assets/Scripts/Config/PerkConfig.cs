using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/Perk")]
public class PerkConfig : SerializedScriptableObject
{
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: OdinSerialize] public PerkCondition PerkCondition { get; private set; }
    [field: OdinSerialize] public AutoTargetEffect AutoTargetEffect { get; private set; }
    [field: SerializeField] public bool UseAutoTarget { get; private set; } = true;
    [field: SerializeField] public bool UseActionCasterAsTarget { get; private set; }
}