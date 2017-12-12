[System.Serializable]
public class PlayerData {
	public string playerName;
	public RocketPartsPersistantData rocketPartsData = new RocketPartsPersistantData ();
	public EffortTrackerPersistantData effortTrackerData = new EffortTrackerPersistantData ();
	public QuestionsPersistentData questionsData = new QuestionsPersistentData();
	public StatsControllerPersistentData statsControllerData = new StatsControllerPersistentData();
	public TargetPlanetPersistentData targetPlanetData = new TargetPlanetPersistentData ();

	public void Load(string name) {
		playerName = name;
		rocketPartsData.Load ();
		effortTrackerData.Load ();
		statsControllerData.Load (Questions.maxNum);
		targetPlanetData.Load ();
		questionsData.Load ();
	}
}
