﻿using UnityEngine;
using System.Linq;

public class PlayerNameController
{
	const string namesPrefsKey = "playerNames";
	public const string curPlayerPrefsKey = "curPlayer";

	public string[] names { get; private set; }

	public string curName;

    static public bool IsPlayerSet() => PlayerPrefs.HasKey(curPlayerPrefsKey) && PlayerPrefs.GetString(curPlayerPrefsKey).Length > 0;

    public bool IsNameValid(string playerName) => playerName.Length > 0 && !names.Contains(playerName);

	public void Load ()
	{
		names = PlayerPrefsArray.GetStringArray (namesPrefsKey);
		curName = IsPlayerSet () ? PlayerPrefs.GetString (curPlayerPrefsKey) : "";
	}

	public void Save ()
	{
		PlayerPrefsArray.SetStringArray (namesPrefsKey, names);
		PlayerPrefs.SetString (curPlayerPrefsKey, curName);
	}

	public void AppendName (string name)
	{
		var newPlayerNames = new string[names.Length + 1];
		for (int i = 0; i < names.Length; ++i) {
			newPlayerNames [i] = names [i];
		}
		newPlayerNames [names.Length] = name;
		names = newPlayerNames;
	}

	public void Clear ()
	{
		if (names != null) {
			System.Array.Clear (names, 0, names.Length);
		} else {
			names = new string[0];
		}
		curName = "";
	}
}