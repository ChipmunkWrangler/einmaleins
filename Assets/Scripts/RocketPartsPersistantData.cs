[System.Serializable]
class RocketPartsPersistantData
{
    const string PrefsKey = "rocketParts";

    public bool JustUpgraded { get; set; }
    public int NumParts { get; set; }
    public bool IsRocketBuilt { get; set; }
    public int UpgradeLevel { get; set; }

    public void Save()
    {
        MDPrefs.SetBool(PrefsKey + ":isBuilt", IsRocketBuilt);
        MDPrefs.SetInt(PrefsKey + ":upgradeLevel", UpgradeLevel);
        MDPrefs.SetBool(PrefsKey + ":justUpgraded", JustUpgraded);
        MDPrefs.SetInt(PrefsKey, NumParts);
    }

    public void Load()
    {
        NumParts = MDPrefs.GetInt(PrefsKey, 0);
        IsRocketBuilt = MDPrefs.GetBool(PrefsKey + ":isBuilt");
        UpgradeLevel = MDPrefs.GetInt(PrefsKey + ":upgradeLevel", 0);
        JustUpgraded = MDPrefs.GetBool(PrefsKey + ":justUpgraded");
    }
}