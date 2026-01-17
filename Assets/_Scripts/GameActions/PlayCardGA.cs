public class PlayCardGA : GameAction
{
    public CardData CardData { get; set; }
    public EnemyView ManualTarget { get; private set; }

    public PlayCardGA(CardData cardData)
    {
        CardData = cardData;
        ManualTarget = null;
    }

    public PlayCardGA(CardData cardData, EnemyView target)
    {
        CardData = cardData;
        ManualTarget = target;
    }
}