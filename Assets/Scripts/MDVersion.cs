using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MDVersion : MonoBehaviour {
	[SerializeField] Questions questions;

	const int majorVersion = 0;
	const int minorVersion = 1;
	const int buildNumber = 11;

	bool isChecking;

	static public string GetCurrentVersion() {
		return majorVersion + "." + minorVersion + "." + buildNumber;
	}
		
	public void CheckVersion() {
		if (isChecking) {
			return;
		}
		isChecking = true;
		string oldVersion = PlayerPrefs.GetString ("version");
		if (oldVersion == GetCurrentVersion ()) {
			return;
		}
		if (SceneManager.GetActiveScene ().name != "updateVersion") {
			SceneManager.LoadScene ("updateVersion");
			return;
		} 
		switch (oldVersion) {
		case "0.1.10":
		case "0.1.9":
		case "0.1.8":
			UpdateFrom_0_1_8_To_0_1_11 ();
			break;
		default:
			RestartWithNewVersion ();
			break;
		}
		WriteNewVersion ();
		SceneManager.LoadScene ("choosePlayer");
	}

	void Start() {
		CheckVersion();
	}

	void OnApplicationPause(bool pauseStatus) {
		if (!pauseStatus) {
			CheckVersion ();
		}
	}

	void RestartWithNewVersion() {
		PlayerPrefs.DeleteAll ();
	}

	void WriteNewVersion() {
		PlayerPrefs.SetString ("version", GetCurrentVersion ());
		PlayerPrefs.Save ();
		isChecking = false;
	}

	void UpdateFrom_0_1_8_To_0_1_11() {
		const float oldAnswerTimeInitial = 3f + 0.01f;
		foreach (string playerName in NewPlayerName.GetPlayerNames()) {
			NewPlayerName.SetCurPlayerName (playerName);
			Debug.Log ("Updating question for " + playerName);
			questions.gameObject.SetActive (true); // load question list
			foreach(Question question in questions.questions) {
				question.UpdateInitialAnswerTime (oldAnswerTimeInitial);
			}
			questions.Save ();
			questions.gameObject.SetActive (false);
		}
	}
}
