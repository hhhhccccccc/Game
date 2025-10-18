using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Zenject;

public abstract class AppManager : MonoBehaviour
{
  private List<IUpdate> IUpdateLogicList = new List<IUpdate>();
  protected DiContainer DiContainer { get; private set; }
  private void Awake() => this.OnAwake();
  protected virtual void OnAwake()
  {
  }

  protected IEnumerator Start()
  {
    yield return (object)this.OnPreWork();
    yield return (object)this.InitAppManager();
    yield return (object)this.OnStart();
  }

  protected virtual IEnumerator OnPreWork()
  {
    yield break;
  }

  private IEnumerator InitAppManager()
  {
    AppManager appManager = this;
    SceneContext sceneContext = new GameObject("[AppManager]").AddComponent<SceneContext>();
    string managerAssemblyName = GameConst.AssemblyNameForManager;
    Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().First<Assembly>((Func<Assembly, bool>) (a => a.GetName().Name == managerAssemblyName));
    List<Type> initRootTypes = ((IEnumerable<Type>) assembly.GetTypes()).Where<Type>((Func<Type, bool>) (type => typeof (IInitRootBefore).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)).ToList<Type>();
    initRootTypes.AddRange(((IEnumerable<Type>) assembly.GetTypes()).Where<Type>((Func<Type, bool>) (type => typeof (IInitRootAfter).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)).ToList<Type>());
    appManager.DiContainer = sceneContext.Container;
    List<IManager> customManagers = new List<IManager>();
    List<IManager> list = new List<IManager>();
    yield return (object) appManager.InitCustomManagerBefore(customManagers);
    list.AddRange(customManagers);
    foreach (Type type in initRootTypes)
    {
      appManager.DiContainer.Bind(type).AsSingle();
      list.Add((IManager)appManager.DiContainer.Resolve(type));
    }
    yield return (object) appManager.InitCustomManagerAfter(customManagers);
    foreach (var initObj in list)
    {
      yield return initObj.Init();
    }

    yield return AddToInterfaceList();
    yield return (object) appManager.OnGameReady();
  }

  private IEnumerator AddToInterfaceList()
  {
    IUpdateLogicList.Add(DiContainer.Resolve<UIManager>());
    IUpdateLogicList.Add(DiContainer.Resolve<InputManager>());
    IUpdateLogicList.Add(DiContainer.Resolve<IJobManager>());
    
    yield break;
  }
  
  protected TManagerBase BindAndInjectManager<TManagerBase, TManager>()
    where TManagerBase : IManager
    where TManager : ManagerBase, TManagerBase, IManager
  {
    this.DiContainer.Bind<TManagerBase>().To<TManager>().AsSingle();
    return this.DiContainer.Resolve<TManagerBase>();
  }

  protected virtual IEnumerator OnGameReady()
  {
    yield break;
  }

  protected virtual IEnumerator OnStart()
  {
    yield break;
  }
  
  protected virtual IEnumerator InitCustomManagerBefore(List<IManager> customManagers)
  {
    yield break;
  }

  protected virtual IEnumerator InitCustomManagerAfter(List<IManager> customManagers)
  {
    yield break;
  }

  private void OnApplicationQuit() => this.DiContainer.Resolve<ModelManager>().Save();

  private void OnApplicationFocus(bool hasFocus)
  {
  }

  private void OnApplicationPause(bool pauseStatus)
  {
  }

  private void Update()
  {
    foreach (var logic in IUpdateLogicList)
    {
      logic.OnUpdate(Time.deltaTime);
    }
  }
}

