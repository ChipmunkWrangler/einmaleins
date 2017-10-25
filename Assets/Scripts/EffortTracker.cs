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
public class EffortTracker : MonoBehaviour {
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
	int _frustration;
	int frustration {
		get { return _frustration; }
		set { _frustration = Mathf.Clamp (value, MIN_FRUSTRATION, MAX_FRUSTRATION); }
	}
	int effort;
	int previousQuestionIdx = -1;
	const string prefsKey = "effortTracking";

	public void HandleFastAnswer() {
		effort += EFFORT_FAST;
		frustration += FRUSTRATION_FAST;
	}

	public void HandleSlowAnswer() {
		effort += EFFORT_RIGHT;
		frustration += FRUSTRATION_RIGHT;
	}

	public void HandleWrongAnswer(bool wasNew) {
		frustration += FRUSTRATION_WRONG;
		effort += wasNew ? EFFORT_NEWWRONG : EFFORT_WRONG;
	}

	public void Save() {
		MDPrefs.SetDateTime (prefsKey + ":date", System.DateTime.Today);
		MDPrefs.SetInt (prefsKey + ":frustration", frustration);
		MDPrefs.SetInt (prefsKey + ":effort", effort);
	}

	public void Load() {
		if (MDPrefs.GetDateTime (prefsKey + ":date", System.DateTime.MinValue) < System.DateTime.Today) {
			// yesterday's data are obsolete
			frustration = 0;
			effort = 0;
		} else {
			frustration = MDPrefs.GetInt (prefsKey + ":frustration", 0);
			effort = MDPrefs.GetInt (prefsKey + ":effort", 0);
		}
	}

	public Question GetQuestion(Question[] questions) {
		Debug.Log ("frustration = " + frustration + " effort = " + effort + " time = " + CCTime.Now ());
		Question question = null;
		int expectedUrgentEffort = questions.Count (q => q.IsUrgent ()) * EFFORT_RIGHT;
		bool urgentOnly = effort + expectedUrgentEffort >= EFFORT_PER_DAY;
		var candidates = questions.Where (q => IsQuestionAllowed (q, urgentOnly, previousQuestionIdx));
		var nonNewCandidates = candidates.Where (q => !q.IsNew ());
		if (nonNewCandidates.Any ()) {
			var candidatesByDifficulty = (frustration > 0) ? nonNewCandidates.OrderBy (q => q.difficulty) : nonNewCandidates.OrderByDescending (q => q.difficulty);
			question = candidatesByDifficulty.ThenBy (q => q.reviewAt).First ();
		} else {
			question = candidates.FirstOrDefault ();
		}
		return question;
	}

	public void SetPreviousQuestionIdx(int idx) {
		previousQuestionIdx = idx;
	}

	bool IsQuestionAllowed(Question q, bool urgentOnly, int previousQuestionIdx) {
		return q.idx != previousQuestionIdx 
			&& !q.IsMastered() 
			&& ((urgentOnly && q.IsUrgent())
				|| (!urgentOnly && q.reviewAt <= CCTime.Now()));
	}
}
