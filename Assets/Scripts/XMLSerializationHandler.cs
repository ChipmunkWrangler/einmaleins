using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public static class XMLSerializationHandler
{
    const string FileName = "gamedata.xml";

	public static void SaveToFile ()
	{
		try {
			var serializer = new XmlSerializer (typeof(GameData));
			System.Text.Encoding encoding = System.Text.Encoding.GetEncoding ("UTF-8");
			using (FileStream file = File.Open (GetPath (), FileMode.Create, FileAccess.Write, FileShare.None)) {
				using (var sw = new StreamWriter (file, encoding)) {
					serializer.Serialize (sw, GetData ());
				}
			}
		} catch (System.Exception ex) {
			Debug.Log (ex.ToString ());
			throw(ex);
		}
	}

	public static string GetAsString ()
	{
		var serializer = new XmlSerializer (typeof(GameData));
		using (var sw = new StringWriter ()) {
			serializer.Serialize (sw, GetData ());
			return sw.ToString ();
		}
	}

	public static GameData LoadFromFile ()
	{
		GameData data = null;
		try {
			string path = GetPath ();
			if (File.Exists (path)) {
				using (FileStream file = File.OpenRead (GetPath ())) {
					XmlSerializer serializer = new XmlSerializer (typeof(GameData));
					data = serializer.Deserialize (file) as GameData;
					data.Save ();
				}
			}
		} catch (System.Exception ex) {
			Debug.Log (ex.ToString ());
			throw(ex);
		}	
		return data;
	}

	public static void LoadFromString (string xml)
	{
		try {
			var reader = new StringReader (xml);
			var serializer = new XmlSerializer (typeof(GameData));
			GameData data = serializer.Deserialize (reader) as GameData;
			data.Save ();
		} catch (System.Exception ex) {
			Debug.Log (ex.ToString ());
			throw(ex);
		}	
	}

	static string GetPath ()
	{
		return Path.Combine (Application.persistentDataPath, FileName);
	}

	static GameData GetData ()
	{
		var data = new GameData ();
		data.Load ();
		return data;
	}
}