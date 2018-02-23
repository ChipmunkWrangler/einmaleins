[System.Serializable]
class TargetPlanetPersistentData
{
    public int TargetPlanetIdx { get; set; }
    public int LastReachedPlanetIdx { get; set; }

    public void Load()
    {
        TargetPlanetIdx = TargetPlanet.GetTargetPlanetIdx();
        LastReachedPlanetIdx = TargetPlanet.GetLastReachedIdx();
    }

    public void Save()
    {
        TargetPlanet.SetTargetPlanetIdx(TargetPlanetIdx);
        TargetPlanet.SetLastReachedIdx(LastReachedPlanetIdx);
    }
}