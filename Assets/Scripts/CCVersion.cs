using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
		case "0.1.7":
			StartCoroutine (Update_0_1_7 ());
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
		//if scene is update, do update
		// otherwise, goto update
		UnityEngine.SceneManagement.SceneManager.LoadScene ("choosePlayer");
		yield return new WaitForEndOfFrame ();
		yield return new WaitForEndOfFrame ();
		PlayerPrefs.DeleteAll ();
		TODO choosePlazer should check Version first
	}

	IEnumerator Update_0_1_7() {
		yield return new WaitForEndOfFrame ();
		string log = "Start";
		string[] playerNames = NewPlayerName.GetPlayerNames ();
		foreach (string playerName in playerNames) {
			log += "\nUpdating player " + playerName;
			log += NewPlayerName.SetCurPlayerName (playerName);
			Update_0_1_7_Player ();
		}
		log += "\nEnd";
		UnityEngine.SceneManagement.SceneManager.LoadScene ("choosePlayer");
	}

	string Update_0_1_7_Player() {
		if (!Questions.WereQuestionsCreated ()) {
			return;
		}
		string log;
		Question[] questions = Questions.CreateQuestions ();
		for (int i = 0; i < questions.Length; ++i) {
			log += Update_0_1_7_Question(Questions.GetQuestionKey(i), i);
		}
		log += Update_0_1_7_RocketParts ();
		return log;
	}

	string Update_0_1_7_Question(string prefsKey, int questionIdx) {
		List<float> answerTimes = Question.GetAnswerTimes(prefsKey);
		bool isNew = answerTimes.All (answerTime => Mathf.Approximately (answerTime, 60f));
		MDPrefs.SetBool (prefsKey + ":isNew", isNew);
		string log = "\nQuestion " + questionIdx + " isNew = " + isNew;
		if (isNew) {
			Question.SetAnswerTimes (prefsKey, Question.GetNewAnswerTimes ());
		}
	}

	string Update_0_1_7_RocketParts() {
		RocketParts rocketParts = RocketParts.instance;
		rocketParts.isRocketBuilt = true;
		//		rocketParts.upgradeLevel = 
	}
}
