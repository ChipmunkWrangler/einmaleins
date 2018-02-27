using UnityEngine;

// make this nonstatic, initialize it from build settings if possible, and make put it in CrazyChipmunk namespace
static class VersionNumber
{
    const int MajorVersion = 1;
    const int MinorVersion = 0;
    const int BuildNumber = 0;

    public static string GetCurrentVersion()
    {
        return MajorVersion + "." + MinorVersion + "." + BuildNumber;
    }

    public static void WriteNewVersion()
    {
        PlayerPrefs.SetString("version", GetCurrentVersion());
        PlayerPrefs.Save();
    }

    public static string GetSavedVersion()
    {
        return PlayerPrefs.GetString("version");
    }
}