using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Question {
	public int a { get; private set; }

	public int b { get; private set; }

	public bool wasAnsweredInThisQuiz { get; private set; }

	public bool isLaunchCode;

	public const float FAST_TIME = 4.0f;
	const float ANSWER_TIME_MAX = 60.0f;
	const float WRONG_ANSWER_TIME_PENALTY = 1f;

	QuestionPersistentData data;

	public Question (int _a, int _b, QuestionPersistentData _data) {
		a = _a;
		b = _b;
		data = _data;
	}

	public int GetAnswer () {
		return a * b;
	}

	public bool WasWrong () {
		return data.wasWrong;
	}

	public bool IsNew () {
		return data.isNew;
	}

	public bool GaveUp () {
		return data.gaveUp;
	}

	public bool IsAnswerCorrect (string answer) {
		int result;
		if (int.TryParse( answer, out result )) {
			return result == GetAnswer();
		}
		return false;
	}

	public bool IsMastered () {
		return GetAverageAnswerTime() <= FAST_TIME;
	}

	public float GetLastAnswerTime () {
		return data.answerTimes[ data.answerTimes.Count - 1 ];
	}

	public float GetAverageAnswerTime () {
		return data.answerTimes.Average();
	}

	public void Ask () {		
		data.wasWrong = false;
		data.gaveUp = false;
		isLaunchCode = false;
	}

	public void ResetForNewQuiz () {
		wasAnsweredInThisQuiz = false;
	}

	public bool Answer (bool isCorrect, float timeRequired) {
		bool isNewlyMastered = false;
		if (isCorrect) {
			RecordAnswerTime( GetAdjustedTime( timeRequired ) );
			data.isNew = false;
			wasAnsweredInThisQuiz = true;
			if (!data.wasMastered && IsMastered()) {
				isNewlyMastered = true;
				data.wasMastered = true;
			}
		} else {
			data.wasWrong = true;
		}
		Debug.Log( ToString() );
		return isNewlyMastered;
	}

	public void GiveUp () {
		data.isNew = false;
		data.gaveUp = true;
		wasAnsweredInThisQuiz = true;
		data.Save();
	}

	public override string ToString () {
		string s = data.idx + " is " + a + " * " + b + " : asMastered = " + data.wasMastered + " wasWrong = " + data.wasWrong + " isNew = " + data.isNew + " gaveUp " + data.gaveUp + " averageTime " + GetAverageAnswerTime() + " times = ";
		foreach (var time in data.answerTimes) {
			s += time + " ";
		}
		return s;
	}

	public void UpdateInitialAnswerTime (float oldAnswerTimeInitial) {
		List<float> newAnswerTimes = new List<float>();
		foreach (var time in data.answerTimes) {
			newAnswerTimes.Add( time == oldAnswerTimeInitial ? QuestionPersistentData.ANSWER_TIME_INTIAL : time );
		}
		data.answerTimes = newAnswerTimes;	
	}

	void RecordAnswerTime (float timeRequired) {
		data.answerTimes.Add( timeRequired );
		if (data.answerTimes.Count > QuestionPersistentData.NUM_ANSWER_TIMES_TO_RECORD) {
			data.answerTimes.RemoveRange( 0, data.answerTimes.Count - QuestionPersistentData.NUM_ANSWER_TIMES_TO_RECORD );
		}
	}

	float GetAdjustedTime (float timeRequired) {
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

[System.Serializable]
public class QuestionPersistentData {
	public int idx;
	public bool wasMastered;
	// even if it is no longer mastered. This is for awarding rocket parts
	public bool wasWrong;
	// if a question is answered wrong, then wasWrong is true until it is next asked
	public bool isNew = true;
	public bool gaveUp;
	public List<float> answerTimes;

	[System.NonSerialized] public const int NUM_ANSWER_TIMES_TO_RECORD = 3;
	[System.NonSerialized] public const float ANSWER_TIME_INTIAL = Question.FAST_TIME + 0.01f;

	string prefsKey;

	public void Load (string _prefsKey, int _idx) {
		prefsKey = _prefsKey;
		idx = _idx;
		answerTimes = GetAnswerTimes( prefsKey );
		wasMastered = MDPrefs.GetBool( prefsKey + ":wasMastered" );
		wasWrong = MDPrefs.GetBool( prefsKey + ":wasWrong" );
		isNew = MDPrefs.GetBool( prefsKey + ":isNew", true );
		gaveUp = MDPrefs.GetBool( prefsKey + ":gaveUp" );
	}

	public void Create (string _prefsKey, int _idx) {
		prefsKey = _prefsKey;
		idx = _idx;
		answerTimes = GetNewAnswerTimes();
	}

	public void Save (string _prefsKey = "") {
		if (_prefsKey == "") {
			_prefsKey = prefsKey;
		}
		UnityEngine.Assertions.Assert.AreNotEqual( _prefsKey.Length, 0 );
		SetAnswerTimes( _prefsKey, answerTimes );
		MDPrefs.SetBool( _prefsKey + ":wasMastered", wasMastered );
		MDPrefs.SetBool( _prefsKey + ":wasWrong", wasWrong );
		MDPrefs.SetBool( _prefsKey + ":isNew", isNew );
		MDPrefs.SetBool( _prefsKey + ":gaveUp", gaveUp );
	}

	static List<float> GetAnswerTimes (string prefsKey) {
		return MDPrefs.GetFloatArray( prefsKey + ":times" ).ToList();
	}

	static void SetAnswerTimes (string prefsKey, List<float> answerTimes) {
		MDPrefs.SetFloatArray( prefsKey + ":times", answerTimes.ToArray() );
	}

	static List<float> GetNewAnswerTimes () {
		List<float> answerTimes = new List<float>();
		for (int i = 0; i < NUM_ANSWER_TIMES_TO_RECORD; ++i) {
			answerTimes.Add( ANSWER_TIME_INTIAL );
		}
		return answerTimes;
	}
}
