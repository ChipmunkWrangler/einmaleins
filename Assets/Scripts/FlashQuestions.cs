using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// The question list consists of the first N mastered questions, ordered by average response time in the last three trials.
public class FlashQuestions : Questions {
	public const int ASK_LIST_LENGTH = 10;
	public bool wasFilled { private get; set; }

	protected override void FillToAsk() {
		if (toAsk.Count > 0 || wasFilled) {
			Debug.Log ("AskList count " + toAsk.Count);
			return;
		}
		Debug.Log ("Filling list");
		toAsk = questions.Where(q => q.stage == Question.Stage.Mastered).OrderBy (q => q.GetAverageAnswerTime ()).Take (ASK_LIST_LENGTH).Select(q => q.idx).ToList();
		Debug.Log ("Questions");
		foreach (Question question in questions.Where(q => q.stage == Question.Stage.Mastered).OrderBy (q => q.GetAverageAnswerTime ())) {
			Debug.Log (question);
		}
		Debug.Log("AskList");
		foreach (int i in toAsk) {
			Debug.Log (i + ": " + questions[i]);
		}
		wasFilled = true;
	}
}
