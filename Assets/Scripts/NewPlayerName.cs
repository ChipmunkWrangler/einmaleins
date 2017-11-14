﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class NewPlayerName : MonoBehaviour {
	[SerializeField] GameObject enterNamePanel = null;
	[SerializeField] TextButton[] playerButtons = null;
	[SerializeField] Button playButton = null;
	[SerializeField] Image playButtonImage = null;
	[SerializeField] InputField inputField = null;
	[SerializeField] float buttonFadeAlpha = 0.5f;
	[SerializeField] float buttonFadeDuration = 0.1f;
	[SerializeField] GameObject rocketPartsGameObj = null;
	const string playerNamesPrefsKey = "playerNames";
	const string curPlayerPrefsKey = "curPlayer";
	string[] playerNames;
	string newName;
	bool buttonsAlreadyPressed;

	void Start() {
		if (PlayerPrefs.HasKey (curPlayerPrefsKey)) {
			Destroy(RocketParts.instance);
			TargetPlanet.Reset ();
		} // else this is initial start
		ActivatePlayButton (false);
//		PlayerPrefs.SetInt (playerNamesPrefsKey + ":StringArray:ArrayLen", 2);
		playerNames = PlayerPrefsArray.GetStringArray (playerNamesPrefsKey);
		int numPlayers = playerNames.Length;
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
		if (!buttonsAlreadyPressed) {
			ActivatePlayButton (IsNameValid (name));
			newName = name;
		}
	}

	public void OnPlayerNameButton(int i) {
		if (!buttonsAlreadyPressed) {
			SetCurPlayerName (playerNames [i]);
			Play ();
		}
	}

	public void OnPlay() {
		if (!buttonsAlreadyPressed) {
			AppendToPlayerNames (newName);
			SetCurPlayerName (newName);
			Play ();
		}
	}

	bool IsNameValid(string playerName) {
		return playerName.Length > 0 && !playerNames.Contains (playerName);
	}

	void ActivatePlayButton(bool b) {
		playButton.interactable = b;
		playButtonImage.CrossFadeAlpha (b ? 1.0f : buttonFadeAlpha, buttonFadeDuration, false);
	}
	void Play() {
		// todo transition
		DisableButtons();
		PlayerPrefs.Save ();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (IsRocketBuilt() ? "launch" : "rocketBuilding");
	}

	bool IsRocketBuilt() {
		if (PlayerPrefs.HasKey (curPlayerPrefsKey)) {
			rocketPartsGameObj.SetActive (true);
			return RocketParts.instance.isRocketBuilt;
		}
		return false;
	}

	void DisableButtons() {
		buttonsAlreadyPressed = true;			
		playButton.enabled = false;
		foreach(TextButton button in playerButtons) {
			button.enabled = false;
		}
	}

	void SetCurPlayerName(string name) {
		PlayerPrefs.SetString (curPlayerPrefsKey, name);
	}

	void AppendToPlayerNames(string name) {
		string[] newPlayerNames = new string[playerNames.Length + 1];
		for (int i = 0; i < playerNames.Length; ++i) {
			newPlayerNames [i] = playerNames [i];
		}
		newPlayerNames [playerNames.Length] = name;
		playerNames = newPlayerNames;
		PlayerPrefsArray.SetStringArray (playerNamesPrefsKey, playerNames);
	}
}

