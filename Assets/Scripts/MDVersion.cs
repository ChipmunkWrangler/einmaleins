using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MDVersion : MonoBehaviour {
    [SerializeField] Questions questions = null;

	const int majorVersion = 1;
	const int minorVersion = 0;
	const int buildNumber = 0;

	bool isChecking;

	static public string GetCurrentVersion () {
		return majorVersion + "." + minorVersion + "." + buildNumber;
	}

	public void CheckVersion () {
		if (isChecking) {
			return;
		}
		Debug.unityLogger.logEnabled = Debug.isDebugBuild;	
		isChecking = true;
		string oldVersion = PlayerPrefs.GetString( "version" );
		if (oldVersion == GetCurrentVersion()) {
			return;
		}
		if (SceneManager.GetActiveScene().name != "updateVersion") {
			SceneManager.LoadScene( "updateVersion" );
			return;
		} 
		switch (oldVersion) {
		case "0.1.14":
			break;
		case "0.1.13":
		case "0.1.12":
		case "0.1.11":
			UpdateFrom_0_1_11_To_0_1_14();
			break;
		case "0.1.10":
		case "0.1.9":
		case "0.1.8":
			UpdateFrom_0_1_8_To_0_1_11();
			break;
		default:
			RestartWithNewVersion();
			break;
		}
		WriteNewVersion();
		isChecking = false;
		SceneManager.LoadScene( "choosePlayer" );
	}

	void Start () {
		CheckVersion();
	}

	void OnApplicationPause (bool pauseStatus) {
		if (!pauseStatus) {
			CheckVersion();
		}
	}

	void RestartWithNewVersion () {
		PlayerPrefs.DeleteAll();
	}

	public static void WriteNewVersion () {
		PlayerPrefs.SetString( "version", GetCurrentVersion() );
		PlayerPrefs.Save();
	}

	void UpdateFrom_0_1_11_To_0_1_14 () {
		PlayerNameController playerNameController = new PlayerNameController();
		playerNameController.Load();
		string oldName = playerNameController.curName;
		foreach (string playerName in playerNameController.names) {
			playerNameController.curName = playerName;
			playerNameController.Save();
//			Debug.Log( "Updating question for " + playerName );
			questions.gameObject.SetActive( true ); // load question list
			foreach (Question question in questions.questions) {
				question.SetNewFromAnswerTime();
			}
			questions.Save();
			questions.gameObject.SetActive( false );
		}
		playerNameController.curName = oldName;
		playerNameController.Save();
	}

	void UpdateFrom_0_1_8_To_0_1_11 () {
		const float oldAnswerTimeInitial = 3F + 0.01F;
		PlayerNameController playerNameController = new PlayerNameController();
		playerNameController.Load();
		string oldName = playerNameController.curName;
		foreach (string playerName in playerNameController.names) {
			playerNameController.curName = playerName;
			playerNameController.Save();
			Debug.Log( "Updating question for " + playerName );
			questions.gameObject.SetActive( true ); // load question list
			foreach (Question question in questions.questions) {
				question.UpdateInitialAnswerTime( oldAnswerTimeInitial );
			}
			questions.Save();
			questions.gameObject.SetActive( false );
		}
		playerNameController.curName = oldName;
		playerNameController.Save();
	}
}
