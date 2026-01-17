using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/Enemy")]
public class EnemyConfig : SerializedScriptableObject
{
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int AttackPower { get; private set; }
}