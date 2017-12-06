using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class XMLSerializationHandler  {
	const string fName = "gamedata.xml";

	static public void SaveToFile() {
		try {
			var serializer = new XmlSerializer(typeof(GameData));
			System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("UTF-8");
			using (FileStream file = File.Open (GetPath(), FileMode.Create, FileAccess.Write, FileShare.None)) {
				using(var sw = new StreamWriter(file, encoding)) {
					serializer.Serialize(sw, GetData());
				}
			}
		} catch (System.Exception ex) {
			Debug.Log (ex.ToString());
			throw(ex);
		}
	}

	static public string GetAsString() {
		var serializer = new XmlSerializer(typeof(GameData));
		using(var sw = new StringWriter()) {
			serializer.Serialize(sw, GetData());
			return sw.ToString ();
		}
	}

	static public GameData LoadFromFile() {
		GameData data = null;
		try {
			string path = GetPath();
			if (File.Exists(path)) {
				using (FileStream file = File.OpenRead (GetPath())) {
					XmlSerializer serializer = new XmlSerializer(typeof(GameData));


					data = serializer.Deserialize(file) as GameData;
				}
			}
		} catch (System.Exception ex) {
			Debug.Log (ex.ToString());
			throw(ex);
		}	
		return data;
	}


	static string GetPath() {
		return Path.Combine(Application.persistentDataPath, fName);
	}

	static GameData GetData() {
		var data = new GameData();
		data.testInt = 3;
		return data;
	}
}

