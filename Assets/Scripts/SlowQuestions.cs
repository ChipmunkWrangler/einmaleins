using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
 * Track effort and frustration of player on a given day.
 * Wrong answers are frustrating, fast answers are low effort, etc.
 * If effort < EFFORT_PER_DAY, ask another question:
  * Prefer review questions to new questions. 
  * if frustration > 0, pick the easiest nonmastered card
  * otherwise pick the hardest nonmastered card that wasn't just asked
  * See Question class for question difficulties.
*/
public class SlowQuestions : Questions, OnWrongAnswer, OnCorrectAnswer {
	const int EFFORT_NEWWRONG = 7; // N.B. Since the question is repeated until it is correct, the actual effort will be greater. Also, it will be repeated soon.
	const int EFFORT_WRONG = 1; // N.B. Since the question is repeated until it is correct, the actual effort will be greater.
	const int EFFORT_RIGHT = 3;
	const int EFFORT_FAST = 1;
	const int FRUSTRATION_WRONG = 2; // N.B. Since the question is repeated until it is correct, the net effect will be FRUSTRATION_WRONG * n - FRUSTRATION_RIGHT (or _FAST)
	const int FRUSTRATION_RIGHT = -1;
	const int FRUSTRATION_FAST = -2;
	const int EFFORT_PER_DAY = 30;
	const int MIN_FRUSTRATION = -2;
	const int MAX_FRUSTRATION = 3;
	int previousQuestionIdx = -1;
	int frustration;
	int effort;

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		if (question.LastAnswerWasFast ()) {
			effort += EFFORT_FAST;
			frustration += FRUSTRATION_FAST;
		} else {
			effort += EFFORT_RIGHT;
			frustration += FRUSTRATION_RIGHT;
		}
		frustration = Mathf.Clamp (frustration, MIN_FRUSTRATION, MAX_FRUSTRATION);
	}

	public void OnWrongAnswer(bool wasNew) {
		frustration += FRUSTRATION_WRONG;
		frustration = Mathf.Clamp (frustration, MIN_FRUSTRATION, MAX_FRUSTRATION);
		effort += wasNew ? EFFORT_NEWWRONG : EFFORT_WRONG;
	}

	public override void Save() {
		base.Save();
		MDPrefs.SetDateTime (prefsKey + ":date", System.DateTime.Today);
		MDPrefs.SetInt (prefsKey + ":frustration", frustration);
		MDPrefs.SetInt (prefsKey + ":effort", effort);
	}

	public override void Reset() {
		frustration = 0;
		effort = 0;
	}

	public void Debug_Tomorrow() {
		foreach (Question question in questions) {
			question.Debug_Tomorrow ();
		}
	}

	protected override void Load() {
		base.Load();
		if (MDPrefs.GetDateTime (prefsKey + ":date", System.DateTime.MinValue) < System.DateTime.Today) {
			Reset ();  // yesterday's data are obsolete
		} else {
			frustration = MDPrefs.GetInt (prefsKey + ":frustration", 0);
			effort = MDPrefs.GetInt (prefsKey + ":effort", 0);
		}
	}

	protected override void FillToAsk() {
		if (toAsk.Count > 0) {
			Debug.Log ("AskList count " + toAsk.Count);
			return;
		}
		Debug.Log("frustration = " + frustration + " effort = " + effort);
		int expectedUrgentEffort = questions.Count (q => q.IsUrgent()) * EFFORT_RIGHT;
		bool urgentOnly = effort + expectedUrgentEffort > EFFORT_PER_DAY;
		var candidates = questions.Where (q => IsQuestionAllowed (q, urgentOnly));
		var nonNewCandidates = candidates.Where (q => !q.IsNew ());
		Question question = null;
		if (nonNewCandidates.Any()) {
			var candidatesByDifficulty = (frustration > 0) ? nonNewCandidates.OrderBy (q => q.difficulty) : nonNewCandidates.OrderByDescending (q => q.difficulty);
			question = candidatesByDifficulty.ThenBy (q => q.reviewAt).First ();
		} else {
			question = candidates.FirstOrDefault ();
		}
		if (question != null) {
			previousQuestionIdx = question.idx;
			toAsk.Add (previousQuestionIdx);
		} else {
			previousQuestionIdx = -1;
		}
	}

	bool IsQuestionAllowed(Question q, bool urgentOnly) {
		return q.idx != previousQuestionIdx 
			&& !q.IsMastered() 
			&& (!urgentOnly || q.IsUrgent()) 
			&& (q.reviewAt <= System.DateTime.UtcNow);
	}

}
