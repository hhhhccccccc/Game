using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ReferenceCollectorData
{
    [TableColumnWidth(100,resizable:false)]
    [HorizontalGroup("名称")]
    [HideLabel]
    public string Name;
    
    [FormerlySerializedAs("Obj")]
    [TableColumnWidth(150,resizable:false)]
    [HorizontalGroup("对象")]
    [HideLabel]
    public GameObject go;

    [TableColumnWidth(150,resizable:false)]
    [ValueDropdown("GetComs",NumberOfItemsBeforeEnablingSearch = 5),HorizontalGroup("组件类型")]
    [HideLabel]
    public int comType;
#if UNITY_EDITOR

    private List<ValueDropdownItem<int>> GetComs()
    {
        List<ValueDropdownItem<int>> result = new List<ValueDropdownItem<int>>();

        if (this.go == null)
            return null;

        if (!this.go.CompareTag("UIReference"))
        {
            result.Add(new ValueDropdownItem<int>("不生成代码", 0));
            return null;
        }

        var rfc = this.go.GetComponent<ReferenceCollector>();
        result.Add(new ValueDropdownItem<int>("GameObject", 0));
        
        foreach (var monoComponentInfo in ReferenceCollectorUtil.GetComponentTypes())
        {
            var index = monoComponentInfo.Index;
            var type = monoComponentInfo.Type;
            if (this.go.GetComponent(type) != null)
            {
                result.Add(new ValueDropdownItem<int>(type.Name, index));
            }
        }

        return result;
    }
#endif

}