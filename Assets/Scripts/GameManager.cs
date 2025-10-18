using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using YooAsset;
#if !UNITY_EDITOR || FORCE_HOT_FIX
//using System.Collections.Generic;
//using System.IO;
//using System.Reflection;
//using HybridCLR;
#endif

namespace App
{
    public class GameManager : AppManager
    {
        [LabelText("是否是调试战斗")]
        public bool IsDebugBattle = false;
        protected override IEnumerator OnPreWork()
        {
            YooAssets.Initialize();
            var package = YooAssets.CreatePackage("DefaultPackage");
            YooAssets.SetDefaultPackage(package);
            var buildResult = EditorSimulateModeHelper.SimulateBuild("DefaultPackage");    
            var packageRoot = buildResult.PackageRootDirectory;
            var editorFileSystemParams = FileSystemParameters.CreateDefaultEditorFileSystemParameters(packageRoot);
            var initParameters = new EditorSimulateModeParameters
            {
                EditorFileSystemParameters = editorFileSystemParams
            };
            var initOperation = package.InitializeAsync(initParameters);
            yield return initOperation;
    
            if(initOperation.Status == EOperationStatus.Succeed)
                Debug.Log("资源包初始化成功！");
            else 
                Debug.LogError($"资源包初始化失败：{initOperation.Error}");
        
            // 2. 请求资源清单的版本信息
            var operation2 = package.RequestPackageVersionAsync();
            yield return operation2;
            if (operation2.Status != EOperationStatus.Succeed)
                yield break;
    
            // 3. 传入的版本信息更新资源清单
            var operation3 = package.UpdatePackageManifestAsync(operation2.PackageVersion);
            yield return operation3;
            if (operation3.Status != EOperationStatus.Succeed)
                yield break;
        }
        
        protected override IEnumerator InitCustomManagerBefore(List<IManager> customManagers)
        {
            //初始化表格
            DiContainer.Bind<ConfigManager>().AsSingle();
            var configManager = DiContainer.Resolve<ConfigManager>();
            yield return configManager.OnInit();
            //将所需服务依赖注入进来
            customManagers.Add(BindAndInjectManager<IResourceManager, ResourceManager>());
            customManagers.Add(BindAndInjectManager<IMessageManager, MessageManager>());
            customManagers.Add(BindAndInjectManager<ILogManager, LogManager>());
            customManagers.Add(BindAndInjectManager<IPoolManager, PoolManager>());
            customManagers.Add(BindAndInjectManager<IJobManager, JobManager>());
            customManagers.Add(BindAndInjectManager<IArchiveManager, ArchiveManager>());
            customManagers.Add(BindAndInjectManager<IConditionManager, ConditionManager>());
        }
        
        protected override IEnumerator OnGameReady()
        {
            DiContainer.Resolve<IMessageManager>().DispatchMsg<GameStartEventModel>(null);
            if (IsDebugBattle)
            {
                DebugManager.Instance.DebugStart(this.DiContainer);
            }
            yield break;
        }
    }
}
