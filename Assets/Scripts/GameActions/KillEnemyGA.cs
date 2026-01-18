public class KillEnemyGA : GameAction
{
    public EnemyView Target { get; private set; }

    public KillEnemyGA(EnemyView target)
    {
        Target = target;
    }
}