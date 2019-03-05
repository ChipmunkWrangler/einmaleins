using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

internal static class XMLSerializationHandler
{
    private const string FileName = "gamedata.xml";

    public static void SaveToFile()
    {
        try
        {
            var serializer = new XmlSerializer(typeof(GameData));
            var encoding = Encoding.GetEncoding("UTF-8");
            using (var file = File.Open(GetPath(), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var sw = new StreamWriter(file, encoding))
                {
                    serializer.Serialize(sw, GetData());
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ExceptionPrettyPrint.Msg(ex));
            throw;
        }
    }

    public static string GetAsString()
    {
        var serializer = new XmlSerializer(typeof(GameData));
        using (var sw = new StringWriter())
        {
            serializer.Serialize(sw, GetData());
            return sw.ToString();
        }
    }

    public static GameData LoadFromFile()
    {
        GameData data = null;
        try
        {
            var path = GetPath();
            if (File.Exists(path))
                using (var file = File.OpenRead(GetPath()))
                {
                    var serializer = new XmlSerializer(typeof(GameData));
                    data = (GameData) serializer.Deserialize(file);
                    data.Save();
                }
        }
        catch (Exception ex)
        {
            Debug.Log(ExceptionPrettyPrint.Msg(ex));
            throw;
        }

        return data;
    }

    public static void LoadFromString(string xml)
    {
        try
        {
            var reader = new StringReader(xml);
            var serializer = new XmlSerializer(typeof(GameData));
            var data = (GameData) serializer.Deserialize(reader);
            data.Save();
        }
        catch (Exception ex)
        {
            Debug.Log(ExceptionPrettyPrint.Msg(ex));
            throw;
        }
    }

    private static string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, FileName);
    }

    private static GameData GetData()
    {
        var data = new GameData();
        data.Load();
        return data;
    }
}