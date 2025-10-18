using System;
using UnityEngine;

public class LogManager : ManagerBase, ILogManager
{
    private bool IgnoreLog = false;

    public void Debug(string msg)
    {
        if(IgnoreLog)return;
        UnityEngine.Debug.Log(msg);
    }
    
    public void Error(string msg)
    {
        if(IgnoreLog)return;
        UnityEngine.Debug.LogError(msg);
    }
    
    public void Error(Exception e)
    {
        if (e.Data.Contains("StackTrace"))
        {
            Error($"{e.Data["StackTrace"]}\n{e}");
            return;
        }
        string str = e.ToString();
        Error(str);
    }
}