using UnityEngine;
using Zenject;

public class GameStartController : ControllerBase<GameStartEventModel>
{
    [Inject] private ILogManager LogManager;
    [Inject] private IPoolManager PoolManager;
    public override void Handle(GameStartEventModel model)
    {
        LogManager.Debug("游戏开始");
    }
}
