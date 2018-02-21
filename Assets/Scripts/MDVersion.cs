using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MDVersion : MonoBehaviour {
    [SerializeField] Questions QuestionContainer = null;

    const int MajorVersion = 1;
    const int MinorVersion = 0;
    const int BuildNumber = 0;

    bool IsChecking;

	public static string GetCurrentVersion () {
		return MajorVersion + "." + MinorVersion + "." + BuildNumber;
	}

	public void CheckVersion () {
		if (IsChecking) {
			return;
		}
		Debug.unityLogger.logEnabled = Debug.isDebugBuild;	
		IsChecking = true;
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
		IsChecking = false;
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
		var playerNameController = new PlayerNameController();
		playerNameController.Load();
		string oldName = playerNameController.CurName;
		foreach (string playerName in playerNameController.Names) {
			playerNameController.CurName = playerName;
			playerNameController.Save();
//			Debug.Log( "Updating question for " + playerName );
			QuestionContainer.gameObject.SetActive( true ); // load question list
			foreach (Question question in QuestionContainer.QuestionArray) {
				question.SetNewFromAnswerTime();
			}
			QuestionContainer.Save();
			QuestionContainer.gameObject.SetActive( false );
		}
		playerNameController.CurName = oldName;
		playerNameController.Save();
	}

	void UpdateFrom_0_1_8_To_0_1_11 () {
		const float oldAnswerTimeInitial = 3F + 0.01F;
		var playerNameController = new PlayerNameController();
		playerNameController.Load();
		string oldName = playerNameController.CurName;
		foreach (string playerName in playerNameController.Names) {
			playerNameController.CurName = playerName;
			playerNameController.Save();
			Debug.Log( "Updating question for " + playerName );
			QuestionContainer.gameObject.SetActive( true ); // load question list
			foreach (Question question in QuestionContainer.QuestionArray) {
				question.UpdateInitialAnswerTime( oldAnswerTimeInitial );
			}
			QuestionContainer.Save();
			QuestionContainer.gameObject.SetActive( false );
		}
		playerNameController.CurName = oldName;
		playerNameController.Save();
	}
}
