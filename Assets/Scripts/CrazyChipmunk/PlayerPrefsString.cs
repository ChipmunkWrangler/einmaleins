using System;
using UnityEngine;

namespace CrazyChipmunk
{
    // we could decouple the persister and make this a generic PersistentString,
    // but then we'd have to assign a PlayerPrefsPersister in the inspector every time we add a variable.
    // todo Editor magic?
    [CreateAssetMenu(menuName = "CrazyChipmunk/PlayerPrefsString")]
    public class PlayerPrefsString : PersistentString
    {
        [SerializeField] string uniqueKey = Guid.NewGuid().ToString();

        protected override string Load()
        {
            return PlayerPrefs.GetString(uniqueKey);
        }

        protected override void Save(string value)
        {
            PlayerPrefs.SetString(uniqueKey, value);
            PlayerPrefs.Save();
        }
    }
}
