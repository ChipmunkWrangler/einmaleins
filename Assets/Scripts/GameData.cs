using System.Collections.Generic;

[System.Serializable]
public class GameData
{
	public string version = "";
	public List<PlayerData> playerList = new List<PlayerData> ();

	PlayerNameController playerNameController = new PlayerNameController ();

	public void Load ()
	{
		version = MDVersion.GetCurrentVersion ();
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
		if (version != MDVersion.GetCurrentVersion ()) {
			throw new System.NotSupportedException ("File version " + version + " doesn't match current version " + MDVersion.GetCurrentVersion ());
		}
		UnityEngine.PlayerPrefs.DeleteAll ();
		MDVersion.WriteNewVersion ();
		playerNameController.Clear ();
		foreach (PlayerData playerData in playerList) {
			playerNameController.AppendName (playerData.playerName);
			playerNameController.curName = playerData.playerName;
			playerNameController.Save ();
			playerData.Save ();
		}
		playerNameController.curName = "";
		playerNameController.Save ();
		UnityEngine.PlayerPrefs.Save ();
	}
}
