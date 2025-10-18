using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Zenject;

public class ConditionManager : ManagerBase, IConditionManager
{
    [Inject] private ConfigManager ConfigManager;
    [Inject] private DiContainer DiContainer;
    [Inject] private IPoolManager PoolManager;
    private Dictionary<string, Type> NameToType = new();
    protected override IEnumerator OnInit()
    {
        string modelName = GameConst.AssemblyNameForManager;
        Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault<Assembly>((Func<Assembly, bool>) (a => a.GetName().Name == modelName));
        Type[] allTypes = !(assembly == null) ? assembly.GetTypes() : throw new Exception("not found assembly, name: " + modelName);
        Type interfaceType = typeof (ICondition);
        IEnumerable<Type> types = ((IEnumerable<Type>) allTypes).Where<Type>((Func<Type, bool>) (t => interfaceType.IsAssignableFrom(t) && t != interfaceType && !t.IsAbstract));
        foreach (Type type in types)
        {
            if (type == null || string.IsNullOrEmpty(type.FullName))
                Debug.LogWarning((object) $"{type} is null or FullName is null.");
            else
            {
                this.DiContainer.Bind(type).AsTransient();
            }
        }
        yield break;
    }

    public bool Check(int conditionID, List<int> conditionParam)
    {
        var config = ConfigManager.GetConditionConfig(conditionID);
        var typeName = config.ConditionName;
        if (!NameToType.TryGetValue(typeName, out var type))
        {
            type = Type.GetType(typeName);
            NameToType.Add(typeName, type);
        }
        
        var conditionImpl = (ICondition)PoolManager.GetClass(type);
        var result = conditionImpl.Check();
        PoolManager.RecycleClass(conditionImpl);
        return result;
    }
}
