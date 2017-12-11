using UnityEngine;
using System.Linq;

public class PlayerNameController {
	const string namesPrefsKey = "playerNames";
	public const string curPlayerPrefsKey = "curPlayer";

	public string[] names { get; private set; }
	public string curName;

	public void Load() {
		names = PlayerPrefsArray.GetStringArray (namesPrefsKey);
		curName = IsPlayerSet () ? PlayerPrefs.GetString (curPlayerPrefsKey) : "";
	}

	public void Save() {
		PlayerPrefsArray.SetStringArray (namesPrefsKey, names);
		PlayerPrefs.SetString (curPlayerPrefsKey, curName);
	}

	static public bool IsPlayerSet() {
		return PlayerPrefs.HasKey (curPlayerPrefsKey) && PlayerPrefs.GetString (curPlayerPrefsKey).Length > 0;
	}

	public void AppendName(string name) {
		string[] newPlayerNames = new string[names.Length + 1];
		for (int i = 0; i < names.Length; ++i) {
			newPlayerNames [i] = names [i];
		}
		newPlayerNames [names.Length] = name;
		names = newPlayerNames;
	}

	public bool IsNameValid(string playerName) {
		return playerName.Length > 0 && !names.Contains (playerName);
	}
}
