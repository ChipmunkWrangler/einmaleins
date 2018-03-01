using System;
using CrazyChipmunk;
using UnityEngine;

[CreateAssetMenu(menuName = "TimesTables/RocketPartsPersistentData")]
class RocketPartsPersistentData : ScriptableObject
{
    const string PrefsKey = "rocketParts";

    [SerializeField] Prefs prefs = null;

    public bool JustUpgraded { get; set; }
    public int NumParts { get; set; }
    public bool IsRocketBuilt { get; set; }
    public int UpgradeLevel { get; set; }

    public void Save()
    {
        prefs.SetBool(PrefsKey + ":isBuilt", IsRocketBuilt);
        prefs.SetInt(PrefsKey + ":upgradeLevel", UpgradeLevel);
        prefs.SetBool(PrefsKey + ":justUpgraded", JustUpgraded);
        prefs.SetInt(PrefsKey, NumParts);
    }

    public void Load()
    {
        NumParts = prefs.GetInt(PrefsKey, 0);
        IsRocketBuilt = prefs.GetBool(PrefsKey + ":isBuilt");
        UpgradeLevel = prefs.GetInt(PrefsKey + ":upgradeLevel", 0);
        JustUpgraded = prefs.GetBool(PrefsKey + ":justUpgraded");
    }
}