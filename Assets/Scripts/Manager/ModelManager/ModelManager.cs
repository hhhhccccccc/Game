using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Zenject;

public class ModelManager : ManagerBase, IInitRootBefore
{
  [Inject] private DiContainer DiContainer { get; set; }
  [Inject] private IArchiveManager ArchiveManager { get; set; }
  protected override IEnumerator OnInit()
  {
    this.InitModel();
    this.InitMessageModel();
    yield break;
  }

  private void InitModel()
  {
    string modelName = GameConst.AssemblyNameForModel;
    Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault<Assembly>((Func<Assembly, bool>) (a => a.GetName().Name == modelName));
    Type[] allTypes = !(assembly == null) ? assembly.GetTypes() : throw new Exception("not found assembly, name: " + modelName);
    Type interfaceType = typeof (IModel);
    IEnumerable<Type> types = ((IEnumerable<Type>) allTypes).Where<Type>((Func<Type, bool>) (t => interfaceType.IsAssignableFrom(t) && t != interfaceType && !t.IsAbstract));
    List<Type> typeList = new List<Type>();
    foreach (Type type in types)
    {
      if (type == null || string.IsNullOrEmpty(type.FullName))
        Debug.LogWarning((object) $"{type} is null or FullName is null.");
      else if (((IEnumerable<System.Type>) type.GetInterfaces()).Contains<System.Type>(typeof (ISingleArchiveModel)))
      {
        object instance = Activator.CreateInstance(type);
        ISingleArchiveModel obj = (ISingleArchiveModel)ArchiveManager.Load(type.FullName.GetHashCode().ToString(), instance);
        this.DiContainer.Inject(obj);
        this.DiContainer.Bind(type).FromInstance(obj).AsSingle();
        obj.Init();
      }
      else if (((IEnumerable<System.Type>) type.GetInterfaces()).Contains<System.Type>(typeof (ISingleModel)))
      {
        this.DiContainer.Bind(type).AsSingle();
        typeList.Add(type);
      }
      else
      {
        this.DiContainer.Bind(type).AsTransient();
      }
    }
    foreach (Type contractType in typeList)
      this.DiContainer.Resolve(contractType);
  }
  
  private void InitMessageModel()
  {
    string modelName = GameConst.AssemblyNameForMessage;
    Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault<Assembly>((Func<Assembly, bool>) (a => a.GetName().Name == modelName));
    Type[] allTypes = !(assembly == null) ? assembly.GetTypes() : throw new Exception("not found assembly, name: " + modelName);
    Type interfaceType = typeof (MessageModel);
    IEnumerable<Type> types = ((IEnumerable<Type>) allTypes).Where<Type>((Func<Type, bool>) (t => interfaceType.IsAssignableFrom(t) && t != interfaceType && !t.IsAbstract));
    foreach (Type type in types)
    {
      this.DiContainer.Bind(type).AsTransient();
    }
  }
  public void Save()
  {
    string modelName = GameConst.AssemblyNameForModel;
    Assembly assembly = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).FirstOrDefault<Assembly>((Func<Assembly, bool>) (a => a.GetName().Name == modelName));
    System.Type[] source = !(assembly == (Assembly) null) ? assembly.GetTypes() : throw new Exception("not found assembly, name: " + modelName);
    System.Type interfaceType = typeof (ISingleArchiveModel);
    Func<System.Type, bool> predicate = (Func<System.Type, bool>) (t => interfaceType.IsAssignableFrom(t) && t != interfaceType && !t.IsAbstract);
    foreach (System.Type contractType in ((IEnumerable<System.Type>) source).Where<System.Type>(predicate))
    {
      if (contractType == (System.Type) null || string.IsNullOrEmpty(contractType.FullName))
        Debug.LogWarning((object) string.Format("{0} is null or FullName is null.", (object) contractType));
      else
        ((ISingleArchiveModel) this.DiContainer.Resolve(contractType)).Save();
    }
  }
}
