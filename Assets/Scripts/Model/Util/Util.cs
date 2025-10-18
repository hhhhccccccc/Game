
using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Util
{
    public static List<BattleKeyType> KeyList = new()
    {
        BattleKeyType.KeyUp,
        BattleKeyType.KeyDown,
        BattleKeyType.KeyLeft,
        BattleKeyType.KeyRight,
    };

    public static List<BattleKeyType> KeyList_NotHasUp = new()
    {
        BattleKeyType.KeyDown,
        BattleKeyType.KeyLeft,
        BattleKeyType.KeyRight,
    };
    
    public static List<BattleKeyType> KeyList_NotHasDown = new()
    {
        BattleKeyType.KeyUp,
        BattleKeyType.KeyLeft,
        BattleKeyType.KeyRight,
    };
    
    public static List<BattleKeyType> KeyList_NotHasLeft = new()
    {  
        BattleKeyType.KeyUp,
        BattleKeyType.KeyDown,
        BattleKeyType.KeyRight,
    };
    
    public static List<BattleKeyType> KeyList_NotHasRight = new()
    {
        BattleKeyType.KeyUp,
        BattleKeyType.KeyDown,
        BattleKeyType.KeyLeft,
    };
    public static int GetRandomInt(int min, int max) => Random.Range(min, max);
    public static float GetRandomFloat(float min, float max) => Random.Range(min, max);
    public static List<BattleKeyType> GetRandomKey(int count, int ignoreKeyType = 0)
    {
        var list = KeyList;
        if (ignoreKeyType == 1)
        {
            list = KeyList_NotHasUp;
        }
        if (ignoreKeyType == 2)
        {
            list = KeyList_NotHasDown;
        }
        if (ignoreKeyType == 3)
        {
            list = KeyList_NotHasLeft;
        }
        if (ignoreKeyType == 4)
        {
            list = KeyList_NotHasRight;
        }
        var result = new List<BattleKeyType>();
        for (int i = 0; i < count; i++)
        {
            var key = GetRandom<BattleKeyType>(list);
            result.Add(key);
        }

        return result;
    }

    public static T GetRandom<T>(List<T> list)
    {
        var count = list.Count;
        var index = UnityEngine.Random.Range(0,  count);
        return list[index];
    }

    public static Dictionary<int, int> KeyListToDictionary(List<int> keyList)
    {
        var result = new Dictionary<int, int>();
        foreach (var key in keyList)
        {
            if (!result.TryAdd(key, 1))
            {
                result[key]++;
            }
        }

        return result;
    }

    /// <summary>
    /// 权重
    /// </summary>
    /// <param name="list"></param>
    /// <param name="weightList"></param>
    /// <param name="index"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetRandom<T>(List<T> list, List<int> weightList, out int index)
    {
        var sum = weightList.Sum();
        var result = Random.Range(0,  sum);
        var temp = 0;
        for (int i = 0; i < weightList.Count; i++)
        {
            if (result <= weightList[i] + temp)
            {
                index = i;
                return list[i];
            }

            temp += weightList[i];
        }

        index = list.Count - 1;
        return list[^1];
    }

    /// <summary>
    /// count = 0 返回全部
    /// </summary>
    /// <param name="list"></param>
    /// <param name="weightList"></param>
    /// <param name="count"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<T> GetRandomNoSame<T>(List<T> list, List<int> weightList, int count)
    {
        var result = new List<T>();
        if (count == 0)
        {
            result.AddRange(list);
            return list;
        }
        
        for (int i = 0; i < count; i++)
        {
            var config = GetRandom(list, weightList, out var index);
            result.Add(config);
            list.RemoveAt(index);
            weightList.RemoveAt(index);
            if (list.Count == 0)
            {
                return result;
            }
        }

        return result;
    }

    public static List<int> GetSameChanceList(int count)
    {
        var result = new List<int>();
        for (int i = 0; i < count; i++)
        {
            result.Add(10);
        }

        return result;
    }
}
