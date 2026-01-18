using TMPro;
using UnityEngine;

public class EnemyView : CombatantView
{
    [SerializeField] private TMP_Text attackText;
    public int AttackPower { get; set; }

    public void Setup(EnemyConfig enemyConfig)
    {
        AttackPower = enemyConfig.AttackPower;
        UpdateAttackText();
        SetupBase(enemyConfig.Health, enemyConfig.Image);
    }

    public void UpdateAttackText()
    {
        attackText.text = "ATK:" + AttackPower;
    }
}