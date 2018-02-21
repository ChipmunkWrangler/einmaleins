[System.Serializable]
public class PlayerData {
	public string PlayerName;
    readonly RocketPartsPersistantData RocketPartsData = new RocketPartsPersistantData();
    readonly TargetPlanetPersistentData TargetPlanetData = new TargetPlanetPersistentData();
    readonly EffortTrackerPersistantData EffortTrackerData = new EffortTrackerPersistantData();
    readonly StatsControllerPersistentData StatsControllerData = new StatsControllerPersistentData();
    readonly QuestionsPersistentData QuestionsData = new QuestionsPersistentData();

	public void Load (string name) {
		if (PlayerNameController.IsPlayerSet()) {
			RocketParts.Reset();
			TargetPlanet.Reset();
		}
		PlayerName = name;
		RocketPartsData.Load();
		EffortTrackerData.Load();
		StatsControllerData.Load( Questions.MaxNum );
		TargetPlanetData.Load();
		QuestionsData.Load();
	}

	public void Save () {
		RocketPartsData.Save();
		EffortTrackerData.Save();
		StatsControllerData.Save( Questions.MaxNum );
		TargetPlanetData.Save();
		QuestionsData.Save();
	}
}
