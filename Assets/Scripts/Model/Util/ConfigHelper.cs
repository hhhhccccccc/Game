
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class ConfigHelper : SingleModel
{
    [Inject] private ConfigManager ConfigManager { get; set; }

    private List<CommonPoolData> TempCommonPoolOriginList = new();
    private List<int> TempPoolWeightList = new();
    
    public List<CommonPoolData> RandomCommonPool(int poolID)
    {
        if (poolID == 0)
        {
            return new List<CommonPoolData>();
        }
        
        var config = ConfigManager.GetCommonPoolConfig(poolID);
        TempCommonPoolOriginList.Clear();
        foreach (var data in config.Pool)
        {
            TempCommonPoolOriginList.Add(data);
            TempPoolWeightList.Add(data.Weight);
        }
        
        return Util.GetRandomNoSame(TempCommonPoolOriginList, TempPoolWeightList, config.Count);
    }
}
