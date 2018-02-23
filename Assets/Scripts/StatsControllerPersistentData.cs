class StatsControllerPersistentData
{
    const string PrefsKey = "seenMastered";

    public bool[][] SeenMastered { get; set; }

    public void Load(int numMax)
    {
        SeenMastered = new bool[numMax][];
        for (int i = 0; i < numMax; ++i)
        {
            string key = PrefsKey + ":" + i;
            SeenMastered[i] = MDPrefs.GetBoolArray(key);
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
            MDPrefs.SetBoolArray(PrefsKey + ":" + i, SeenMastered[i]);
        }
    }
}