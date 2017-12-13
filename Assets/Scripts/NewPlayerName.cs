using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayerName : MonoBehaviour {
	[SerializeField] GameObject enterNamePanel = null;
	[SerializeField] TextButton[] playerButtons = null;
	[SerializeField] Button playButton = null;
	[SerializeField] Image playButtonImage = null;
	[SerializeField] InputField inputField = null;
	[SerializeField] float buttonFadeAlpha = 0.5f;
	[SerializeField] float buttonFadeDuration = 0.1f;
	[SerializeField] GameObject rocketPartsGameObj = null;

	string newName;
	bool buttonsAlreadyPressed;
	PlayerNameController playerNameController;

	void Start () {
		playerNameController = new PlayerNameController();
		playerNameController.Load();
		if (PlayerNameController.IsPlayerSet()) {
			RocketParts.Reset();
			TargetPlanet.Reset();
		} // else this is initial start
		ActivatePlayButton( false );
		int numPlayers = playerNameController.names.Length;
		enterNamePanel.SetActive( numPlayers < playerButtons.Length );
		if (numPlayers == 0) {
			inputField.Select();
		}
		for (int i = 0; i < numPlayers; ++i) {
			playerButtons[ i ].SetText( playerNameController.names[ i ] );
			playerButtons[ i ].SetActive( true );
		}
		for (int i = numPlayers; i < playerButtons.Length; ++i) {
			playerButtons[ i ].SetActive( false );
		}
	}


	public void OnPlayerNameChanged (string name) {
		if (!buttonsAlreadyPressed) {
			ActivatePlayButton( playerNameController.IsNameValid( name ) );
			newName = name;
		}
	}

	public void OnPlayerNameButton (int i) {
		if (!buttonsAlreadyPressed) {
			playerNameController.curName = playerNameController.names[ i ];
			Play();
		}
	}

	public void OnPlay () {
		if (!buttonsAlreadyPressed) {
			playerNameController.AppendName( newName );
			playerNameController.curName = newName;
			Play();
		}
	}

	void ActivatePlayButton (bool b) {
		playButton.interactable = b;
		playButtonImage.CrossFadeAlpha( b ? 1.0f : buttonFadeAlpha, buttonFadeDuration, false );
	}

	void Play () {
		// todo transition
		DisableButtons();
		playerNameController.Save();
		PlayerPrefs.Save();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync( IsRocketBuilt() ? "launch" : "rocketBuilding" );
	}

	bool IsRocketBuilt () {
		if (PlayerNameController.IsPlayerSet()) {
			rocketPartsGameObj.SetActive( true );
			return RocketParts.instance.isRocketBuilt && ChooseRocketColour.HasChosenColour();
		}
		return false;
	}

	void DisableButtons () {
		buttonsAlreadyPressed = true;			
		playButton.enabled = false;
		foreach (TextButton button in playerButtons) {
			button.enabled = false;
		}
	}
}

