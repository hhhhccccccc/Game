using UnityEngine;
#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector;
using UnityEditor;
using Object = UnityEngine.Object;
#endif


public class ReferenceCollector : MonoBehaviour, ISerializationCallbackReceiver
{
    public const string UIGenRoot = "Assets/Scripts/View/Panel";
#if UNITY_EDITOR
    [VerticalGroup("g1")]
    [LabelText("类型")]
    [ValueDropdown("GetViewType")]
    public int ViewType;
    
    [VerticalGroup("g1")]
    [LabelText("类型")]
    [ValueDropdown("GetComponentType")]
    //[ShowIf("IsComponent")]
    [HideInInspector]
    public int ComponentType; 
    
    [VerticalGroup("g1")]
    [Button("添加引用",ButtonSizes.Large)]
    private void Add()
    {
        AutoAddRef();
    }

    private bool IsComponent()
    {
        return ViewType == (int)ReferenceCollectorDefine.ViewType.Component;
    }
    
    private List<ValueDropdownItem<int>> GetViewType()
    {
        var list = new List<ValueDropdownItem<int>>();
        foreach (var info in ReferenceCollectorUtil.GetViewTypes())
        {
            list.Add(new ValueDropdownItem<int>(info.TypeName, info.Index));
        }

        return list;
    }
    
    private List<ValueDropdownItem<int>> GetComponentType()
    {
        var list = new List<ValueDropdownItem<int>>();
        foreach (var info in ReferenceCollectorUtil.GetComponentTypes())
        {
            list.Add(new ValueDropdownItem<int>(info.Type.Name, info.Index));
        }

        return list;
    }
    
    [Button("生成代码",ButtonSizes.Large)]
    //[ShowIf("ShowGen")]
    public void GenCode()
    {
        AutoGenCode();
    }

    private bool ShowGen()
    {
        switch (ViewType)
        {
            case (int)ReferenceCollectorDefine.ViewType.Window:
            case (int)ReferenceCollectorDefine.ViewType.Component when ComponentType != 0:
                return true;
            default:
                return false;
        }
    }
    
    //用于序列化的List
    [VerticalGroup("obj")]
    [TableList]
    public List<ReferenceCollectorData> Data = new List<ReferenceCollectorData>();
    //Object并非C#基础中的Object，而是 UnityEngine.Object
    private readonly List<Object> GoList = new List<Object>();

    private void AutoAddRef()
    {
        var gos = new List<GameObject>();
        FindTagGameObject(gameObject, gos);
        this.Data.Clear();
        foreach (var go in gos)
        {
            AddFromReference(go);
        }
    }

    private static void FindTagGameObject(GameObject parent , List<GameObject> result)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            var go = parent.transform.GetChild(i).gameObject;
            if (PrefabUtility.GetPrefabAssetType(go) == PrefabAssetType.Regular)//是预制体
            {
                var com = go.GetComponent<ReferenceCollector>();
                if (com != null && com.ViewType == (int)ReferenceCollectorDefine.ViewType.Component && com.ComponentType != 0)
                {
                    result.Add(go);
                }
                continue;   
            }
               
            if (go.CompareTag("UIReference"))
                result.Add(go);
            FindTagGameObject(go, result);
        }
    }

    private void AddFromReference(GameObject go)
    {
        var code = GetFirstComponent(go);
        var info = new ReferenceCollectorData()
        {
            comType = code,
            Name = go.name,
            go = go
        };
        if (Data.Any(d => d.Name == name))
        {
            Debug.LogError($"存在相同名称请修改 名称:{name}");
        }
        Data.Add(info);
    }

    private int GetFirstComponent(GameObject obj)
    {
        if (!obj.CompareTag("UIReference"))
        {
            return 0;
        }
       
        foreach (var component in ReferenceCollectorUtil.GetComponentTypes())
        {
            if (obj.GetComponent(component.Type) != null)
            {
                return component.Index;
            }
        }
        
        return 0;
    }
    
    
    #region 生成代码
    private void AutoGenCode()
    {
        var folder_Gen = GetFolder(true);
        if (!Directory.Exists(folder_Gen))
        {
            Directory.CreateDirectory(folder_Gen);
        }
        var csGenPath_Gen = GetCSGenPath(true);
        GenCS_Gen(csGenPath_Gen);
        
        var folder = GetFolder(false);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        var csGenPath = GetCSGenPath(false);
        GenCS(csGenPath);
    }

    private void GenCS_Gen(string filePath)
    {
        void Gen()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"using UnityEngine;");
            sb.AppendLine($"using UnityEngine.UI;");
            sb.AppendLine($"using TMPro;");
            var isWindow = ViewType == (int)ReferenceCollectorDefine.ViewType.Window;
            if (isWindow)
            {
                sb.AppendLine($"public partial class {gameObject.name} : Panel");
            }
            else
            {
                var tempName = gameObject.name;
                var final = gameObject.name;;
                if (tempName.EndsWith("Entity"))
                {
                    final = tempName.Replace("Entity", "Component");
                }
                sb.AppendLine($"public partial class {final} : UIComponent");
            }
            sb.AppendLine("{");
            var btnList = new List<GameObject>();
            foreach (var data in Data)
            {
                if (data.comType > 0)
                {
                    var typeName = ReferenceCollectorUtil.GetComponentNameByIndex(data.comType);
                    sb.AppendLine($"    [AutoFind] private {typeName} {data.Name}  {{ get; set; }}");

                    if (typeName == "Button")
                    {
                        btnList.Add(data.go);
                    }
                }
                else
                {
                    sb.AppendLine($"    [AutoFind] private GameObject {data.Name}  {{ get; set; }}");
                }
            }
            sb.AppendLine("    protected override void OnAwake()");
            sb.AppendLine("    {");
            sb.AppendLine("        base.OnAwake();");
            foreach (var btnObj in btnList)
            {
                sb.AppendLine($"        {btnObj.name}.onClick.AddListener(On{btnObj.name});");
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");
            File.WriteAllText(filePath,sb.ToString());
        }
        
        if (!File.Exists(filePath))
            using (File.Create(filePath)){}
        Gen();
    }

    private void GenCS(string filePath)
    {
        void Gen()
        {
            StringBuilder sb = new StringBuilder();
            var isWindow = ViewType == (int)ReferenceCollectorDefine.ViewType.Window;
            if (isWindow)
            {
                sb.AppendLine($"public partial class {gameObject.name} : Panel");
            }
            else
            {
                var tempName = gameObject.name;
                var final = gameObject.name;;
                if (tempName.EndsWith("Entity"))
                {
                    final = tempName.Replace("Entity", "Component");
                }
                sb.AppendLine($"public partial class {final}");
            }
            sb.AppendLine("{");
            sb.AppendLine("}");
            File.WriteAllText(filePath,sb.ToString());
        }

        if (File.Exists(filePath)) return;
        using (File.Create(filePath)){}
        Gen();
    }

    private string GetFolder(bool isGen)
    {
        if (isGen)
        {
            return $"{Application.dataPath}/Scripts/View/Panel/Gen/{gameObject.name}";
        }
        else
        {
            return $"{Application.dataPath}/Scripts/View/Panel/UILogic/{gameObject.name}";
        }
    }
    
    private string GetCSGenPath(bool isGen)
    {
        if (isGen)
        {
            return $"{Application.dataPath}/Scripts/View/Panel/Gen/{gameObject.name}/{gameObject.name}_Gen.cs";
        }
        else
        {
            return $"{Application.dataPath}/Scripts/View/Panel/UILogic/{gameObject.name}/{gameObject.name}.cs";
        }
    }
    
    private void WriteFunction(StringBuilder sb,string parentName,string functionName, string extraCode = "")
    {
        sb.AppendLine($"function {parentName}:{functionName}()");
        sb.AppendLine($"    {extraCode}");
        sb.AppendLine($"end");
        sb.AppendLine("");
    }
    #endregion
    

#endif
    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
#if UNITY_EDITOR
        GoList.Clear();
        foreach (ReferenceCollectorData referenceCollectorData in Data)
        {
            if (!GoList.Contains(referenceCollectorData.go))
            {
                GoList.Add(referenceCollectorData.go);
            }
        }
#endif
    }
}
