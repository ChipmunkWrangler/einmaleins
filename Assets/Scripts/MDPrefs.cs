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
	public static int GetInt(string key, int defaultValue) {
		return PlayerPrefs.GetInt (GetKey(key), defaultValue);
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

	public static System.DateTime GetDateTime(string key, System.DateTime defaultValue) {
		return PlayerPrefsUtility.GetDateTime (GetKey(key), defaultValue);
	}
	public static void SetDateTime(string key, System.DateTime dateTime) {
		PlayerPrefsUtility.SetDateTime (GetKey(key), dateTime);
	}

	static string GetKey(string key) {
		UnityEngine.Assertions.Assert.IsTrue (PlayerPrefs.HasKey ("curPlayer") && PlayerPrefs.GetString ("curPlayer").Length != 0);
		return PlayerPrefs.GetString ("curPlayer") + ":" + key;
	}
}
