using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Question { 
	public int a {  get; private set; }
	public int b { get; private set; }
	public int idx { get; private set; }
	public bool wasAnsweredInThisQuiz {get; private set; } // not saved
	public bool isLaunchCode; // not saved

	public const float FAST_TIME = 4.0f;
	const float ANSWER_TIME_MAX = 60.0f;
	const float ANSWER_TIME_INTIAL = FAST_TIME + 0.01f; 
	const float WRONG_ANSWER_TIME_PENALTY = 1f;
	const int NUM_ANSWER_TIMES_TO_RECORD = 3;

	QuestionPersistantData data = new QuestionPersistantData ();

	public Question(int _a, int _b) {
		a = _a;
		b = _b;
		data.isNew = true;
	}

	public int GetAnswer() {
		return a * b;
	}

	public bool WasWrong() {
		return data.wasWrong;
	}

	public bool IsNew() {
		return data.isNew;
	}

	public bool GaveUp() {
		return data.gaveUp;
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
		return data.answerTimes [data.answerTimes.Count - 1];
	}

	public float GetAverageAnswerTime() {
		return data.answerTimes.Average ();
	}

	public void Ask() {		
		data.wasWrong = false;
		data.gaveUp = false;
		isLaunchCode = false;
	}

	public void ResetForNewQuiz() {
		wasAnsweredInThisQuiz = false;
	}

	public bool Answer(bool isCorrect, float timeRequired) {
		bool isNewlyMastered = false;
		if (isCorrect) {
			RecordAnswerTime (GetAdjustedTime(timeRequired));
			data.isNew = false;
			wasAnsweredInThisQuiz = true;
			if (!data.wasMastered && IsMastered()) {
				isNewlyMastered = true;
				data.wasMastered = true;
			}
		} else {
			data.wasWrong = true;
		}
		Debug.Log(ToString());
		return isNewlyMastered;
	}

	public void GiveUp() {
		data.isNew = false;
		data.gaveUp = true;
		wasAnsweredInThisQuiz = true;
		data.Save ();
	}
		
	public void Load(string _prefsKey, int _idx) {
		data.Load (_prefsKey);
		idx = _idx;
	}

	public void Create(string _prefsKey, int _idx) {
		idx = _idx;
		data.Create (_prefsKey);
		data.answerTimes = GetNewAnswerTimes ();
	}

	public void Save() {
		data.Save();
	}

	public override string ToString() {
		string s = idx + " is " + a + " * " + b + " : asMastered = " + data.wasMastered + " wasWrong = " + data.wasWrong + " isNew = " + data.isNew + " gaveUp " + data.gaveUp + " averageTime " + GetAverageAnswerTime () + " times = ";
		foreach (var time in data.answerTimes) {
			s += time + " ";
		}
		return s;
	}

	public void UpdateInitialAnswerTime (float oldAnswerTimeInitial)
	{
		List<float> newAnswerTimes = new List<float>();
		foreach(var time in data.answerTimes) {
			newAnswerTimes.Add(time == oldAnswerTimeInitial ? ANSWER_TIME_INTIAL : time);
		}
		data.answerTimes = newAnswerTimes;	
	}
		
	static List<float> GetNewAnswerTimes ()
	{
		List<float> answerTimes = new List<float> ();
		for (int i = 0; i < NUM_ANSWER_TIMES_TO_RECORD; ++i) {
			answerTimes.Add (ANSWER_TIME_INTIAL);
		}
		return answerTimes;
	}

	void RecordAnswerTime (float timeRequired)
	{
		data.answerTimes.Add (timeRequired);
		if (data.answerTimes.Count > NUM_ANSWER_TIMES_TO_RECORD) {
			data.answerTimes.RemoveRange (0, data.answerTimes.Count - NUM_ANSWER_TIMES_TO_RECORD);
		}
	}

	float GetAdjustedTime(float timeRequired) {
		if (isLaunchCode) {
			timeRequired = ANSWER_TIME_MAX;
		} else if (data.wasWrong) {
			timeRequired += WRONG_ANSWER_TIME_PENALTY;
		}
		if (timeRequired > ANSWER_TIME_MAX) {
			timeRequired = ANSWER_TIME_MAX;
		}
		return timeRequired;
	}

}

public class QuestionPersistantData {
	public bool wasMastered;  // even if it is no longer mastered. This is for awarding rocket parts
	public bool wasWrong; // if a question is answered wrong, then wasWrong is true until it is next asked
	public bool isNew;
	public bool gaveUp;
	public List<float> answerTimes;

	string prefsKey;

	public void Load(string _prefsKey) {
		prefsKey = _prefsKey;
		answerTimes = GetAnswerTimes (prefsKey);
		wasMastered = MDPrefs.GetBool (prefsKey + ":wasMastered");
		wasWrong = MDPrefs.GetBool (prefsKey + ":wasWrong");
		isNew = MDPrefs.GetBool (prefsKey + ":isNew", true);
		gaveUp = MDPrefs.GetBool (prefsKey + ":gaveUp");
	}

	public void Create(string _prefsKey) {
		prefsKey = _prefsKey;
	}

	public void Save ()
	{
		UnityEngine.Assertions.Assert.AreNotEqual (prefsKey.Length, 0);
		SetAnswerTimes (prefsKey, answerTimes);
		MDPrefs.SetBool (prefsKey + ":wasMastered", wasMastered);
		MDPrefs.SetBool (prefsKey + ":wasWrong", wasWrong);
		MDPrefs.SetBool (prefsKey + ":isNew", isNew);
		MDPrefs.SetBool (prefsKey + ":gaveUp", gaveUp);
	}
		
	static List<float> GetAnswerTimes (string prefsKey)
	{
		return MDPrefs.GetFloatArray (prefsKey + ":times").ToList ();
	}

	static void SetAnswerTimes (string prefsKey, List<float> answerTimes) {
		MDPrefs.SetFloatArray (prefsKey + ":times", answerTimes.ToArray ());
	}
}
