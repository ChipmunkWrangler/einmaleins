using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question {
	public enum Stage { //rename
		Inactive,
		Wrong,
		Hard,
		Ok,
		Fast,
		Mastered
	}
	public Stage stage;

	public int a {  get; private set; }
	public int b { get; private set; }
	public int correctInARow { get; private set; }

	const float FAST_TIME = 5.0f;
	const float OK_TIME = 10.0f;
	const int CORRECT_BEFORE_MASTERED = 7;
	string prefsKey;

	public Question(int _a, int _b) {
		a = _a;
		b = _b;
		stage = Stage.Inactive;
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
		if (isCorrect && stage != Stage.Wrong) { // once it is wrong, it stays wrong until the next list is generated. "Wrong" means "not right on the first try".)
			++correctInARow;
			if (timeRequired < FAST_TIME) {
				stage = (stage == Stage.Fast) ? Stage.Mastered : Stage.Fast;
			} else if (correctInARow >= CORRECT_BEFORE_MASTERED) {
				stage = Stage.Mastered;
			} else if (timeRequired < OK_TIME) {
				stage = Stage.Ok;
			} else {
				stage = Stage.Hard;
			}
		} else {
			stage = Stage.Wrong;
			correctInARow = 0;
		}
		Debug.Log(ToString());
	}
		
	public void Load(string _prefsKey) {
		prefsKey = _prefsKey;
		string stageKey = prefsKey + ":stage";
		if (MDPrefs.HasKey (stageKey)) {
			try {
				stage = (Stage) System.Enum.Parse(typeof(Stage), MDPrefs.GetString (stageKey));
			} catch (System.ArgumentException e) {
				Debug.Log ("Invalid stage " + e);
			}

		}
		correctInARow = MDPrefs.GetInt(prefsKey + ":inarow", 0);
	}

	public void Save() {
		UnityEngine.Assertions.Assert.AreNotEqual (prefsKey.Length, 0);
		MDPrefs.SetString(prefsKey + ":stage", stage.ToString());
		MDPrefs.SetInt(prefsKey + ":inarow", correctInARow);
//		Debug.Log ("Saving " + ToString ());
	}

	public override string ToString() {
		return a + " * " + b + " : " + stage + " correct = " + correctInARow;
	}
}
