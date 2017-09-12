﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
	public int idx { get; private set; }

	const float FAST_TIME = 15.0f;
	const float OK_TIME = 60.0f;
	public const int CORRECT_BEFORE_MASTERED = 7;
	const int NUM_ANSWER_TIMES_TO_RECORD = 3;
	string prefsKey;
	List<float> answerTimes;
	bool wasMastered; // even if it is no longer mastered. This is for awarding rocket parts

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

	public float GetAverageAnswerTime() {
		return (answerTimes.Count == 0) ? float.MaxValue : answerTimes.Average ();
	}

	public bool UpdateState(bool isCorrect, float timeRequired) {
		bool isNewlyMastered = false;
		if (isCorrect && stage != Stage.Wrong) { // once it is wrong, it stays wrong until the next list is generated. "Wrong" means "not right on the first try".)
			++correctInARow;
			RecordAnswerTime (timeRequired);
			if (timeRequired > OK_TIME) {
				SetStage(Stage.Hard);
			} else if (stage != Stage.Mastered) {
				if (correctInARow >= CORRECT_BEFORE_MASTERED) {
					isNewlyMastered = SetStage (Stage.Mastered);
				} else if (timeRequired < FAST_TIME) {
					isNewlyMastered = SetStage((stage == Stage.Fast) ? Stage.Mastered : Stage.Fast);
				} else {
					UnityEngine.Assertions.Assert.IsTrue (timeRequired < OK_TIME);
					SetStage(Stage.Ok);
				}
			}
		} else {
			SetStage(Stage.Wrong);
			correctInARow = 0;
		}

		Debug.Log(ToString());
		return isNewlyMastered;
	}

//	public void Load(int idx) {
//		stage = Stage.Hard;
//		switch (idx) {
//		case 0: // 1x1
//		case 1: // 1x2
//		case 2: // 1x3
//		case 3: // 1x4
//		case 10: // 2x2
//		case 11: // 2x3
//		case 12: // 2x4
//		case 19: // 3x3
//		case 20: // 3x4	
//			correctInARow = 5;
//			break;
//		case 4: // 1x5
//		case 5: // 1x6
//		case 6: // 1x7
//		case 13: // 2x5
//		case 14: // 2x6
//		case 15: // 2x7
//		case 21: // 3x5
//		case 22: // 3x6
//		case 23: // 3x7
//		case 27: // 4x4
//		case 29: // 4x6
//		case 30: // 4x7
//		case 34: // 5x5
//		case 35: // 5x6
//		case 40: // 6x6
//		case 41: // 6x7
//		case 45: // 7x7
//			correctInARow = 4;
//			break;
//		case 7: // 1x8
//		case 16: // 2x8
//		case 24: // 3x8
//		case 31: // 4x8
//		case 36: // 5x7
//			correctInARow = 3;			
//			break;
//		case 28: // 4x5
//		case 37: // 5x8
//		case 42: // 6x8
//			correctInARow = 2;
//			break;
//		case 46: // 7x8
//		case 49: // 8x8
//			correctInARow = 1;
//			break;
//		case 8: // 1x9
//		case 9: // 1x10
//		case 17: // 2x9
//		case 18: // 2x10
//		case 25: // 3x9
//		case 26: // 3x10
//		case 32: // 4x9
//		case 33: // 4x10
//		case 38: // 5x9
//		case 39: // 5x10
//		case 43: // 6x9
//		case 44: // 6x10
//		case 47: // 7x9
//		case 48: // 7x10			
//		default:
//			stage = Stage.Inactive;
//			break;
//		}
//	}
		
	public void Load(string _prefsKey, int _idx) {
		prefsKey = _prefsKey;
		idx = _idx;
		string stageKey = prefsKey + ":stage";
		if (MDPrefs.HasKey (stageKey)) {
			try {
				stage = (Stage) System.Enum.Parse(typeof(Stage), MDPrefs.GetString (stageKey));
			} catch (System.ArgumentException e) {
				Debug.Log ("Invalid stage " + e);
			}

		}
		correctInARow = MDPrefs.GetInt(prefsKey + ":inarow", 0);
		answerTimes = MDPrefs.GetFloatArray (prefsKey + ":times").ToList();
		wasMastered = MDPrefs.GetBool (prefsKey + ":wasMastered");
	}

	public void Save() {
		UnityEngine.Assertions.Assert.AreNotEqual (prefsKey.Length, 0);
		MDPrefs.SetString(prefsKey + ":stage", stage.ToString());
		MDPrefs.SetInt(prefsKey + ":inarow", correctInARow);
		MDPrefs.SetFloatArray (prefsKey + ":times", answerTimes.ToArray());
		MDPrefs.SetBool (prefsKey + ":wasMastered", wasMastered);
//		Debug.Log ("Saving " + ToString ());
	}

	public override string ToString() {
		string s = idx + " is " + a + " * " + b + " : " + stage + " wasMastered = " + wasMastered + " correct = " + correctInARow + " times = ";
		foreach (var time in answerTimes) {
			s += time + " ";
		}
		return s;
	}

	void RecordAnswerTime (float timeRequired)
	{
		if (answerTimes.Count >= NUM_ANSWER_TIMES_TO_RECORD) {
			answerTimes.RemoveRange (0, 1 + answerTimes.Count - NUM_ANSWER_TIMES_TO_RECORD);
		}
		answerTimes.Add (timeRequired);

	}

	bool SetStage (Stage newStage)
	{
		stage = newStage;
		bool isNewlyMastered = false;
		if (newStage == Stage.Mastered) {
			isNewlyMastered = !wasMastered;
			wasMastered = true;
		}
		return isNewlyMastered;
	}

}
