[System.Serializable]
public class PlayerData {
	public string playerName;
	public RocketPartsPersistantData rocketPartsData = new RocketPartsPersistantData ();
	public TargetPlanetPersistentData targetPlanetData = new TargetPlanetPersistentData ();
	public EffortTrackerPersistantData effortTrackerData = new EffortTrackerPersistantData ();
	public StatsControllerPersistentData statsControllerData = new StatsControllerPersistentData();
	public QuestionsPersistentData questionsData = new QuestionsPersistentData();

	public void Load(string name) {
		playerName = name;
		rocketPartsData.Load ();
		effortTrackerData.Load ();
		statsControllerData.Load (Questions.maxNum);
		targetPlanetData.Load ();
		questionsData.Load ();
	}
}
