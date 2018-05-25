using System;
// todo this is broken as long as the PersistentDatas are ScriptableObjects 
[Serializable]
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
        RocketParts.Reset();
        TargetPlanet.Reset();
        PlayerName = name;
        rocketPartsData.Load();
        effortTrackerData.Load();
        statsControllerData.Load(QuestionGenerator.MaxMultiplicand);
        targetPlanetData.Load();
        //questionsData.Load(); // TODO Need to know what kind of Questions
    }

    public void Save()
    {
        rocketPartsData.Save();
        effortTrackerData.Save();
        statsControllerData.Save(QuestionGenerator.MaxMultiplicand);
        targetPlanetData.Save();
        questionsData.Save();
    }
}
