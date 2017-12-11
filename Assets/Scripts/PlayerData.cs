[System.Serializable]
public class PlayerData {
	public string playerName;
	public RocketPartsPersistantData rocketPartsData = new RocketPartsPersistantData ();
	public EffortTrackerPersistantData effortTrackerData = new EffortTrackerPersistantData ();
	public QuestionsPersistentData questions = new QuestionsPersistentData();

	public void Load(string name) {
		playerName = name;
		rocketPartsData.Load ();
		effortTrackerData.Load ();
		questions.Load ();
	}
}
