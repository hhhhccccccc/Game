using System.Globalization;
using Zenject;

public class DebugController : ControllerBase<DebugEventModel>
{
    [Inject] private ConfigManager ConfigManager;
    [Inject] private ILogManager LogManager;
    [Inject] private IConditionManager ConditionManager;
    public override void Handle(DebugEventModel model)
    {
        /*LogManager.Debug(ConfigManager.GetSceneConfig(1).InteractionItem[0].InteractionItemID.ToString());
        LogManager.Debug(ConfigManager.GetSceneConfig(1).InteractionItem[0].X.ToString(CultureInfo.InvariantCulture));
        LogManager.Debug(ConfigManager.GetSceneConfig(1).InteractionItem[0].Y.ToString(CultureInfo.InvariantCulture));
        
        LogManager.Debug(ConfigManager.GetSceneConfig(1).MiniMapPos.X.ToString(CultureInfo.InvariantCulture));
        LogManager.Debug(ConfigManager.GetSceneConfig(1).MiniMapPos.Y.ToString(CultureInfo.InvariantCulture));

        ConditionManager.Check(1, null);*/
    }
}
