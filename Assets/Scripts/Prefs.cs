using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace CrazyChipmunk
{
    [CreateAssetMenu(menuName = "CrazyChipmunk/Prefs")]
    public class Prefs : ScriptableObject
    {
        [SerializeField] private ReadOnlyString playerName;

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(GetKey(key));
        }

        public void DeleteKey(string key)
        {
            var arrayKey = GetArrayKey(key);
            if (arrayKey.Length > 0)
            {
                // is array 
                var lengthKey = GetLengthKey(arrayKey);
                var length = PlayerPrefs.GetInt(lengthKey);
                var arrayKeyWithPlayer = GetKey(arrayKey);
                for (var i = 0; i < length; ++i) PlayerPrefs.DeleteKey(arrayKeyWithPlayer + ":" + i);
                PlayerPrefs.DeleteKey(lengthKey);
            }
            else
            {
                PlayerPrefs.DeleteKey(GetKey(key));
            }
        }

        public int GetInt(string key, int defaultValue)
        {
            return PlayerPrefs.GetInt(GetKey(key), defaultValue);
        }

        public void SetInt(string key, int i)
        {
            PlayerPrefs.SetInt(GetKey(key), i);
        }

        public float GetFloat(string key, float defaultValue)
        {
            return PlayerPrefs.GetFloat(GetKey(key), defaultValue);
        }

        public void SetFloat(string key, float f)
        {
            PlayerPrefs.SetFloat(GetKey(key), f);
        }

        public string GetString(string key, string defaultValue = default)
        {
            return PlayerPrefs.GetString(GetKey(key), defaultValue);
        }

        public void SetString(string key, string s)
        {
            PlayerPrefs.SetString(GetKey(key), s);
        }

        public bool GetBool(string key, bool defaultValue = default)
        {
            return PlayerPrefsUtility.GetBool(GetKey(key), defaultValue);
        }

        public void SetBool(string key, bool b)
        {
            PlayerPrefsUtility.SetBool(GetKey(key), b);
        }

        public DateTime GetDateTime(string key, DateTime defaultValue)
        {
            return PlayerPrefsUtility.GetDateTime(GetKey(key), defaultValue);
        }

        public void SetDateTime(string key, DateTime dateTime)
        {
            PlayerPrefsUtility.SetDateTime(GetKey(key), dateTime);
        }

        public Color GetColor(string key, Color defaultValue = default)
        {
            key = GetKey(key);
            Color color;
            color.r = PlayerPrefs.GetFloat(key + ".r", defaultValue.r);
            color.g = PlayerPrefs.GetFloat(key + ".g", defaultValue.g);
            color.b = PlayerPrefs.GetFloat(key + ".b", defaultValue.b);
            color.a = PlayerPrefs.GetFloat(key + ".a", defaultValue.a);
            return color;
        }

        public void SetColor(string key, Color color)
        {
            key = GetKey(key);
            PlayerPrefs.SetFloat(key + ".r", color.r);
            PlayerPrefs.SetFloat(key + ".g", color.g);
            PlayerPrefs.SetFloat(key + ".b", color.b);
            PlayerPrefs.SetFloat(key + ".a", color.a);
        }

        public void SetIntArray(string key, int[] value)
        {
            key += ":IntArray";
            SetLength(key, value.Length);
            for (var i = 0; i < value.Length; ++i) SetInt(key + ":" + i, value[i]);
        }

        public int[] GetIntArray(string key, int dfault = default)
        {
            key += ":IntArray";
            var length = GetLength(key);
            var returns = new int[length];
            for (var i = 0; i < length; ++i) returns.SetValue(GetInt(key + ":" + i, dfault), i);
            return returns;
        }

        public void SetFloatArray(string key, float[] value)
        {
            key += ":FloatArray";
            SetLength(key, value.Length);
            for (var i = 0; i < value.Length; ++i) SetFloat(key + ":" + i, value[i]);
        }

        // Get an array of floats
        public float[] GetFloatArray(string key)
        {
            key += ":FloatArray";
            var length = GetLength(key);
            var returns = new float[length];
            for (var i = 0; i < length; ++i) returns.SetValue(GetFloat(key + ":" + i, default), i);
            return returns;
        }

        public string[] GetStringArray(string key)
        {
            key += ":StringArray";
            var length = GetLength(key);
            var returns = new string[length];
            for (var i = 0; i < length; ++i) returns.SetValue(GetString(key + ":" + i), i);
            return returns;
        }

        public void SetStringArray(string key, string[] value)
        {
            key += ":StringArray";
            SetLength(key, value.Length);
            for (var i = 0; i < value.Length; ++i) SetString(key + ":" + i, value[i]);
        }

        public void SetBoolArray(string key, bool[] value)
        {
            key += ":BoolArray";
            SetLength(key, value.Length);
            for (var i = 0; i < value.Length; ++i) SetBool(key + ":" + i, value[i]);
        }

        public bool[] GetBoolArray(string key)
        {
            key += ":BoolArray";
            var length = GetLength(key);
            var returns = new bool[length];
            for (var i = 0; i < length; ++i) returns.SetValue(GetBool(key + ":" + i), i);
            return returns;
        }

        private string GetArrayKey(string key)
        {
            var arrayKey = key + ":IntArray";
            if (PlayerPrefs.HasKey(GetLengthKey(arrayKey))) return arrayKey;
            arrayKey = key + ":FloatArray";
            if (PlayerPrefs.HasKey(GetLengthKey(arrayKey))) return arrayKey;
            arrayKey = key + ":StringArray";
            if (PlayerPrefs.HasKey(GetLengthKey(arrayKey))) return arrayKey;
            return "";
        }

        private string GetKey(string key)
        {
            Assert.IsTrue(playerName?.Get()?.Length > 0);
            return playerName + ":" + key;
        }

        private string GetLengthKey(string key)
        {
            return GetKey(key) + ":ArrayLen";
        }

        private int GetLength(string key)
        {
            return PlayerPrefs.GetInt(GetLengthKey(key));
        }

        private void SetLength(string key, int len)
        {
            PlayerPrefs.SetInt(GetLengthKey(key), len);
        }
    }
}