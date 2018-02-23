using UnityEngine;

namespace CrazyChipmunk
{
    public static class Prefs
    {
        public static bool HasKey(string key) => PlayerPrefs.HasKey(GetKey(key));

        public static void DeleteKey(string key)
        {
            string arrayKey = GetArrayKey(key);
            if (arrayKey.Length > 0)
            { // is array 
                string lengthKey = GetLengthKey(arrayKey);
                int length = PlayerPrefs.GetInt(lengthKey);
                string arrayKeyWithPlayer = GetKey(arrayKey);
                for (int i = 0; i < length; ++i)
                {
                    PlayerPrefs.DeleteKey(arrayKeyWithPlayer + ":" + i);
                }
                PlayerPrefs.DeleteKey(lengthKey);
            }
            else
            {
                PlayerPrefs.DeleteKey(GetKey(key));
            }
        }

        public static int GetInt(string key, int defaultValue) => PlayerPrefs.GetInt(GetKey(key), defaultValue);
        public static void SetInt(string key, int i)
        {
            PlayerPrefs.SetInt(GetKey(key), i);
        }
        public static float GetFloat(string key, float defaultValue) => PlayerPrefs.GetFloat(GetKey(key), defaultValue);
        public static void SetFloat(string key, float f)
        {
            PlayerPrefs.SetFloat(GetKey(key), f);
        }
        public static string GetString(string key, string defaultValue = default(string)) => PlayerPrefs.GetString(GetKey(key), defaultValue);
        public static void SetString(string key, string s)
        {
            PlayerPrefs.SetString(GetKey(key), s);
        }
        public static bool GetBool(string key, bool defaultValue = default(bool)) => PlayerPrefsUtility.GetBool(GetKey(key), defaultValue);
        public static void SetBool(string key, bool b)
        {
            PlayerPrefsUtility.SetBool(GetKey(key), b);
        }

        public static System.DateTime GetDateTime(string key, System.DateTime defaultValue) => PlayerPrefsUtility.GetDateTime(GetKey(key), defaultValue);
        public static void SetDateTime(string key, System.DateTime dateTime)
        {
            PlayerPrefsUtility.SetDateTime(GetKey(key), dateTime);
        }
        public static Color GetColor(string key, Color defaultValue = default(Color))
        {
            key = GetKey(key);
            Color color;
            color.r = PlayerPrefs.GetFloat(key + ".r", defaultValue.r);
            color.g = PlayerPrefs.GetFloat(key + ".g", defaultValue.g);
            color.b = PlayerPrefs.GetFloat(key + ".b", defaultValue.b);
            color.a = PlayerPrefs.GetFloat(key + ".a", defaultValue.a);
            return color;
        }
        public static void SetColor(string key, Color color)
        {
            key = GetKey(key);
            PlayerPrefs.SetFloat(key + ".r", color.r);
            PlayerPrefs.SetFloat(key + ".g", color.g);
            PlayerPrefs.SetFloat(key + ".b", color.b);
            PlayerPrefs.SetFloat(key + ".a", color.a);
        }
        public static void SetIntArray(string key, int[] value)
        {
            key += ":IntArray";
            SetLength(key, value.Length);
            for (int i = 0; i < value.Length; ++i)
            {
                Prefs.SetInt(key + ":" + i.ToString(), value[i]);
            }
        }

        public static int[] GetIntArray(string key, int dfault = default(int))
        {
            key += ":IntArray";
            int length = GetLength(key);
            var returns = new int[length];
            for (int i = 0; i < length; ++i)
            {
                returns.SetValue(Prefs.GetInt(key + ":" + i, dfault), i);
            }
            return returns;
        }

        public static void SetFloatArray(string key, float[] value)
        {
            key += ":FloatArray";
            SetLength(key, value.Length);
            for (int i = 0; i < value.Length; ++i)
            {
                Prefs.SetFloat(key + ":" + i.ToString(), value[i]);
            }
        }

        // Get an array of floats
        public static float[] GetFloatArray(string key)
        {
            key += ":FloatArray";
            int length = GetLength(key);
            var returns = new float[length];
            for (int i = 0; i < length; ++i)
            {
                returns.SetValue(Prefs.GetFloat(key + ":" + i, default(float)), i);
            }
            return returns;
        }

        public static string[] GetStringArray(string key)
        {
            key += ":StringArray";
            int length = GetLength(key);
            var returns = new string[length];
            for (int i = 0; i < length; ++i)
            {
                returns.SetValue(Prefs.GetString(key + ":" + i), i);
            }
            return returns;
        }

        public static void SetStringArray(string key, string[] value)
        {
            key += ":StringArray";
            SetLength(key, value.Length);
            for (int i = 0; i < value.Length; ++i)
            {
                Prefs.SetString(key + ":" + i.ToString(), value[i]);
            }
        }

        public static void SetBoolArray(string key, bool[] value)
        {
            key += ":BoolArray";
            SetLength(key, value.Length);
            for (int i = 0; i < value.Length; ++i)
            {
                Prefs.SetBool(key + ":" + i, value[i]);
            }
        }

        public static bool[] GetBoolArray(string key)
        {
            key += ":BoolArray";
            int length = GetLength(key);
            var returns = new bool[length];
            for (int i = 0; i < length; ++i)
            {
                returns.SetValue(Prefs.GetBool(key + ":" + i), i);
            }
            return returns;
        }

        static string GetArrayKey(string key)
        {
            string arrayKey = key + ":IntArray";
            if (PlayerPrefs.HasKey(GetLengthKey(arrayKey)))
            {
                return arrayKey;
            }
            arrayKey = key + ":FloatArray";
            if (PlayerPrefs.HasKey(GetLengthKey(arrayKey)))
            {
                return arrayKey;
            }
            arrayKey = key + ":StringArray";
            if (PlayerPrefs.HasKey(GetLengthKey(arrayKey)))
            {
                return arrayKey;
            }
            return "";
        }

        static string GetKey(string key)
        {
            UnityEngine.Assertions.Assert.IsTrue(PlayerNameController.IsPlayerSet());
            return PlayerPrefs.GetString(PlayerNameController.CurPlayerPrefsKey) + ":" + key; // HACK should probably make all this nonstatic and use an instance of playerNameController instead of loading the name directly
        }

        static string GetLengthKey(string key)
        {
            return GetKey(key) + ":ArrayLen";
        }

        static int GetLength(string key) => PlayerPrefs.GetInt(GetLengthKey(key));

        static void SetLength(string key, int len)
        {
            PlayerPrefs.SetInt(GetLengthKey(key), len);
        }
    }
}