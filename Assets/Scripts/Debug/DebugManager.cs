using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class DebugManager : MonoSingleton<DebugManager>
{
    [LabelText("调试配置")] 
    private DebugConfig DebugConfig;
    
    private DiContainer DiContainer;
    private IResourceManager ResourceManager;
    private IPoolManager PoolManager;
    private ILogManager LogManager;
    public void DebugStart(DiContainer diContainer)
    {
        DiContainer = diContainer;
        ResourceManager = diContainer.Resolve<IResourceManager>();
        PoolManager = diContainer.Resolve<IPoolManager>();
        LogManager = diContainer.Resolve<ILogManager>();
        LogManager.Debug("调试开战初始化");
    }
}
