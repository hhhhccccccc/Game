using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public class BinaryDataMgr
{
    public static readonly string DATA_BINARY_PATH = Application.streamingAssetsPath + "/Binary/";
    private readonly Dictionary<string, object> tableDic = new();
    private static readonly string SAVE_PATH = Application.persistentDataPath + "/Data/";
    public static BinaryDataMgr Instance { get; } = new();
    private bool isInit;
    private BinaryDataMgr() { }

    public void InitData()
    {
        if (isInit)
            return;
        isInit = true;
        //在此加载表格数据：
        LoadTable<CardInfoContainer, CardInfo>();
    }

    public void LoadTable<T, K>()
    {
        using FileStream fs = File.Open(DATA_BINARY_PATH + typeof(K).Name + ".tang", FileMode.Open, FileAccess.Read);
        byte[] bytes = new byte[fs.Length];
        fs.Read(bytes, 0, bytes.Length);
        fs.Close();
        int index = 0;
        int count = BitConverter.ToInt32(bytes, index);
        index += 4;
        int keyNameLength = BitConverter.ToInt32(bytes, index);
        index += 4;
        string keyName = Encoding.UTF8.GetString(bytes, index, keyNameLength);
        index += keyNameLength;
        Type containerType = typeof(T);
        object containerObj = Activator.CreateInstance(containerType);
        Type classType = typeof(K);
        FieldInfo[] infos = classType.GetFields();
        for (int i = 0; i < count; i++)
        {
            object dataObj = Activator.CreateInstance(classType);
            foreach (FieldInfo info in infos)
                if (info.FieldType == typeof(int))
                {
                    info.SetValue(dataObj, BitConverter.ToInt32(bytes, index));
                    index += 4;
                }
                else if (info.FieldType == typeof(float))
                {
                    info.SetValue(dataObj, BitConverter.ToSingle(bytes, index));
                    index += 4;
                }
                else if (info.FieldType == typeof(bool))
                {
                    info.SetValue(dataObj, BitConverter.ToBoolean(bytes, index));
                    index += 1;
                }
                else if (info.FieldType == typeof(string))
                {
                    int length = BitConverter.ToInt32(bytes, index);
                    index += 4;
                    info.SetValue(dataObj, Encoding.UTF8.GetString(bytes, index, length));
                    index += length;
                }

            object dicObject = containerType.GetField("dataDic").GetValue(containerObj);
            MethodInfo mInfo = dicObject.GetType().GetMethod("Add");
            object keyValue = infos[0].GetValue(dataObj);
            mInfo.Invoke(dicObject, new[] { keyValue, dataObj });
        }

        tableDic.Add(typeof(T).Name, containerObj);
        fs.Close();
    }

    public T GetTable<T>() where T : class
    {
        string tableName = typeof(T).Name;
        if (tableDic.TryGetValue(tableName, out object value))
            return value as T;
        return null;
    }

    public void Save(object obj, string fileName)
    {
        if (!Directory.Exists(SAVE_PATH))
            Directory.CreateDirectory(SAVE_PATH);

        using FileStream fs = new(SAVE_PATH + fileName + ".tang", FileMode.OpenOrCreate, FileAccess.Write);
        BinaryFormatter bf = new();
        bf.Serialize(fs, obj);
        fs.Close();
        Debug.Log($"游戏数据{obj}保存成功！位置在{SAVE_PATH + fileName + ".tang"}");
    }

    public T Load<T>(string fileName) where T : class
    {
        if (!File.Exists(SAVE_PATH + fileName + ".tang"))
            return null;

        using FileStream fs = File.Open(SAVE_PATH + fileName + ".tang", FileMode.Open, FileAccess.Read);
        BinaryFormatter bf = new();
        T obj = bf.Deserialize(fs) as T;
        fs.Close();
        return obj;
    }
}