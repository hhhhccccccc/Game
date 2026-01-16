
using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Util 
{
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
