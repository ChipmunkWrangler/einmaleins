using UnityEngine;

public class TimesTablesSavedDataVersionModel : CrazyChipmunk.SavedDataVersionModel
{
    public override string Version
    {
        get
        {
            return PlayerPrefs.GetString("version");
        }

        set
        {
            PlayerPrefs.SetString("version", value);
            PlayerPrefs.Save();
        }
    }
}
