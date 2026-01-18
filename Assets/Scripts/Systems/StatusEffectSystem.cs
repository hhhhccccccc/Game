using System.Collections;
using UnityEngine;

public class StatusEffectSystem : MonoBehaviour
{
    private void OnEnable()
    {
        ActionSystem.AttachPerformer<AddStatusEffectGA>(AddStatusEffectPerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<AddStatusEffectGA>();
    }

    private IEnumerator AddStatusEffectPerformer(AddStatusEffectGA addStatusEffectGA)
    {
        foreach (CombatantView target in addStatusEffectGA.Targets)
        {
            target.AddStatusEffect(addStatusEffectGA.statusEffectType, addStatusEffectGA.StackCount);
            yield return null;
        }
    }
}