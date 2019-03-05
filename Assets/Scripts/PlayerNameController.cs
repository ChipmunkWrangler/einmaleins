using System;
using System.Linq;
using CrazyChipmunk;
using UnityEngine;

[CreateAssetMenu(menuName = "TimesTables/PlayerNameController")]
internal class PlayerNameController : ScriptableObject
{
    private const string NamesPrefsKey = "playerNames";
    [SerializeField] private VariableString playerName;

    public string[] Names { get; private set; }

    public bool IsNameValid(string playerName)
    {
        return playerName.Length > 0 && !Names.Contains(playerName);
    }

    public void Load()
    {
        Names = PlayerPrefsArray.GetStringArray(NamesPrefsKey);
    }

    public void Save()
    {
        PlayerPrefsArray.SetStringArray(NamesPrefsKey, Names);
    }

    public void AppendName(string name)
    {
        var newPlayerNames = new string[Names.Length + 1];
        for (var i = 0; i < Names.Length; ++i) newPlayerNames[i] = Names[i];
        newPlayerNames[Names.Length] = name;
        Names = newPlayerNames;
    }

    public void Clear()
    {
        if (Names != null)
            Array.Clear(Names, 0, Names.Length);
        else
            Names = new string[0];
        playerName.Value = ""; // todo there should ideally be only one writer for PlayerName, and it is NewPlayerName
    }
}