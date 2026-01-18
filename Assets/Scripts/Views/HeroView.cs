public class HeroView : CombatantView
{
    public void Setup(HeroConfig heroConfig)
    {
        SetupBase(heroConfig.Health, heroConfig.Image);
    }
}