using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ReferenceCollectorDefine
{
    public enum ViewType
    {
        Window = 1,
        Component = 2
    }
}

public struct ComponentType
{
    public int Index;
    public Type Type;
}

public struct ViewTypeInfo
{
    public int Index;
    public string TypeName;
}

public static class ReferenceCollectorUtil
{
    public static List<ViewTypeInfo> GetViewTypes()
    {
        var list = new List<ViewTypeInfo>();
        foreach (var data in Enum.GetValues(typeof(ReferenceCollectorDefine.ViewType)))
        {
            int intValue = (int)data;
            string stringValue = Enum.GetName(typeof(ReferenceCollectorDefine.ViewType), data);
            list.Add(new ViewTypeInfo{Index = intValue, TypeName = stringValue});
        }

        return list;
    }
    
    public static List<ComponentType> GetComponentTypes()
    {
        return new List<ComponentType>
        {
            new ComponentType{Index = 1, Type = typeof(UnityEngine.UI.Button)},
            new ComponentType{Index = 2, Type = typeof(UnityEngine.UI.Image)},
            new ComponentType{Index = 3, Type = typeof(TextMeshProUGUI)},
            new ComponentType{Index = 4, Type = typeof(UIBar)}
        };
    }

    public static string GetComponentNameByIndex(int index)
    {
        if (index == 0)
            return string.Empty;
        return GetComponentTypes()[index - 1].Type.Name;
    }
}
