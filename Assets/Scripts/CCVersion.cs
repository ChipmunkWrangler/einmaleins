using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCVersion : MonoBehaviour {
	static int majorVersion = 0;
	static int minorVersion = 2;
	static int buildNumber = 0;

	static public string GetCurrentVersion() {
		return majorVersion + "." + minorVersion + "." + buildNumber;
	}
		
	public void CheckVersion() {
		if (IsVersionMismatched()) {
			print ("Mismatch");
			RestartWithNewVersion ();
			StartCoroutine(RestartWithNewVersion ());
		}
	}

	void OnApplicationPause(bool pauseStatus) {
		if (!pauseStatus) {
			print ("Unpaused");
			CheckVersion ();
		}
	}

	bool IsVersionMismatched() {
		return PlayerPrefs.GetString ("version") != GetCurrentVersion ();
	}

	IEnumerator RestartWithNewVersion() {
		print ("Restart");
		yield return new WaitForEndOfFrame ();
		print ("2");
		PlayerPrefs.DeleteAll ();
		PlayerPrefs.SetString ("version", GetCurrentVersion ());
		UnityEngine.SceneManagement.SceneManager.LoadScene ("choosePlayer");
		print("4");
	}
}
