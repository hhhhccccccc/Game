using System.Collections.Generic;

public class DictAndList<TKey, TValue> where TValue : class
{
    #region 字典

    private Dictionary<TKey, TValue> Dictionary = new Dictionary<TKey, TValue>();

    public void Add(TKey key, TValue value)
    {
        Dictionary.Add(key, value);
    }

    public TValue Get(TKey key)
    {
        return Dictionary[key];
    }

    public TValue TryGetValue(TKey key)
    {
        return Dictionary.GetValueOrDefault(key, null);
    }

    public bool ContainsKey(TKey key)
    {
        return Dictionary.ContainsKey(key);
    }

    public bool TryAdd(TKey key, TValue value)
    {
        return Dictionary.TryAdd(key, value);
    }

    public void Clear()
    {
        Dictionary.Clear();
    }

    public void Remove(TKey key)
    {
        Dictionary.Remove(key);
    }

    public Dictionary<TKey, TValue> GetDictionary() => Dictionary;

    #endregion
  

    #region 列表

    private List<TKey> TempListKey = new();

    public List<TKey> GetListKey()
    {
        TempListKey.Clear();
        foreach (var (key, value) in Dictionary)
        {
            TempListKey.Add(key);
        }

        return TempListKey;
    }

    private List<TValue> TempListValue = new();

    public List<TValue> GetListValue()
    {
        TempListValue.Clear();
        foreach (var (key, value) in Dictionary)
        {
            TempListValue.Add(value);
        }

        return TempListValue;
    }

    #endregion
}
