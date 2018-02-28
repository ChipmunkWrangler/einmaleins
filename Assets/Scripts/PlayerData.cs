[System.Serializable]
class PlayerData
{
    readonly RocketPartsPersistentData rocketPartsData = new RocketPartsPersistentData();
    readonly TargetPlanetPersistentData targetPlanetData = new TargetPlanetPersistentData();
    readonly EffortTrackerPersistentData effortTrackerData = new EffortTrackerPersistentData();
    readonly StatsControllerPersistentData statsControllerData = new StatsControllerPersistentData();
    readonly QuestionsPersistentData questionsData = new QuestionsPersistentData();

    public string PlayerName { get; set; }

    public void Load(string name)
    {
        if (PlayerNameController.IsPlayerSet())
        {
            RocketParts.Reset();
            TargetPlanet.Reset();
        }
        PlayerName = name;
        rocketPartsData.Load();
        effortTrackerData.Load();
        statsControllerData.Load(Questions.MaxNum);
        targetPlanetData.Load();
        questionsData.Load();
    }

    public void Save()
    {
        rocketPartsData.Save();
        effortTrackerData.Save();
        statsControllerData.Save(Questions.MaxNum);
        targetPlanetData.Save();
        questionsData.Save();
    }
}
