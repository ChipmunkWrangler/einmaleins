using System;
using UnityEngine;

namespace CrazyChipmunk
{
    // we could decouple the persister and make this a generic PersistentString,
    // but then we'd have to assign a PlayerPrefsPersister in the inspector every time we add a variable.
    // todo Editor magic?
    public class PlayerPrefsStringReference : PersistentStringReference
    {
        [SerializeField] string uniqueKey = Guid.NewGuid().ToString();

        override protected string Load()
        {
            return PlayerPrefs.GetString(uniqueKey);
        }

        override protected void Save(string val)
        {
            PlayerPrefs.SetString(uniqueKey, val);
        }
    }
}