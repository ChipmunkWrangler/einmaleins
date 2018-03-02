using CrazyChipmunk;
using UnityEngine;

/// <summary>Saved state of how much time and effort the player has invested today</summary>
[CreateAssetMenu(menuName = "TimesTables/EffortTrackerPersistentData")]
class EffortTrackerPersistentData : ScriptableObject
{
    [SerializeField] Prefs prefs = null;
    const string PrefsKey = "effortTracking";

    public int Frustration { get; set; }
    public int QuizzesToday { get; set; }
    public float TimeToday { get; set; }

    public void Save()
    {
        prefs.SetDateTime(PrefsKey + ":date", System.DateTime.Today);
        prefs.SetInt(PrefsKey + ":frustration", Frustration);
        prefs.SetInt(PrefsKey + ":quizzesToday", QuizzesToday);
        prefs.SetFloat(PrefsKey + ":timeToday", TimeToday);
    }

    public void Load()
    {
        Frustration = prefs.GetInt(PrefsKey + ":frustration", 0);
        if (prefs.GetDateTime(PrefsKey + ":date", System.DateTime.MinValue) < System.DateTime.Today)
        {
            QuizzesToday = 0;
            TimeToday = 0;
        }
        else
        {
            QuizzesToday = prefs.GetInt(PrefsKey + ":quizzesToday", 0);
            TimeToday = prefs.GetFloat(PrefsKey + ":timeToday", 0);
        }
    }
}