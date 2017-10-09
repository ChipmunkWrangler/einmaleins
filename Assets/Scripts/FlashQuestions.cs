using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlashQuestions : Questions {
	public const int ASK_LIST_LENGTH = 10;
	bool wasFilled;

	public override void Reset() {
		wasFilled = false;
		toAsk.Clear ();
	}

	public override void Abort() {
		toAsk.Clear ();
	}
		
	protected override void FillToAsk() {
		if (toAsk.Count > 0 || wasFilled) {
			Debug.Log ("AskList count " + toAsk.Count);
			return;
		}
//		Debug.Log ("Filling list");
		var candidates = questions.Where(q => q.isFlashQuestion).OrderBy (q => q.IsMastered()).ThenByDescending(q => q.GetAverageAnswerTime ());
		bool isFinalGauntlet = TargetPlanet.GetIdx () == TargetPlanet.heights.Length - 1;
		if (isFinalGauntlet) {
			toAsk = candidates.Select (q => q.idx).ToList ();
		} else {
			toAsk = candidates.Take (ASK_LIST_LENGTH).Select(q => q.idx).ToList();
			toAsk.Reverse(); // so that the player won't have a discouraging start
		}
//		Debug.Log ("Questions");
//		foreach (Question question in questions.Where(q => q.difficulty == Question.MASTERED_DIFFICULTY).OrderBy (q => q.GetAverageAnswerTime ())) {
//			Debug.Log (question);
//		}
//		Debug.Log("AskList");
//		foreach (int i in toAsk) {
//			Debug.Log (i + ": " + questions[i]);
//		}
		wasFilled = true;
	}
}
