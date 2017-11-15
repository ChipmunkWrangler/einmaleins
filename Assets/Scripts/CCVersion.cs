using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCVersion : MonoBehaviour {
	const int majorVersion = 0;
	const int minorVersion = 1;
	const int buildNumber = 10;

	static public string GetCurrentVersion() {
		return majorVersion + "." + minorVersion + "." + buildNumber;
	}
		
	public void CheckVersion() {
		string oldVersion = PlayerPrefs.GetString ("version");
		if (oldVersion == GetCurrentVersion ()) {
			return;
		}
		PlayerPrefs.SetString ("version", GetCurrentVersion ());
		switch( oldVersion ) {
		case "0.1.9":
		case "0.1.8":
			break;
		default:
			StartCoroutine(RestartWithNewVersion ());
			break;
		}
	}

	void OnApplicationPause(bool pauseStatus) {
		if (!pauseStatus) {
			CheckVersion ();
		}
	}

	IEnumerator RestartWithNewVersion() {
		yield return new WaitForEndOfFrame ();
		PlayerPrefs.DeleteAll ();
		UnityEngine.SceneManagement.SceneManager.LoadScene ("choosePlayer");
	}
}
