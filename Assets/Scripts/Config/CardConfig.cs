using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/Card")]
public class CardConfig : SerializedScriptableObject
{
    [field: SerializeField] public int CardId { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Type { get; private set; }
    [field: SerializeField] public string Rarity { get; private set; }
    [field: SerializeField] public string Desc { get; private set; }
    [field: SerializeField] public bool IsEcho { get; private set; }
    [field: SerializeField] public int CDTurns { get; private set; }
    [field: SerializeField] public int UpgradeId { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    [OdinSerialize] public Effect ManualTargetEffect { get; private set; }
    [OdinSerialize] public List<AutoTargetEffect> OtherEffects { get; private set; }
}