using System;
using System.Data;
using System.IO;
using System.Text;
using Excel;
using UnityEditor;
using UnityEngine;

public class ExcelTool
{
    public static readonly string EXCEL_PATH = Application.dataPath + "/Resources/Excel/";
    public static readonly string DATA_CLASS_PATH = Application.dataPath + "/Scripts/ExcelData/DataClass/";
    public static readonly string DATA_CONTAINER_PATH = Application.dataPath + "/Scripts/ExcelData/Container/";
    public static readonly int BEGIN_INDEX = 3;

    [MenuItem("GameTool/GenerateExcel")]
    private static void GenerateExcelInfo()
    {
        DirectoryInfo dInfo = Directory.CreateDirectory(EXCEL_PATH);
        FileInfo[] files = dInfo.GetFiles();
        DataTableCollection tableCollection;
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Extension != ".xlsx" &&
                files[i].Extension != ".xls")
                continue;

            using (FileStream fs = files[i].Open(FileMode.Open, FileAccess.Read))
            {
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                tableCollection = excelReader.AsDataSet().Tables;
                fs.Close();
            }

            foreach (DataTable table in tableCollection)
            {
                GenerateExcelDataClass(table);
                GenerateExcelContainer(table);
                GenerateExcelBinary(table);
            }
        }
    }

    private static void GenerateExcelDataClass(DataTable table)
    {
        DataRow rowName = GetVariableNameRow(table);
        DataRow rowType = GetVariableTypeRow(table);
        if (!Directory.Exists(DATA_CLASS_PATH))
            Directory.CreateDirectory(DATA_CLASS_PATH);

        string str = "public class " + table.TableName + "\n{\n";
        for (int i = 0; i < table.Columns.Count; i++)
            str += "    public " + rowType[i] + " " + rowName[i] + ";\n";
        str += "}";
        File.WriteAllText(DATA_CLASS_PATH + table.TableName + ".cs", str);
        AssetDatabase.Refresh();
    }

    private static void GenerateExcelContainer(DataTable table)
    {
        DataRow rowType = GetVariableTypeRow(table);
        if (!Directory.Exists(DATA_CONTAINER_PATH))
            Directory.CreateDirectory(DATA_CONTAINER_PATH);
        string str = $@"using System.Collections.Generic;
public class {table.TableName}Container
{{
    public Dictionary<{rowType[0]}, {table.TableName}> dataDic = new();
}}";
        File.WriteAllText(DATA_CONTAINER_PATH + table.TableName + "Container.cs", str);
        AssetDatabase.Refresh();
    }

    private static void GenerateExcelBinary(DataTable table)
    {
        if (!Directory.Exists(BinaryDataMgr.DATA_BINARY_PATH))
            Directory.CreateDirectory(BinaryDataMgr.DATA_BINARY_PATH);

        using (FileStream fs = new(BinaryDataMgr.DATA_BINARY_PATH + table.TableName + ".tang", FileMode.OpenOrCreate, FileAccess.Write))
        {
            fs.Write(BitConverter.GetBytes(table.Rows.Count - BEGIN_INDEX), 0, 4);
            string keyName = GetVariableNameRow(table)[0].ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(keyName);
            fs.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
            fs.Write(bytes, 0, bytes.Length);
            DataRow row;
            DataRow rowType = GetVariableTypeRow(table);
            for (int i = BEGIN_INDEX; i < table.Rows.Count; i++)
            {
                row = table.Rows[i];
                for (int j = 0; j < table.Columns.Count; j++)
                    switch (rowType[j].ToString())
                    {
                        case "int":
                            fs.Write(BitConverter.GetBytes(int.Parse(row[j].ToString())), 0, 4);
                            break;
                        case "float":
                            fs.Write(BitConverter.GetBytes(float.Parse(row[j].ToString())), 0, 4);
                            break;
                        case "bool":
                            fs.Write(BitConverter.GetBytes(bool.Parse(row[j].ToString())), 0, 1);
                            break;
                        case "string":
                            bytes = Encoding.UTF8.GetBytes(row[j].ToString());
                            fs.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
                            fs.Write(bytes, 0, bytes.Length);
                            break;
                    }
            }
            fs.Close();
        }
        AssetDatabase.Refresh();
    }

    private static DataRow GetVariableNameRow(DataTable table)
    {
        return table.Rows[0];
    }

    private static DataRow GetVariableTypeRow(DataTable table)
    {
        return table.Rows[1];
    }
}