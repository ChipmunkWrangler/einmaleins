using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayerName : MonoBehaviour {
	[SerializeField] GameObject enterNamePanel = null;
	[SerializeField] TextButton[] playerButtons = null;
	[SerializeField] Button playButton = null;
	[SerializeField] InputField inputField = null;
	const string playerNamesPrefsKey = "playerNames";
	const string curPlayerPrefsKey = "curPlayer";
	string[] playerNames;
	string newName;

	void Start() {
		playButton.interactable = false;
		playerNames = PlayerPrefsArray.GetStringArray (playerNamesPrefsKey);
		int numPlayers = playerNames == null ? 0 : playerNames.Length;
		enterNamePanel.SetActive (numPlayers < playerButtons.Length);
		if (numPlayers == 0) {
			inputField.Select ();
		}
		for(int i = 0; i < numPlayers; ++i) {
			playerButtons [i].SetText (playerNames [i]);
			playerButtons [i].SetActive (true);
		}
		for(int i = numPlayers; i < playerButtons.Length; ++i) {
			playerButtons [i].SetActive (false);
		}
	}

	public void OnPlayerNameChanged(string name) {
		playButton.interactable = name.Length > 0;
		newName = name;
	}

	public void OnPlayerNameButton(int i) {
		SetCurPlayerName (playerNames[i]);
		Play ();
	}

	public void OnPlay() {
		// TODO check that newName is different from all existing names
		AppendToPlayerNames(newName);
		SetCurPlayerName (newName);
		Play ();
	}

	void Play() {
		// todo transition
		LoadMainScene();
	}

	void LoadMainScene() {
		PlayerPrefs.Save ();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("main");
	}

	void SetCurPlayerName(string name) {
		PlayerPrefs.SetString (curPlayerPrefsKey, name);
	}

	void AppendToPlayerNames(string name) {
		if (playerNames == null) {
			playerNames = new string[] {name};
		} else {
			string[] newPlayerNames = new string[playerNames.Length + 1];
			for (int i = 0; i < playerNames.Length; ++i) {
				newPlayerNames [i] = playerNames [i];
			}
			newPlayerNames [playerNames.Length] = name;
			playerNames = newPlayerNames;
		}
		PlayerPrefsArray.SetStringArray (playerNamesPrefsKey, playerNames);
	}
}

