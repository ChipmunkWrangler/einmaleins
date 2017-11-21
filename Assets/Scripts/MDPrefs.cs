using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MDPrefs {
	public static bool HasKey(string key) {
		return PlayerPrefs.HasKey (GetKey (key));
	}

	public static void DeleteKey(string key) { 
		string arrayKey = GetArrayKey (key);
		if (arrayKey.Length > 0) { // is array 
			string lengthKey = GetLengthKey(arrayKey);
			int length = PlayerPrefs.GetInt (lengthKey);
			string arrayKeyWithPlayer = GetKey (arrayKey);
			for (int i = 0; i < length; ++i) {
				PlayerPrefs.DeleteKey (arrayKeyWithPlayer + ":" + i);
			}
			PlayerPrefs.DeleteKey (lengthKey);
		} else {
			PlayerPrefs.DeleteKey (GetKey (key));
		}
	}

	static string GetArrayKey(string key) {
		string arrayKey = key + ":IntArray";
		if (PlayerPrefs.HasKey (GetLengthKey(arrayKey))) {
			return arrayKey;
		}
		arrayKey = key + ":FloatArray";
		if (PlayerPrefs.HasKey (GetLengthKey(arrayKey))) {
			return arrayKey;
		}
		arrayKey = key + ":StringArray";
		if (PlayerPrefs.HasKey (GetLengthKey(arrayKey))) {
			return arrayKey;
		}
		return "";
	}

	public static int GetInt(string key, int defaultValue) {
		return PlayerPrefs.GetInt (GetKey(key), defaultValue);
	}
	public static void SetInt(string key, int i) {
		PlayerPrefs.SetInt (GetKey(key), i);
	}
	public static float GetFloat(string key, float defaultValue) {
		return PlayerPrefs.GetFloat (GetKey(key), defaultValue);
	}
	public static void SetFloat(string key, float f) {
		PlayerPrefs.SetFloat (GetKey(key), f);
	}
	public static string GetString(string key, string defaultValue = default(string)) {
		return PlayerPrefs.GetString (GetKey(key), defaultValue);
	}
	public static void SetString(string key, string s) {
		PlayerPrefs.SetString (GetKey(key), s);
	}
	public static bool GetBool(string key, bool defaultValue = default(bool)) {
		return PlayerPrefsUtility.GetBool (GetKey(key), defaultValue);
	}
	public static void SetBool(string key, bool b) {
		PlayerPrefsUtility.SetBool (GetKey(key), b);
	}

	public static System.DateTime GetDateTime(string key, System.DateTime defaultValue) {
		return PlayerPrefsUtility.GetDateTime (GetKey(key), defaultValue);
	}
	public static void SetDateTime(string key, System.DateTime dateTime) {
		PlayerPrefsUtility.SetDateTime (GetKey(key), dateTime);
	}
	public static Color GetColor(string key, Color defaultValue = default(Color)) {
		key = GetKey (key);
		Color color;
		color.r = PlayerPrefs.GetFloat (key + ".r", defaultValue.r);
		color.g = PlayerPrefs.GetFloat (key + ".g", defaultValue.g);
		color.b = PlayerPrefs.GetFloat (key + ".b", defaultValue.b);
		color.a = PlayerPrefs.GetFloat (key + ".a", defaultValue.a);
		return color;
	}
	public static void SetColor(string key, Color color) {
		key = GetKey (key);
		PlayerPrefs.SetFloat (key + ".r", color.r);
		PlayerPrefs.SetFloat (key + ".g", color.g);
		PlayerPrefs.SetFloat (key + ".b", color.b);
		PlayerPrefs.SetFloat (key + ".a", color.a);
	}
	public static void SetIntArray( string key, int[] value ){
		key += ":IntArray";
		SetLength (key, value.Length);
		for(int i = 0; i < value.Length; ++i){
			MDPrefs.SetInt(key + ":" + i.ToString(), value[i]);
		}
	}

	public static int[] GetIntArray( string key, int dfault = default(int) ){
		key += ":IntArray";
		int length = GetLength (key);
		int[] returns = new int[length];
		for(int i = 0; i < length; ++i) {
			returns.SetValue(MDPrefs.GetInt(key + ":" + i, dfault), i);
		}
		return returns;
	}

	public static void SetFloatArray( string key, float[] value ){
		key += ":FloatArray";
		SetLength (key, value.Length);
		for(int i = 0; i < value.Length; ++i){
			MDPrefs.SetFloat(key + ":" + i.ToString(), value[i]);
		}
	}

	//Get an array of floats
	public static float[] GetFloatArray( string key ){
		key += ":FloatArray";
		int length = GetLength (key);
		float[] returns = new float[length];
		for(int i = 0; i < length; ++i) {
			returns.SetValue(MDPrefs.GetFloat(key + ":" + i, default(float)), i);
		}
		return returns;
	}

	public static string[] GetStringArray( string key ){
		key += ":StringArray";
		int length = GetLength (key);
		string[] returns = new string[length];
		for(int i = 0; i < length; ++i) {
			returns.SetValue(MDPrefs.GetString(key + ":" + i), i);
		}
		return returns;
	}

	public static void SetStringArray( string key, string[] value ){
		key += ":StringArray";
		SetLength (key, value.Length);
		for(int i = 0; i < value.Length; ++i){
			MDPrefs.SetString(key + ":" + i.ToString(), value[i]);
		}
	}

	public static void SetBoolArray( string key, bool[] value ){
		key += ":BoolArray";
		SetLength (key, value.Length);
		for(int i = 0; i < value.Length; ++i){
			MDPrefs.SetBool(key + ":" + i, value[i]);
		}
	}

	public static bool[] GetBoolArray( string key ){
		key += ":BoolArray";
		int length = GetLength (key);
		bool[] returns = new bool[length];
		for(int i = 0; i < length; ++i) {
			returns.SetValue(MDPrefs.GetBool(key + ":" + i), i);
		}
		return returns;
	}

	static string GetKey(string key) {
		UnityEngine.Assertions.Assert.IsTrue (PlayerPrefs.HasKey ("curPlayer") && PlayerPrefs.GetString ("curPlayer").Length != 0);
		return PlayerPrefs.GetString ("curPlayer") + ":" + key;
	}

	static string GetLengthKey(string key) { 
		return GetKey(key) + ":ArrayLen";
	}

	static int GetLength(string key) {
		return PlayerPrefs.GetInt (GetLengthKey(key));
	}

	static void SetLength (string key, int len)
	{
		PlayerPrefs.SetInt (GetLengthKey (key), len);
	}
}
