[System.Serializable]
public class PlayerData {
	public string playerName;
	public RocketPartsData rocketPartsData = new RocketPartsData ();


	public void Load(string name) {
		playerName = name;
		rocketPartsData.Load ();
	}
}
