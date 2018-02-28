using CrazyChipmunk;

[System.Serializable]
class RocketPartsPersistentData
{
    const string PrefsKey = "rocketParts";

    public bool JustUpgraded { get; set; }
    public int NumParts { get; set; }
    public bool IsRocketBuilt { get; set; }
    public int UpgradeLevel { get; set; }

    public void Save()
    {
        Prefs.SetBool(PrefsKey + ":isBuilt", IsRocketBuilt);
        Prefs.SetInt(PrefsKey + ":upgradeLevel", UpgradeLevel);
        Prefs.SetBool(PrefsKey + ":justUpgraded", JustUpgraded);
        Prefs.SetInt(PrefsKey, NumParts);
    }

    public void Load()
    {
        NumParts = Prefs.GetInt(PrefsKey, 0);
        IsRocketBuilt = Prefs.GetBool(PrefsKey + ":isBuilt");
        UpgradeLevel = Prefs.GetInt(PrefsKey + ":upgradeLevel", 0);
        JustUpgraded = Prefs.GetBool(PrefsKey + ":justUpgraded");
    }
}