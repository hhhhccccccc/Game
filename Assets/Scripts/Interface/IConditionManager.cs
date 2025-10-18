using System.Collections.Generic;

public interface IConditionManager : IManager
{
    bool Check(int conditionID, List<int> conditionParam);
}
