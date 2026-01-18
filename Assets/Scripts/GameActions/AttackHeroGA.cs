public class AttackHeroGA : GameAction, IHaveCaster
{
    public EnemyView Attacker { get; }
    public CombatantView Caster { get; }

    public AttackHeroGA(EnemyView attacker)
    {
        Attacker = attacker;
        Caster = Attacker;
    }
}