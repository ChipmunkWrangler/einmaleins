using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public string Version = "";
    public List<PlayerData> PlayerList = new List<PlayerData>();

    readonly PlayerNameController PlayerNameController = new PlayerNameController();

    public void Load()
    {
        Version = MDVersion.GetCurrentVersion();
        PlayerNameController.Load();
        string oldName = PlayerNameController.CurName;
        foreach (string playerName in PlayerNameController.Names)
        {
            PlayerNameController.CurName = playerName;
            PlayerNameController.Save();
            var playerData = new PlayerData();
            playerData.Load(playerName);
            PlayerList.Add(playerData);
        }
        PlayerNameController.CurName = oldName;
        PlayerNameController.Save();
    }

    public void Save()
    {
        if (Version != MDVersion.GetCurrentVersion())
        {
            throw new System.NotSupportedException("File version " + Version + " doesn't match current version " + MDVersion.GetCurrentVersion());
        }
        UnityEngine.PlayerPrefs.DeleteAll();
        MDVersion.WriteNewVersion();
        PlayerNameController.Clear();
        foreach (PlayerData playerData in PlayerList)
        {
            PlayerNameController.AppendName(playerData.PlayerName);
            PlayerNameController.CurName = playerData.PlayerName;
            PlayerNameController.Save();
            playerData.Save();
        }
        PlayerNameController.CurName = "";
        PlayerNameController.Save();
        UnityEngine.PlayerPrefs.Save();
    }
}
