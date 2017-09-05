﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * At the beginning of the day, make an "active" list: all the wrongs, then the hards, then the oks, then the fasts.
 * Fill the list up to N cards with new cards.
 * A wrong answer causes the card to repeat until right, then go to "wrong", regardless of its previous category
 * A right answer shifts the card depending on answer time:
  * > 1m: hard
  * <= 1m and > 10s: ok
  * <= 10s: fast, or mastered if already fast.
 * Track the number of times answered correctly since the last wrong answer. If a card is answered correctly seven times in a row, it also counts as mastered regardless of times.
 * Once the active list is empty, create a new active list with wrongs, hards, and oks, and repeat. Do this twice.
 * Now every card that is not >= fast has been shown thrice.
 * Create an active list with wrongs only. Repeat until empty.
 * If a card would be shown twice in a row, and there are still cards left or new cards available, move it one back in the list.
 */
public class Questions {
	public Question[] questions { get; private set; }

	const int MIN_INITIAL_ASK_LIST_LENGTH = 10;
	const int TIMES_TO_REPEAT_HARDS = 3;
	const int TIMES_TO_REPEAT_OKS = 3;
	const int TIMES_TO_REPEAT_FASTS = 1;
	const int TIMES_TO_ADD_NEW = 3;
	const string prefsKey = "questions";
	const int maxNum = 10;
	List<int> toAsk = new List<int>(); // list of indices in questions[]
	int toAskListNum = -1; // -1: no lists shown today, toAsk not generated. 0 => toAsk is today's first list. etc.

	public Questions() {
		CreateQuestions ();
		Load ();
		FillToAsk ();
		Save ();
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

	void AppendByStage (Question.Stage2 stage)
	{
		for (int i = 0; i < questions.Length; ++i) {
			if (questions [i].stage2 == stage) {
				toAsk.Add (i);
			}
		}
	}

	void FillToAsk() {
		if (toAsk.Count > 0) {
			Debug.Log ("AskList count " + toAsk.Count);
			return;
		}
		++toAskListNum;
		Debug.Log ("Filling list number " + toAskListNum);
		AppendByStage (Question.Stage2.Wrong);
		if (toAskListNum < TIMES_TO_REPEAT_HARDS) {
			AppendByStage (Question.Stage2.Hard);
		}
		if (toAskListNum < TIMES_TO_REPEAT_OKS) {
			AppendByStage (Question.Stage2.Ok);
		}
		if (toAskListNum < TIMES_TO_REPEAT_FASTS) {
			AppendByStage (Question.Stage2.Fast);
		}
		if (toAskListNum < TIMES_TO_ADD_NEW) {
			for (int i = 0; i < questions.Length && toAsk.Count < MIN_INITIAL_ASK_LIST_LENGTH; ++i) {
				if (questions [i].stage2 == Question.Stage2.Inactive) {
					toAsk.Add (i);
				}
			}
		}
		foreach (int qIdx in toAsk) {
			Debug.Log (questions [qIdx]);
		}
		foreach (var question in questions) {
			if (question.stage2 == Question.Stage2.Wrong) {
				question.stage2 = Question.Stage2.Hard;
			}
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
//		questions [0].stage = Question.Stage.Active;
	}

	void LoadQuestionsList ()
	{
		UnityEngine.Assertions.Assert.AreEqual (MDPrefs.GetInt (prefsKey + ":ArrayLen", questions.Length), questions.Length);
		for (int i = 0; i < questions.Length; ++i) {
			questions [i].Load (prefsKey + ":" + i);
		}
	}

	void SaveQuestionsList ()
	{
		MDPrefs.SetInt (prefsKey + ":ArrayLen", questions.Length);
		for (int i = 0; i < questions.Length; ++i) {
			questions [i].Save ();
		}
	}

	void LoadAskList(string prefix) {
		if (MDPrefs.GetDateTime (prefix + "date", System.DateTime.MinValue) < System.DateTime.Today) {
			return; // yesterday's lists are obsolete
		}
		toAskListNum = MDPrefs.GetInt (prefix + "toAskListNum", -1);
		for( int i = 0; i < MDPrefs.GetInt (prefix + "ArrayLen", -1); ++i) {
			toAsk.Add( MDPrefs.GetInt (prefix + i, -1));
		}

	}

	void SaveAskList (string prefix)
	{
		MDPrefs.SetDateTime (prefix + "date", System.DateTime.Today);
		MDPrefs.SetInt (prefix + "toAskListNum", toAskListNum);
		int len = toAsk.Count;
		MDPrefs.SetInt (prefix + "ArrayLen", len);
		for( int i = 0; i < len; ++i) {
			MDPrefs.SetInt (prefix + i, toAsk [i]);
		}
	}

	void Load() {
		LoadQuestionsList ();
		LoadAskList (prefsKey + ":askList:");
	}


	public void Save() {
		SaveQuestionsList ();
		SaveAskList (prefsKey + ":askList:");
	}

}
