using UnityEngine;

public class CheckTest : ICondition
{
    protected override bool OnCheck()
    {
        Debug.Log("sssss");
        return true;
    }
}
