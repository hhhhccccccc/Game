using System;
using UnityEngine;

public class ArchiveManager : ManagerBase, IArchiveManager
{
    public void Save(string key, object value)
    {
        ES3.Save(key, value);
    }

    public object Load(string key, object instance)
    {
        return ES3.Load(key, instance);
    }
}