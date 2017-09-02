using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MDPrefs {
	public static bool HasKey(string key) {
		return PlayerPrefs.HasKey (GetKey (key));
	}

	public static void DeleteKey(string key) { 
		PlayerPrefs.DeleteKey (GetKey (key));
	}
	public static int GetInt(string key) {
		return PlayerPrefs.GetInt (GetKey(key));
	}
	public static void SetInt(string key, int i) {
		PlayerPrefs.SetInt (GetKey(key), i);
	}

	public static string GetString(string key) {
		return PlayerPrefs.GetString (GetKey(key));
	}
	public static void SetString(string key, string s) {
		PlayerPrefs.SetString (GetKey(key), s);
	}

	static string GetKey(string key) {
		UnityEngine.Assertions.Assert.IsTrue (PlayerPrefs.HasKey ("curPlayer") && PlayerPrefs.GetString ("curPlayer").Length != 0);
		return PlayerPrefs.GetString ("curPlayer") + ":" + key;
	}
}
