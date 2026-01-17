using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/Card")]
public class CardConfig : SerializedScriptableObject
{
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public int Mana { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    [OdinSerialize] public Effect ManualTargetEffect { get; private set; }
    [OdinSerialize] public List<AutoTargetEffect> OtherEffects { get; private set; }
}