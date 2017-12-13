using System.Collections.Generic;

[System.Serializable]
	public string version = MDVersion.GetCurrentVersion();
public class GameData
{
	public List<PlayerData> playerList = new List<PlayerData> ();

	PlayerNameController playerNameController = new PlayerNameController ();

	public void Load ()
	{
		playerNameController.Load ();
		string oldName = playerNameController.curName;
		foreach (string playerName in playerNameController.names) {
			playerNameController.curName = playerName;
			playerNameController.Save ();
			PlayerData playerData = new PlayerData ();
			playerData.Load (playerName);
			playerList.Add (playerData);
		}
		playerNameController.curName = oldName;
		playerNameController.Save ();
	}

	public void Save ()
	{
	}
}
