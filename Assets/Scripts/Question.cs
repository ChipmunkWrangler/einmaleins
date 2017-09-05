using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question {
	public enum Stage {//remove
		Inactive,
		Active
	}
	public Stage stage;
	public enum Stage2 { //rename
		Inactive,
		Wrong,
		Hard,
		Ok,
		Fast,
		Mastered
	}
	public Stage2 stage2;

	public int a {  get; private set; }
	public int b { get; private set; }
	public int intervalIdx { get; private set; } // remove this

	const float FAST_TIME = 5.0f;
	const float OK_TIME = 10.0f;
	System.DateTime nextTime; // remove this
	string prefsKey;

	public Question(int _a, int _b, System.DateTime? time = null) {
		a = _a;
		b = _b;
		intervalIdx = 0;
		nextTime = time ?? System.DateTime.UtcNow;
		stage = Stage.Inactive;
		stage2 = Stage2.Inactive;
	}

	public bool IsAnswerCorrect(string answer) {
		int result;
		int correctAnswer = a * b;
		if (int.TryParse (answer, out result)) {
			return result == correctAnswer;
		}
		return false;
	}

	public void UpdateStage(bool isCorrect, float timeRequired) {
		Debug.Log ("timeRequired " + timeRequired + " " + ToString());
		if (isCorrect && stage2 != Stage2.Wrong) { // once it is wrong, it stays wrong until the next list is generated. "Wrong" means "not right on the first try".
			if (timeRequired < FAST_TIME) {
				stage2 = (stage2 == Stage2.Fast) ? Stage2.Mastered : Stage2.Fast;
			} else if (timeRequired < OK_TIME) {
				stage2 = Stage2.Ok;
			} else {
				stage2 = Stage2.Hard;
			}
		} else {
			stage2 = Stage2.Wrong;
		}
		Debug.Log("New stage 2 = " + stage2.ToString());
	}
		
	public void Load(string _prefsKey) {
		prefsKey = _prefsKey;
		string intervalKey = prefsKey + ":intervalIdx";
		if (MDPrefs.HasKey(intervalKey)) {
			intervalIdx = MDPrefs.GetInt(intervalKey, 0);
		}
		string nextTimeKey = prefsKey + ":nextTime";
		if (MDPrefs.HasKey(nextTimeKey)) {
			string asString = MDPrefs.GetString (nextTimeKey);
			long asLong;
			if (long.TryParse (asString, out asLong)) {
				nextTime = System.DateTime.FromBinary (asLong);
			} else {
				Debug.Log ("Failed to parse nextTime " + asString + " for " + ToString());
			}
		}
		string stageKey = prefsKey + ":stage";
		if (MDPrefs.HasKey (stageKey)) {
			try {
				stage = (Stage) System.Enum.Parse(typeof(Stage), MDPrefs.GetString (stageKey));
			} catch (System.ArgumentException e) {
				Debug.Log ("Invalid stage " + e);
				stage = Stage.Inactive;
			}
			try {
				stage2 = (Stage2) System.Enum.Parse(typeof(Stage2), MDPrefs.GetString (stageKey));
			} catch (System.ArgumentException) {
				MapOldToNew ();
			}

		}
	}

	public void Save() {
		UnityEngine.Assertions.Assert.AreNotEqual (prefsKey.Length, 0);
		MDPrefs.SetString(prefsKey + ":nextTime", nextTime.ToBinary().ToString());	
		MDPrefs.SetString(prefsKey + ":stage", stage2.ToString());
//		Debug.Log ("Saving " + ToString ());
	}

	public override string ToString() {
		return a + " * " + b + " : " + stage2;
	}
		
	void MapOldToNew() {
		if (stage == Stage.Inactive) {
			stage2 = Stage2.Inactive;
		} else {
			switch (intervalIdx) {
			case 0: 
			case 1:
				stage2 = Stage2.Wrong;
				break;
			case 2:
			case 3:
				stage2 = Stage2.Hard;
				break;
			case 4:
			case 5:
			default:
				stage2 = Stage2.Ok;
				break;
			}
		}
//		MDPrefs.DeleteKey(prefsKey + ":intervalIdx", intervalIdx);
	}
}
