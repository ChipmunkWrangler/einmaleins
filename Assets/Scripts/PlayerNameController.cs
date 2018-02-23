using System.Linq;
using UnityEngine;

class PlayerNameController
{
    public static readonly string CurPlayerPrefsKey = "curPlayer";

    const string NamesPrefsKey = "playerNames";

    public string CurName { get; set; }
    public string[] Names { get; private set; }

    public static bool IsPlayerSet() => PlayerPrefs.HasKey(CurPlayerPrefsKey) && PlayerPrefs.GetString(CurPlayerPrefsKey).Length > 0;

    public bool IsNameValid(string playerName) => playerName.Length > 0 && !Names.Contains(playerName);

    public void Load()
    {
        Names = PlayerPrefsArray.GetStringArray(NamesPrefsKey);
        CurName = IsPlayerSet() ? PlayerPrefs.GetString(CurPlayerPrefsKey) : "";
    }

    public void Save()
    {
        PlayerPrefsArray.SetStringArray(NamesPrefsKey, Names);
        PlayerPrefs.SetString(CurPlayerPrefsKey, CurName);
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
        CurName = "";
    }
}