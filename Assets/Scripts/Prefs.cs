using UnityEngine;

namespace CrazyChipmunk
{
    [CreateAssetMenu(menuName = "CrazyChipmunk/Prefs")]
    public class Prefs : ScriptableObject
    {
        [SerializeField] StringReadOnly playerName = null;

        public bool HasKey(string key) => PlayerPrefs.HasKey(GetKey(key));

        public void DeleteKey(string key)
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

        public int GetInt(string key, int defaultValue) => PlayerPrefs.GetInt(GetKey(key), defaultValue);
        public void SetInt(string key, int i)
        {
            PlayerPrefs.SetInt(GetKey(key), i);
        }
        public float GetFloat(string key, float defaultValue) => PlayerPrefs.GetFloat(GetKey(key), defaultValue);
        public void SetFloat(string key, float f)
        {
            PlayerPrefs.SetFloat(GetKey(key), f);
        }
        public string GetString(string key, string defaultValue = default(string)) => PlayerPrefs.GetString(GetKey(key), defaultValue);
        public void SetString(string key, string s)
        {
            PlayerPrefs.SetString(GetKey(key), s);
        }
        public bool GetBool(string key, bool defaultValue = default(bool)) => PlayerPrefsUtility.GetBool(GetKey(key), defaultValue);
        public void SetBool(string key, bool b)
        {
            PlayerPrefsUtility.SetBool(GetKey(key), b);
        }

        public System.DateTime GetDateTime(string key, System.DateTime defaultValue) => PlayerPrefsUtility.GetDateTime(GetKey(key), defaultValue);
        public void SetDateTime(string key, System.DateTime dateTime)
        {
            PlayerPrefsUtility.SetDateTime(GetKey(key), dateTime);
        }
        public Color GetColor(string key, Color defaultValue = default(Color))
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
            for (int i = 0; i < value.Length; ++i)
            {
                SetInt(key + ":" + i.ToString(), value[i]);
            }
        }

        public int[] GetIntArray(string key, int dfault = default(int))
        {
            key += ":IntArray";
            int length = GetLength(key);
            var returns = new int[length];
            for (int i = 0; i < length; ++i)
            {
                returns.SetValue(GetInt(key + ":" + i, dfault), i);
            }
            return returns;
        }

        public void SetFloatArray(string key, float[] value)
        {
            key += ":FloatArray";
            SetLength(key, value.Length);
            for (int i = 0; i < value.Length; ++i)
            {
                SetFloat(key + ":" + i.ToString(), value[i]);
            }
        }

        // Get an array of floats
        public float[] GetFloatArray(string key)
        {
            key += ":FloatArray";
            int length = GetLength(key);
            var returns = new float[length];
            for (int i = 0; i < length; ++i)
            {
                returns.SetValue(GetFloat(key + ":" + i, default(float)), i);
            }
            return returns;
        }

        public string[] GetStringArray(string key)
        {
            key += ":StringArray";
            int length = GetLength(key);
            var returns = new string[length];
            for (int i = 0; i < length; ++i)
            {
                returns.SetValue(GetString(key + ":" + i), i);
            }
            return returns;
        }

        public void SetStringArray(string key, string[] value)
        {
            key += ":StringArray";
            SetLength(key, value.Length);
            for (int i = 0; i < value.Length; ++i)
            {
                SetString(key + ":" + i.ToString(), value[i]);
            }
        }

        public void SetBoolArray(string key, bool[] value)
        {
            key += ":BoolArray";
            SetLength(key, value.Length);
            for (int i = 0; i < value.Length; ++i)
            {
                SetBool(key + ":" + i, value[i]);
            }
        }

        public bool[] GetBoolArray(string key)
        {
            key += ":BoolArray";
            int length = GetLength(key);
            var returns = new bool[length];
            for (int i = 0; i < length; ++i)
            {
                returns.SetValue(GetBool(key + ":" + i), i);
            }
            return returns;
        }

        string GetArrayKey(string key)
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

        string GetKey(string key)
        {
            UnityEngine.Assertions.Assert.IsTrue(PlayerNameController.IsPlayerSet());
            return playerName + ":" + key;
        }

        string GetLengthKey(string key)
        {
            return GetKey(key) + ":ArrayLen";
        }

        int GetLength(string key) => PlayerPrefs.GetInt(GetLengthKey(key));

        void SetLength(string key, int len)
        {
            PlayerPrefs.SetInt(GetLengthKey(key), len);
        }
    }
}