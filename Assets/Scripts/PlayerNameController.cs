using System.Linq;
using UnityEngine;
using CrazyChipmunk;

[CreateAssetMenu(menuName = "TimesTables/PlayerNameController")]
class PlayerNameController : ScriptableObject
{
    [SerializeField] VariableString playerName = null;

    const string NamesPrefsKey = "playerNames";

    public string[] Names { get; private set; }

    public bool IsNameValid(string playerName) => playerName.Length > 0 && !Names.Contains(playerName);

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
        for (int i = 0; i < Names.Length; ++i)
        {
            newPlayerNames[i] = Names[i];
        }
        newPlayerNames[Names.Length] = name;
        Names = newPlayerNames;
    }

    public void Clear()
    {
        if (Names != null)
        {
            System.Array.Clear(Names, 0, Names.Length);
        }
        else
        {
            Names = new string[0];
        }
        playerName.Value = ""; // todo there should ideally be only one writer for PlayerName, and it is NewPlayerName
    }
}