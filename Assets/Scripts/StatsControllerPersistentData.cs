using CrazyChipmunk;
using UnityEngine;

[CreateAssetMenu(menuName = "TimesTables/StatsControllerData")]
class StatsControllerPersistentData : ScriptableObject
{
    const string PrefsKey = "seenMastered";

    [SerializeField] Prefs prefs = null;

    public bool[][] SeenMastered { get; set; }

    public void Load(int numMax)
    {
        SeenMastered = new bool[numMax][];
        for (int i = 0; i < numMax; ++i)
        {
            string key = PrefsKey + ":" + i;
            SeenMastered[i] = prefs.GetBoolArray(key);
            if (SeenMastered[i].Length == 0)
            {
                SeenMastered[i] = new bool[numMax];
            }
        }
    }

    public void Save(int numMax)
    {
        for (int i = 0; i < numMax; ++i)
        {
            prefs.SetBoolArray(PrefsKey + ":" + i, SeenMastered[i]);
        }
    }
}