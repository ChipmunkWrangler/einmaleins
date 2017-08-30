using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question {
	public enum Stage {
		Inactive,
		Active
	}
	public Stage stage;

	private const int minute = 60;
	private const int hour = 60 * minute;
	private const int day = 24 * hour;
	private const int month = 30 * day;
	private const int year = 12 * month;

	static private int[] intervalSeconds = new int[] {
		0,
		25,
		2 * minute,
		10 * minute,
		hour,
		5 * hour,
		day,
		5 * day,
		25 * day,
		4 * month,
		2 * year
	};
	private const int masteryIdx = 8; // if the next review is in a month, you have mastered this question

	private int a;
	private int b;
	private int intervalIdx;
	private System.DateTime nextTime;
	private string prefsKey;

	public Question(int _a, int _b) {
		a = _a;
		b = _b;
		intervalIdx = 0;
		nextTime = System.DateTime.UtcNow;
		stage = Stage.Inactive;
	}

	public string GetQuestionString() {
		int x = Random.Range(0,2);
		return (x == 0) ? a + " x " + b : b + " x " + a;
	}

	public bool IsAnswerCorrect(string answer) {
		int result;
		int correctAnswer = a * b;
		if (int.TryParse (answer, out result)) {
			return result == correctAnswer;
		}
		return false;
	}

	public float GetMasteryFraction() {
		return Mathf.Clamp(intervalIdx, 0, masteryIdx) / (float)masteryIdx;
	}

	public void UpdateInterval(bool correct) {
		if (correct) {
			++intervalIdx;
			if (intervalIdx >= intervalSeconds.Length) {
				intervalIdx = intervalSeconds.Length - 1;
			}
		} else {
			intervalIdx = 0;
		}
		nextTime = System.DateTime.UtcNow.AddSeconds(intervalSeconds[intervalIdx]);
		Save ();
	}

	public System.DateTime GetNextTime() {
		return nextTime;
	}

	public void Load(string _prefsKey) {
		prefsKey = _prefsKey;
		string intervalKey = prefsKey + ":intervalIdx";
		if (PlayerPrefs.HasKey(intervalKey)) {
			intervalIdx = PlayerPrefs.GetInt(intervalKey);
		}
		string nextTimeKey = prefsKey + ":nextTime";
		if (PlayerPrefs.HasKey(nextTimeKey)) {
			string asString = PlayerPrefs.GetString (nextTimeKey);
			long asLong;
			if (long.TryParse (asString, out asLong)) {
				nextTime = System.DateTime.FromBinary (asLong);
			} else {
				Debug.Log ("Failed to parse nextTime " + asString + " for " + ToString());
			}
		}
		string stageKey = prefsKey + ":stage";
		if (PlayerPrefs.HasKey (stageKey)) {
			try {
				stage = (Stage) System.Enum.Parse(typeof(Stage), PlayerPrefs.GetString (stageKey));
			} catch (System.ArgumentException e) {
				Debug.Log ("Invalid stage " + e);
				stage = Stage.Inactive;
			}
		}
	}

	public void Save() {
		UnityEngine.Assertions.Assert.AreNotEqual (prefsKey.Length, 0);
		PlayerPrefs.SetInt(prefsKey + ":intervalIdx", intervalIdx);
		PlayerPrefs.SetString(prefsKey + ":nextTime", nextTime.ToBinary().ToString());	
		PlayerPrefs.SetString(prefsKey + ":stage", stage.ToString());
		Debug.Log ("Saving " + ToString ());
	}

	public override string ToString() {
		return a + " * " + b + " : intervalIdx = " + intervalIdx + ", nextTime = " + nextTime.ToString() + ", " + stage;
	}
}
