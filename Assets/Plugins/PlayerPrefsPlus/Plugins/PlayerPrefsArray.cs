using UnityEngine;
using System.Collections;

public class PlayerPrefsArray : MonoBehaviour {
	
	//############################################### int ##############################################
	
	//Set an array of ints
	public static void SetIntArray( string key, int[] value ){
		key += ":IntArray";
		SetLength (key, value.Length);
		for(int i = 0; i < value.Length; ++i){
			PlayerPrefs.SetInt(key + ":" + i.ToString(), value[i]);
		}
	}
	
	//Get an array of ints
	public static int[] GetIntArray( string key ){
		key += ":IntArray";
		int length = GetLength (key);
		int[] returns = new int[length];
		for(int i = 0; i < length; ++i) {
			returns.SetValue(PlayerPrefs.GetInt(key + ":" + i.ToString()), i);
		}
		return returns;
	}
	
	//############################################### float ##############################################
	
	//Set an array of floats
	public static void SetFloatArray( string key, float[] value ){
		key += ":FloatArray";
		SetLength (key, value.Length);
		for(int i = 0; i < value.Length; ++i){
			PlayerPrefs.SetFloat(key + ":" + i.ToString(), value[i]);
	    }
	}
	
	//Get an array of floats
	public static float[] GetFloatArray( string key ){
		key += ":FloatArray";
		int length = GetLength (key);
	    float[] returns = new float[length];
		for(int i = 0; i < length; ++i) {
			returns.SetValue(PlayerPrefs.GetFloat(key + ":" + i.ToString()), i);
	    }
	    return returns;
	}
	
	//############################################### String ##############################################
	
	//Set an array of strings
	public static string[] GetStringArray( string key ){
		key += ":StringArray";
		int length = GetLength (key);
		string[] returns = new string[length];
		for(int i = 0; i < length; ++i) {
			returns.SetValue(PlayerPrefs.GetString(key + ":" + i.ToString()), i);
		}
		return returns;
	}

	//Set an array of strings
	public static void SetStringArray( string key, string[] value ){
		key += ":StringArray";
		SetLength (key, value.Length);
		for(int i = 0; i < value.Length; ++i){
			PlayerPrefs.SetString(key + ":" + i.ToString(), value[i]);
		}
	}
	
	//############################################### bool ##############################################
	
	//Set an array of bool
	public static void SetBoolArray( string key, bool[] value ){
		key += ":BoolArray";
		SetLength (key, value.Length);
		for(int i = 0; i < value.Length; ++i){
			PlayerPrefsPlus.SetBool(key + ":" + i.ToString(), value[i]);
		}
	}
	
	//Get an array of bools
	public static bool[] GetBoolArray( string key ){
		key += ":BoolArray";
		int length = GetLength (key);
		bool[] returns = new bool[length];
		for(int i = 0; i < length; ++i) {
			returns.SetValue(PlayerPrefsPlus.GetBool(key + ":" + i.ToString()), i);
		}
		return returns;
	}
	
	//############################################### Color ##############################################
	
	//Set an array of Colours
	public static void SetColourArray( string key, Color[] value ){
	    PlayerPrefs.SetInt("PlayerPrefsArray:Colour:L:"+key, value.Length);
	    int i = 0;
	    while(i < value.Length){
	        PlayerPrefsPlus.SetColour("PlayerPrefsArray:Colour:"+key + i.ToString(), value[i]);
	        ++i;
	    }
	}
	
	//Get an array of Colours
	public static Color[] GetColourArray( string key ){
	    Color[] returns = new Color[PlayerPrefs.GetInt("PlayerPrefsArray:Colour:L:"+key)];
	    
	   	int i = 0;
	    
	    while(i < PlayerPrefs.GetInt("PlayerPrefsArray:Colour:L:"+key)){
	        returns.SetValue(PlayerPrefsPlus.GetColour("PlayerPrefsArray:Colour:"+key + i.ToString()), i);
	        ++i;
	    }
	    return returns;
	}
	
	//############################################### Color32 ##############################################
	
	//Set an array of Colour32s
	public static void SetColour32Array( string key, Color32[] value ){
	    PlayerPrefs.SetInt("PlayerPrefsArray:Colour32:L:"+key, value.Length);
	    int i = 0;
	    while(i < value.Length){
	        PlayerPrefsPlus.SetColour32("PlayerPrefsArray:Colour32:"+key + i.ToString(), value[i]);
	        ++i;
	    }
	}
	
	//Get an array of Colour32s
	public static Color32[] GetColour32Array( string key ){
	    Color32[] returns = new Color32[PlayerPrefs.GetInt("PlayerPrefsArray:Colour32:L:"+key)];
	    
	   	int i = 0;
	    
	    while(i < PlayerPrefs.GetInt("PlayerPrefsArray:Colour32:L:"+key)){
	        returns.SetValue(PlayerPrefsPlus.GetColour32("PlayerPrefsArray:Colour32:"+key + i.ToString()), i);
	        ++i;
	    }
	    return returns;
	}
	
	//############################################### Vector2 ##############################################
	
	//Set an array of Vector2s
	public static void SetVector2Array( string key, Vector2[] value ){
	    PlayerPrefs.SetInt("PlayerPrefsArray:Vector2:L:"+key, value.Length);
	    int i = 0;
	    while(i < value.Length){
	        PlayerPrefsPlus.SetVector2("PlayerPrefsArray:Vector2:"+key + i.ToString(), value[i]);
	        ++i;
	    }
	}
	
	//Get an array of Vector2s
	public static Vector2[] GetVector2Array( string key ){
	    Vector2[] returns = new Vector2[PlayerPrefs.GetInt("PlayerPrefsArray:Vector2:L:"+key)];
	    
	   	int i = 0;
	    
	    while(i < PlayerPrefs.GetInt("PlayerPrefsArray:Vector2:L:"+key)){
	        returns.SetValue(PlayerPrefsPlus.GetVector2("PlayerPrefsArray:Vector2:"+key + i.ToString()), i);
	        ++i;
	    }
	    return returns;
	}
	
	//############################################### Vector3 ##############################################
	
	//Set an array of Vector3s
	public static void SetVector3Array( string key, Vector3[] value ){
	    PlayerPrefs.SetInt("PlayerPrefsArray:Vector3:L:"+key, value.Length);
	    int i = 0;
	    while(i < value.Length){
	        PlayerPrefsPlus.SetVector3("PlayerPrefsArray:Vector3:"+key + i.ToString(), value[i]);
	        ++i;
	    }
	}
	
	//Get an array of Vector3s
	public static Vector3[] GetVector3Array( string key ){
	    Vector3[] returns = new Vector3[PlayerPrefs.GetInt("PlayerPrefsArray:Vector3:L:"+key)];
	    
	   	int i = 0;
	    
	    while(i < PlayerPrefs.GetInt("PlayerPrefsArray:Vector3:L:"+key)){
	        returns.SetValue(PlayerPrefsPlus.GetVector3("PlayerPrefsArray:Vector3:"+key + i.ToString()), i);
	        ++i;
	    }
	    return returns;
	}
	
	//############################################### Vector4 ##############################################
	
	//Set an array of Vector4s
	public static void SetVector4Array( string key, Vector4[] value ){
	    PlayerPrefs.SetInt("PlayerPrefsArray:Vector4:L:"+key, value.Length);
	    int i = 0;
	    while(i < value.Length){
	        PlayerPrefsPlus.SetVector4("PlayerPrefsArray:Vector4:"+key + i.ToString(), value[i]);
	        ++i;
	    }
	}
	
	//Get an array of Vector4s
	public static Vector4[] GetVector4Array( string key ){
	    Vector4[] returns = new Vector4[PlayerPrefs.GetInt("PlayerPrefsArray:Vector4:L:"+key)];
	    
	   	int i = 0;
	    
	    while(i < PlayerPrefs.GetInt("PlayerPrefsArray:Vector4:L:"+key)){
	        returns.SetValue(PlayerPrefsPlus.GetVector4("PlayerPrefsArray:Vector4:"+key + i.ToString()), i);
	        ++i;
	    }
	    return returns;
	}
	
	//############################################### Quaternion ##############################################
	
	//Set an array of Quaternions
	public static void SetQuaternionArray( string key, Quaternion[] value ){
	    PlayerPrefs.SetInt("PlayerPrefsArray:Quaternion:L:"+key, value.Length);
	    int i = 0;
	    while(i < value.Length){
	        PlayerPrefsPlus.SetQuaternion("PlayerPrefsArray:Quaternion:"+key + i.ToString(), value[i]);
	        ++i;
	    }
	}
	
	//Get an array of Quaternions
	public static Quaternion[] GetQuaternionArray( string key ){
	    Quaternion[] returns = new Quaternion[PlayerPrefs.GetInt("PlayerPrefsArray:Quaternion:L:"+key)];
	    
	   	int i = 0;
	    
	    while(i < PlayerPrefs.GetInt("PlayerPrefsArray:Quaternion:L:"+key)){
	        returns.SetValue(PlayerPrefsPlus.GetQuaternion("PlayerPrefsArray:Quaternion:"+key + i.ToString()), i);
	        ++i;
	    }
	    return returns;
	}
	
	//############################################### Rect ##############################################
	
	//Set an array of Rects
	public static void SetRectArray( string key, Rect[] value ){
	    PlayerPrefs.SetInt("PlayerPrefsArray:Rect:L:"+key, value.Length);
	    int i = 0;
	    while(i < value.Length){
	        PlayerPrefsPlus.SetRect("PlayerPrefsArray:Rect:"+key + i.ToString(), value[i]);
	        ++i;
	    }
	}
	
	//Get an array of Rects
	public static Rect[] GetRectArray( string key ){
	    Rect[] returns = new Rect[PlayerPrefs.GetInt("PlayerPrefsArray:Rect:L:"+key)];
	    
	   	int i = 0;
	    
	    while(i < PlayerPrefs.GetInt("PlayerPrefsArray:Rect:L:"+key)){
	        returns.SetValue(PlayerPrefsPlus.GetRect("PlayerPrefsArray:Rect:"+key + i.ToString()), i);
	        ++i;
	    }
	    return returns;
	}

	static string GetLengthKey(string key) { 
		return key + ":ArrayLen";
	}

	static int GetLength(string key) {
		return PlayerPrefs.GetInt (GetLengthKey(key));
	}
		

	static void SetLength (string key, int len)
	{
		PlayerPrefs.SetInt (GetLengthKey (key), len);
	}
}

public class PPA : PlayerPrefsArray{}