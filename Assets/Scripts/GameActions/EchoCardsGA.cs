public class EchoCardsGA : GameAction
{
    public CardConfig CardConfig { get; set; }
    public int EchoAmount { get; set; }

    public EchoCardsGA(CardConfig cardConfig, int echoAmount)
    {
        CardConfig = cardConfig;
        EchoAmount = echoAmount;
    }
}