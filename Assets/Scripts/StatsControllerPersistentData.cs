using CrazyChipmunk;
using UnityEngine;

[CreateAssetMenu(menuName = "TimesTables/StatsControllerData")]
internal class StatsControllerPersistentData : ScriptableObject
{
    private const string PrefsKey = "seenMastered";

    [SerializeField] private Prefs prefs;

    public bool[][] SeenMastered { get; set; }

    public void Load(int numMax)
    {
        SeenMastered = new bool[numMax][];
        for (var i = 0; i < numMax; ++i)
        {
            var key = PrefsKey + ":" + i;
            SeenMastered[i] = prefs.GetBoolArray(key);
            if (SeenMastered[i].Length == 0) SeenMastered[i] = new bool[numMax];
        }
    }

    public void Save(int numMax)
    {
        for (var i = 0; i < numMax; ++i) prefs.SetBoolArray(PrefsKey + ":" + i, SeenMastered[i]);
    }
}