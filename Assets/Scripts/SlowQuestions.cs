using System.Collections;
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
public class SlowQuestions : Questions {
	const int MIN_INITIAL_ASK_LIST_LENGTH = 10;
	const int TIMES_TO_REPEAT_HARDS = 3;
	const int TIMES_TO_REPEAT_OKS = 3;
	const int TIMES_TO_REPEAT_FASTS = 1;
	const int TIMES_TO_ADD_NEW = 3;
	int toAskListNum = -1; // -1: no lists shown today, toAsk not generated. 0 => toAsk is today's first list. etc.

	public override void Save() {
		base.Save();
		SaveAskList (prefsKey + ":askList:");
	}
		
	protected override void Load() {
		base.Load();
		LoadAskList (prefsKey + ":askList:");
	}

	protected override void FillToAsk() {
		if (toAsk.Count > 0) {
			Debug.Log ("AskList count " + toAsk.Count);
			return;
		}
		++toAskListNum;
		Debug.Log ("Filling list number " + toAskListNum);
		AppendByStage (Question.Stage.Wrong);
		if (toAskListNum < TIMES_TO_REPEAT_HARDS) {
			AppendByStage (Question.Stage.Hard);
		}
		if (toAskListNum < TIMES_TO_REPEAT_OKS) {
			AppendByStage (Question.Stage.Ok);
		}
		if (toAskListNum < TIMES_TO_REPEAT_FASTS) {
			AppendByStage (Question.Stage.Fast);
		}
		if (toAskListNum < TIMES_TO_ADD_NEW) {
			for (int i = 0; i < questions.Length && toAsk.Count < MIN_INITIAL_ASK_LIST_LENGTH; ++i) {
				if (questions [i].stage == Question.Stage.Inactive) {
					toAsk.Add (i);
				}
			}
		}
		foreach (int qIdx in toAsk) {
			Debug.Log (questions [qIdx]);
		}
		foreach (var question in questions) {
			if (question.stage == Question.Stage.Wrong) {
				question.stage = Question.Stage.Hard;
			}
		}
	}

	void AppendByStage (Question.Stage stage)
	{
		for (int i = 0; i < questions.Length; ++i) {
			if (questions [i].stage == stage) {
				toAsk.Add (i);
			}
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
}
