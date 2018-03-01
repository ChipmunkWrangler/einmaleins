using System;
using UnityEngine;

namespace CrazyChipmunk
{
    //public abstract class StringPersister : ScriptableObject
    //{
    //    public abstract string Load();
    //    public abstract void Save(string val);
    //}

    //public interface IValuePersister<T>
    //{
    //    T Load();
    //    void Save(T val);
    //}

    //public abstract class PlayerPrefsPersister<T>
    //{
    //    [SerializeField] string prefsKey = "";
    //    string PrefsKey
    //    {
    //        get
    //        {
    //            return prefsKey;
    //        }
    //    }
    //}

    //public abstract class PlayerPrefsPersister : Persister
    //{
    //    [SerializeField] string prefsKey = "";
    //    protected string PrefsKey
    //    {
    //        get
    //        {
    //            return prefsKey;
    //        }
    //    }
    //}

    //[CreateAssetMenu(menuName = "CrazyChipmunk/PlayerPrefsStringPersister")]
    //public class PlayerPrefsStringPersister : PlayerPrefsPersister
    //{
    //    public override string Load()
    //    {
    //        return PlayerPrefs.GetString(PrefsKey);
    //    }

    //    public override void Save(string val)
    //    {
    //        PlayerPrefs.SetString(PrefsKey, val);
    //    }

    //}

    //[CreateAssetMenu(menuName = "CrazyChipmunk/PersistentString")]
    //public class PersistentString : ScriptableObject
    //{
    //    [SerializeField] string val;
    //    [SerializeField] Persister persister = null;
    //    bool initialized;

    //    public string Value
    //    {
    //        get
    //        {
    //            if (!initialized)
    //            {
    //                val = persister.Load();
    //                initialized = true;
    //            }
    //            return val;
    //        }
    //        set
    //        {
    //            val = value;
    //            persister.Save(val);
    //            initialized = true;
    //        }
    //    }
    //}

  
}