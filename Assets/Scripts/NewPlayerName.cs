using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPlayerName : MonoBehaviour {
    [SerializeField] GameObject EnterNamePanel = null;
    [SerializeField] TextButton[] PlayerButtons = null;
    [SerializeField] Button PlayButton = null;
    [SerializeField] Image PlayButtonImage = null;
    [SerializeField] InputField InputFld = null;
    [SerializeField] float ButtonFadeAlpha = 0.5F;
    [SerializeField] float ButtonFadeDuration = 0.1F;
    [SerializeField] GameObject RocketPartsGameObj = null;

    string NewName;
    bool ButtonsAlreadyPressed;
    PlayerNameController PlayerNameController;

	void Start () {
		PlayerNameController = new PlayerNameController();
		PlayerNameController.Load();
		if (PlayerNameController.IsPlayerSet()) {
			RocketParts.Reset();
			TargetPlanet.Reset();
		} // else this is initial start
		ActivatePlayButton( false );
		int numPlayers = PlayerNameController.Names.Length;
		EnterNamePanel.SetActive( numPlayers < PlayerButtons.Length );
		if (numPlayers == 0) {
			InputFld.Select();
		}
		for (int i = 0; i < numPlayers; ++i) {
			PlayerButtons[ i ].SetText( PlayerNameController.Names[ i ] );
			PlayerButtons[ i ].SetActive( true );
		}
		for (int i = numPlayers; i < PlayerButtons.Length; ++i) {
			PlayerButtons[ i ].SetActive( false );
		}
	}


	public void OnPlayerNameChanged (string name) {
		if (!ButtonsAlreadyPressed) {
			ActivatePlayButton( PlayerNameController.IsNameValid( name ) );
			NewName = name;
		}
	}

	public void OnPlayerNameButton (int i) {
		if (!ButtonsAlreadyPressed) {
			PlayerNameController.CurName = PlayerNameController.Names[ i ];
			Play();
		}
	}

	public void OnPlay () {
		if (!ButtonsAlreadyPressed) {
			PlayerNameController.AppendName( NewName );
			PlayerNameController.CurName = NewName;
			Play();
		}
	}

	void ActivatePlayButton (bool b) {
		PlayButton.interactable = b;
		PlayButtonImage.CrossFadeAlpha( b ? 1.0F : ButtonFadeAlpha, ButtonFadeDuration, false );
	}

	void Play () {
		// todo transition
		DisableButtons();
		PlayerNameController.Save();
		PlayerPrefs.Save();
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync( IsRocketBuilt() ? "launch" : "rocketBuilding" );
	}

	bool IsRocketBuilt () {
		if (PlayerNameController.IsPlayerSet()) {
			RocketPartsGameObj.SetActive( true );
			return RocketParts.Instance.IsRocketBuilt && ChooseRocketColour.HasChosenColour();
		}
		return false;
	}

	void DisableButtons () {
		ButtonsAlreadyPressed = true;			
		PlayButton.enabled = false;
		foreach (TextButton button in PlayerButtons) {
			button.enabled = false;
		}
	}
}

