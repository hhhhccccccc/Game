using System.Collections;
using System.Collections.Generic;
using System.IO;
using cfg;
using SimpleJSON;
using UnityEngine;

public class ConfigManager
{
    private Tables _tables;

    public IEnumerator OnInit()
    {
        string gameConfDir = Application.streamingAssetsPath + "/Luban"; // 替换为gen.bat中outputDataDir指向的目录
        _tables = new Tables(file => JSON.Parse(File.ReadAllText($"{gameConfDir}/{file}.json")));
        yield break;
    }
    
    public Dictionary<int, ConditionConfig> GetConditionConfigMap()
    {
        return _tables.TbConditionConfig.DataMap;
    }

    public ConditionConfig GetConditionConfig(int conditionID)
    {
        return _tables.TbConditionConfig.DataMap.GetValueOrDefault(conditionID, null);
    }

    
    public Dictionary<int, CommonPoolConfig> GetCommonPoolConfigMap()
    {
        return _tables.TbCommonPoolConfig.DataMap;
    }

    public CommonPoolConfig GetCommonPoolConfig(int poolID)
    {
        return _tables.TbCommonPoolConfig.DataMap.GetValueOrDefault(poolID, null);
    }
}
