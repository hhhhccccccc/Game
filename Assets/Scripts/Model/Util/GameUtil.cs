
using System.Collections.Generic;
using System.Reflection;

public static class GameUtil
{
    private static Dictionary<string, Assembly> AssemblyDict = new();
    
    public static Assembly GetAssembly(string assName)
    {
        if (!AssemblyDict.TryGetValue(assName, out var assembly))
        {
            assembly = Assembly.Load($"Game.{assName}");
            AssemblyDict.Add(assName, assembly);
        }
    
        return assembly;
    }
    
}
