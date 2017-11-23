using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Question { 
	public int a {  get; private set; }
	public int b { get; private set; }
	public int idx { get; private set; }
	public bool wasMastered { get; private set; } // even if it is no longer mastered. This is for awarding rocket parts
	public bool wasWrong { get; private set; } // if a question is answered wrong, then wasWrong is true until it is next asked
	public bool isNew { get; private set; }
	public bool wasAnsweredInThisQuiz {get; private set; } // not saved
	public bool gaveUp { get; private set; }
	public bool isLaunchCode; // not saved

	public const float FAST_TIME = 4.0f;
	const float ANSWER_TIME_MAX = 60.0f;
	public const float ANSWER_TIME_INTIAL = FAST_TIME + 0.01f; 
	const float WRONG_ANSWER_TIME_PENALTY = 1f;
	const int NUM_ANSWER_TIMES_TO_RECORD = 3;
	string prefsKey;
	List<float> answerTimes;

	public Question(int _a, int _b) {
		a = _a;
		b = _b;
		isNew = true;
	}

	public int GetAnswer() {
		return a * b;
	}

	public bool IsAnswerCorrect(string answer) {
		int result;
		if (int.TryParse (answer, out result)) {
			return result == GetAnswer();
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
		gaveUp = false;
		isLaunchCode = false;
	}

	public void ResetForNewQuiz() {
		wasAnsweredInThisQuiz = false;
	}

	public bool Answer(bool isCorrect, float timeRequired) {
		bool isNewlyMastered = false;
		if (isCorrect) {
			RecordAnswerTime (GetAdjustedTime(timeRequired));
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

	public void GiveUp() {
		isNew = false;
		gaveUp = true;
		wasAnsweredInThisQuiz = true;
		Save ();
	}
		
	public void Load(string _prefsKey, int _idx) {
		prefsKey = _prefsKey;
		idx = _idx;
		answerTimes = GetAnswerTimes (prefsKey);
		wasMastered = MDPrefs.GetBool (prefsKey + ":wasMastered");
		wasWrong = MDPrefs.GetBool (prefsKey + ":wasWrong");
		isNew = MDPrefs.GetBool (prefsKey + ":isNew", true);
		gaveUp = MDPrefs.GetBool (prefsKey + ":gaveUp");
	}
		
	public void Create(string _prefsKey, int _idx) {
		prefsKey = _prefsKey;
		idx = _idx;
		answerTimes = GetNewAnswerTimes ();
	}

	public void Save() {
		UnityEngine.Assertions.Assert.AreNotEqual (prefsKey.Length, 0);
		SetAnswerTimes (prefsKey, answerTimes);
		MDPrefs.SetBool (prefsKey + ":wasMastered", wasMastered);
		MDPrefs.SetBool (prefsKey + ":wasWrong", wasWrong);
		MDPrefs.SetBool (prefsKey + ":isNew", isNew);
		MDPrefs.SetBool (prefsKey + ":gaveUp", gaveUp);
	}

	public override string ToString() {
		string s = idx + " is " + a + " * " + b + " : wasMastered = " + wasMastered + " wasWrong = " + wasWrong + " isNew = " + isNew + " gaveUp " + gaveUp + " averageTime " + GetAverageAnswerTime () + " times = ";
		foreach (var time in answerTimes) {
			s += time + " ";
		}
		return s;
	}

	static List<float> GetAnswerTimes (string prefsKey)
	{
		return MDPrefs.GetFloatArray (prefsKey + ":times").ToList ();
	}

	static void SetAnswerTimes (string prefsKey, List<float> answerTimes) {
		MDPrefs.SetFloatArray (prefsKey + ":times", answerTimes.ToArray ());
	}

	public void UpdateInitialAnswerTime (float oldAnswerTimeInitial)
	{
		List<float> newAnswerTimes = new List<float>();
		foreach(var time in answerTimes) {
			newAnswerTimes.Add(time == oldAnswerTimeInitial ? ANSWER_TIME_INTIAL : time);
		}
		answerTimes = newAnswerTimes;	
	}

	public static List<float> GetNewAnswerTimes ()
	{
		List<float> answerTimes = new List<float> ();
		for (int i = 0; i < NUM_ANSWER_TIMES_TO_RECORD; ++i) {
			answerTimes.Add (ANSWER_TIME_INTIAL);
		}
		return answerTimes;
	}

	void RecordAnswerTime (float timeRequired)
	{
		answerTimes.Add (timeRequired);
		if (answerTimes.Count > NUM_ANSWER_TIMES_TO_RECORD) {
			answerTimes.RemoveRange (0, answerTimes.Count - NUM_ANSWER_TIMES_TO_RECORD);
		}
	}

	float GetAdjustedTime(float timeRequired) {
		if (isLaunchCode) {
			timeRequired = ANSWER_TIME_MAX;
		} else if (wasWrong) {
			timeRequired += WRONG_ANSWER_TIME_PENALTY;
		}
		if (timeRequired > ANSWER_TIME_MAX) {
			timeRequired = ANSWER_TIME_MAX;
		}
		return timeRequired;
	}
}
