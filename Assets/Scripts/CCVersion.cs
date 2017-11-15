using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CCVersion : MonoBehaviour {
	const int majorVersion = 0;
	const int minorVersion = 1;
	const int buildNumber = 10;

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
		switch( oldVersion ) {
		case "0.1.9":
		case "0.1.8":
			WriteNewVersion ();
			break;
		default:
			RestartWithNewVersion ();
			break;
		}
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
		if (SceneManager.GetActiveScene ().name == "updateVersion") {
			PlayerPrefs.DeleteAll ();
			WriteNewVersion ();
			SceneManager.LoadScene ("choosePlayer");
		} else {
			SceneManager.LoadScene ("updateVersion");
		}
	}

	void WriteNewVersion() {
		PlayerPrefs.SetString ("version", GetCurrentVersion ());
		PlayerPrefs.Save ();
		isChecking = false;
	}
}
