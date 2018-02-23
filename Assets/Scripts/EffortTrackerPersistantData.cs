using CrazyChipmunk;
using UnityEngine;

class EffortTrackerPersistantData
{
    const string PrefsKey = "effortTracking";
    const int MinFrustration = -2;
    const int MaxFrustration = 3;
    const int MinQuizzesPerDay = 3;
    const float MinTimePerDay = 5 * 60.0F;

    int frustration;

    public int QuizzesToday { get; set; } = -1;
    public float TimeToday { get; set; }
    public int Frustration
    {
        get { return frustration; }
        set { frustration = Mathf.Clamp(value, MinFrustration, MaxFrustration); }
    }

    public bool IsDoneForToday()
    {
        if (QuizzesToday < 0)
        {
            Load();
        }
        return QuizzesToday >= MinQuizzesPerDay && TimeToday >= MinTimePerDay;
    }

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