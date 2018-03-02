using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class GameData
{
    readonly PlayerNameController playerNameController = new PlayerNameController(); // todo bug! is scriptable object now

    public string Version { get; set; } = "";
    public List<PlayerData> PlayerList { get; set; } = new List<PlayerData>();

    public void Load()
    {
        //Version = Application.version;
        //playerNameController.Load();
        //string oldName = playerNameController.CurName;
        //foreach (string playerName in playerNameController.Names)
        //{
        //    playerNameController.CurName = playerName;
        //    playerNameController.Save();
        //    var playerData = new PlayerData();
        //    playerData.Load(playerName);
        //    PlayerList.Add(playerData);
        //}
        //playerNameController.CurName = oldName;
        //playerNameController.Save();
    }

    public void Save()
    {
        if (Version != Application.version)
        {
            throw new System.NotSupportedException("File version " + Version + " doesn't match current version " + Application.version);
        }
        PlayerPrefs.DeleteAll();
        var versionModel = new TimesTablesSavedDataVersionModel();
        versionModel.Version = Application.version;
        playerNameController.Clear();
        foreach (PlayerData playerData in PlayerList)
        {
            playerNameController.AppendName(playerData.PlayerName);
            //playerNameController.CurName = playerData.PlayerName;
            playerNameController.Save();
            playerData.Save();
        }
        //playerNameController.CurName = "";
        playerNameController.Save();
        PlayerPrefs.Save();
    }
}
