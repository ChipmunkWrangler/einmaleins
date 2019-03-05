using CrazyChipmunk;
using UnityEngine;

public class TimesTablesSavedDataVersionModel : SavedDataVersionModel
{
    public override string Version
    {
        get => PlayerPrefs.GetString("version");

        set
        {
            PlayerPrefs.SetString("version", value);
            PlayerPrefs.Save();
        }
    }
}