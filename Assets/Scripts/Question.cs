using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Question {
	public int a {  get; private set; }
	public int b { get; private set; }
	public int idx { get; private set; }
	public bool wasMastered { get; private set; } // even if it is no longer mastered. This is for awarding rocket parts

	public float Debug_chanceOfCorrectAnswer;

	public const float FAST_TIME = 3.0f;
	const float ANSWER_TIME_MAX = 60.0f;
	const float ANSWER_TIME_INTIAL = FAST_TIME + 0.1f; 
	const int ADD_TO_DIFFICULTY_FAST = -3;
	const int ADD_TO_DIFFICULTY_OK = -1;
	const int ADD_TO_DIFFICULTY_WRONG = 2;
	const int NUM_ANSWER_TIMES_TO_RECORD = 3;
	string prefsKey;
	List<float> answerTimes;
	public bool wasWrong { get; private set; } // if a question is answered wrong, then wasWrong is true until it is next asked
	public bool isNew { get; private set; }
	public bool wasAnsweredInThisQuiz {get; private set; } // not saved

	public Question(int _a, int _b) {
		a = _a;
		b = _b;
		isNew = true;
	}

	public bool IsAnswerCorrect(string answer) {
		int result;
		int correctAnswer = a * b;
		if (int.TryParse (answer, out result)) {
			return result == correctAnswer;
		}
		return false;
	}

	public bool IsMastered() {
		return GetAverageAnswerTime () <= FAST_TIME;
	}

	public float GetLastAnswerTime() {
		return answerTimes [answerTimes.Count - 1];
	}

	public float GetAverageAnswerTime() {
		return answerTimes.Average ();
	}

	public void Ask() {
		wasWrong = false;
	}

	public void ResetForNewQuiz() {
		wasAnsweredInThisQuiz = false;
	}

	public bool Answer(bool isCorrect, float timeRequired) {
		bool isNewlyMastered = false;
		if (isCorrect) {
			RecordAnswerTime (timeRequired);
			isNew = false;
			wasAnsweredInThisQuiz = true;
			if (!wasMastered && IsMastered()) {
				isNewlyMastered = true;
				wasMastered = true;
			}
		} else {
			wasWrong = true;
		}
		Debug.Log(ToString());
		return isNewlyMastered;
	}
		
	public void Load(string _prefsKey, int _idx) {
		prefsKey = _prefsKey;
		idx = _idx;
		answerTimes = MDPrefs.GetFloatArray (prefsKey + ":times").ToList();
		wasMastered = MDPrefs.GetBool (prefsKey + ":wasMastered");
		wasWrong = MDPrefs.GetBool (prefsKey + ":wasWrong");
		isNew = MDPrefs.GetBool (prefsKey + ":isNew", true);
	}
		
	public void Create(string _prefsKey, int _idx) {
		prefsKey = _prefsKey;
		idx = _idx;
		answerTimes = new List<float>();
		for (int i = 0; i < NUM_ANSWER_TIMES_TO_RECORD; ++i) {
			answerTimes.Add (ANSWER_TIME_INTIAL);
		}
	}

	public void Save() {
		UnityEngine.Assertions.Assert.AreNotEqual (prefsKey.Length, 0);
		MDPrefs.SetFloatArray (prefsKey + ":times", answerTimes.ToArray());
		MDPrefs.SetBool (prefsKey + ":wasMastered", wasMastered);
		MDPrefs.SetBool (prefsKey + ":wasWrong", wasWrong);
	}

	public override string ToString() {
		string s = idx + " is " + a + " * " + b + " : wasMastered = " + wasMastered + " wasWrong = " + wasWrong + " averageTime " + GetAverageAnswerTime () + " times = ";
		foreach (var time in answerTimes) {
			s += time + " ";
		}
		return s;
	}

	void RecordAnswerTime (float timeRequired)
	{
		answerTimes.Add (Mathf.Min(timeRequired,ANSWER_TIME_MAX));
		if (answerTimes.Count > NUM_ANSWER_TIMES_TO_RECORD) {
			answerTimes.RemoveRange (0, answerTimes.Count - NUM_ANSWER_TIMES_TO_RECORD);
		}
	}
}
