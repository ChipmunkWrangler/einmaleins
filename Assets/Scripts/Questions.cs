﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Questions : MonoBehaviour {
	public Question[] questions { get; private set; }

	public const int maxNum = 10;
	protected List<int> toAsk = new List<int>(); // list of indices in questions[]
	protected const string prefsKey = "questions";

	void Awake() {
		CreateQuestions ();
		Load ();
		FillToAsk ();
		Save ();
	}
		
	public static int GetNumQuestions() {
		return maxNum * (maxNum + 1) / 2;
	}
		
	public Question GetNextQuestion() {
		FillToAsk ();
		Question retVal = null;
		if (toAsk.Count > 0) {
			retVal = questions[toAsk [0]];
			toAsk.RemoveAt (0);
		}
		return retVal;
	}

	public bool HasEnoughFlashQuestions() { // hack
//		foreach (Question question in questions.Where(q => q.IsMastered()).OrderByDescending (q => q.GetAverageAnswerTime ())) {
//			Debug.Log (question.GetAverageAnswerTime() + " " + question);
//		}
		return questions.Count(q => q.isFlashQuestion && !q.IsFlashMastered()) >= FlashQuestions.ASK_LIST_LENGTH;
	}
		
	public virtual void Save() {
		SaveQuestionsList ();
	}

	public static void OnUpgrade() {
		MDPrefs.SetBool (prefsKey + ":wasJustUpgraded", true);
	}

	public abstract void Reset();
	public abstract void Abort();

	protected virtual void Load() {
		LoadQuestionsList ();
	}

	protected abstract void FillToAsk ();

	protected void LoadQuestionsList ()
	{
		UnityEngine.Assertions.Assert.AreEqual (MDPrefs.GetInt (prefsKey + ":ArrayLen", questions.Length), questions.Length);
		for (int i = 0; i < questions.Length; ++i) {
			questions [i].Load (prefsKey + ":" + i, i);
		}
		if (MDPrefs.GetBool (prefsKey + ":wasJustUpgraded")) {
			ConvertMasteredToFlash ();
		}
	}

	protected void SaveQuestionsList ()
	{
		MDPrefs.SetInt (prefsKey + ":ArrayLen", questions.Length);
		for (int i = 0; i < questions.Length; ++i) {
			questions [i].Save ();
		}
	}

	void CreateQuestions() {
		questions = new Question[maxNum * (maxNum+1) /2];
		int idx = 0;
		for (int a = 1; a <= maxNum; ++a) {
			for (int b = a; b <= maxNum; ++b) {
				questions [idx] = new Question (a, b);
				++idx;
			}
		}
	}

	void ConvertMasteredToFlash() {
		foreach( Question question in questions.Where(q => q.wasMastered && !q.isFlashQuestion)) {
			question.isFlashQuestion = true;
		}
		MDPrefs.SetBool (prefsKey + ":wasJustUpgraded", false);
	}
}
