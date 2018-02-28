using CrazyChipmunk;

/// <summary>Saved state of how much time and effort the player has invested today</summary>
class EffortTrackerPersistentData
{
    const string PrefsKey = "effortTracking";

    public int Frustration { get; set; }
    public int QuizzesToday { get; set; }
    public float TimeToday { get; set; }

    public void Save()
    {
        Prefs.SetDateTime(PrefsKey + ":date", System.DateTime.Today);
        Prefs.SetInt(PrefsKey + ":frustration", Frustration);
        Prefs.SetInt(PrefsKey + ":quizzesToday", QuizzesToday);
        Prefs.SetFloat(PrefsKey + ":timeToday", TimeToday);
    }

    public void Load()
    {
        Frustration = Prefs.GetInt(PrefsKey + ":frustration", 0);
        if (Prefs.GetDateTime(PrefsKey + ":date", System.DateTime.MinValue) < System.DateTime.Today)
        {
            QuizzesToday = 0;
            TimeToday = 0;
        }
        else
        {
            QuizzesToday = Prefs.GetInt(PrefsKey + ":quizzesToday", 0);
            TimeToday = Prefs.GetFloat(PrefsKey + ":timeToday", 0);
        }
    }
}