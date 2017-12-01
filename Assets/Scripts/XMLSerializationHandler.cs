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
			var data = new SerializableGameData();
			data.testInt = 3;
			var serializer = new XmlSerializer(typeof(SerializableGameData));
			System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("UTF-8");
			using (FileStream file = File.Open (GetPath(), FileMode.Create, FileAccess.Write, FileShare.None)) {
				using(var sw = new StreamWriter(file, encoding)) {
					serializer.Serialize(sw, data);
				}
			}
		} catch (System.Exception ex) {
			Debug.Log (ex.ToString());
			throw(ex);
		}
	}

	static public SerializableGameData LoadFromFile() {
		SerializableGameData data = null;
		try {
			string path = GetPath();
			if (File.Exists(path)) {
				using (FileStream file = File.OpenRead (GetPath())) {
					XmlSerializer serializer = new XmlSerializer(typeof(SerializableGameData));


					data = serializer.Deserialize(file) as SerializableGameData;
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
}

[System.Serializable]
public class SerializableGameData {
//	[XmlArray("YourReadableName")]
	public int testInt = 2;
}
