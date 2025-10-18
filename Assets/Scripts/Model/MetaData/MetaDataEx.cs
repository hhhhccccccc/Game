
using System;
using System.Collections.Generic;

public static class MetaDataEx
{
    public static int ToInt(this float self)
    {
        return (int)self;
    }

    public static List<T> Clone<T>(this List<T> list)
    {
        var cloneList = new List<T>();
        foreach (T value in list)
        {
            cloneList.Add(value);
        }

        return cloneList;
    }
    
    public static int ToRound(this float f)
    {
        return (int)Math.Round(f);
    }
}
