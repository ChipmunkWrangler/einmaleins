using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    readonly PlayerNameController playerNameController = new PlayerNameController();

    public string Version { get; set; } = "";
    public List<PlayerData> PlayerList { get; set; } = new List<PlayerData>();

    public void Load()
    {
        Version = MDVersion.GetCurrentVersion();
        playerNameController.Load();
        string oldName = playerNameController.CurName;
        foreach (string playerName in playerNameController.Names)
        {
            playerNameController.CurName = playerName;
            playerNameController.Save();
            var playerData = new PlayerData();
            playerData.Load(playerName);
            PlayerList.Add(playerData);
        }
        playerNameController.CurName = oldName;
        playerNameController.Save();
    }

    public void Save()
    {
        if (Version != MDVersion.GetCurrentVersion())
        {
            throw new System.NotSupportedException("File version " + Version + " doesn't match current version " + MDVersion.GetCurrentVersion());
        }
        UnityEngine.PlayerPrefs.DeleteAll();
        MDVersion.WriteNewVersion();
        playerNameController.Clear();
        foreach (PlayerData playerData in PlayerList)
        {
            playerNameController.AppendName(playerData.PlayerName);
            playerNameController.CurName = playerData.PlayerName;
            playerNameController.Save();
            playerData.Save();
        }
        playerNameController.CurName = "";
        playerNameController.Save();
        UnityEngine.PlayerPrefs.Save();
    }
}
