using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class XMLSerializer  {
	const string fName = "gamedata.dat";

	public void SerializeToFile() {
		try {
			using (FileStream file = File.Open (Application.persistentDataPath + Path.DirectorySeparatorChar + fName, FileMode.Create, FileAccess.Write, FileShare.None)) {
				SerializableGameData data = new SerializableGameData();
				data.testInt = 3;
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize(file, data);
			}
		} catch (System.Exception ex) {
			Debug.Log (ex.ToString());
			throw(ex);
		}
	}
}

[Serializable]
class SerializableGameData {
	public int testInt = 2;
}
