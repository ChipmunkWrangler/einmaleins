using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlashQuestions : Questions {
	[SerializeField] Goal goal = null;

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
		Goal.CurGoal curGoal = goal.curGoal;
		if (curGoal == Goal.CurGoal.COLLECT_PARTS) {
			return;
		}
//		Debug.Log ("Filling list");
		var candidates = questions.Where(q => q.isFlashQuestion).OrderBy (q => q.IsMastered()).ThenByDescending(q => q.GetAverageAnswerTime ());
		UnityEngine.Assertions.Assert.IsTrue (curGoal == Goal.CurGoal.FLY_TO_PLANET || curGoal == Goal.CurGoal.GAUNTLET || curGoal == Goal.CurGoal.ORBIT || curGoal == Goal.CurGoal.WON, "unexpected goal " + curGoal);
		if (curGoal == Goal.CurGoal.GAUNTLET) {
			toAsk = candidates.Select (q => q.idx).ToList ();
		} else {
			toAsk = candidates.Take (ASK_LIST_LENGTH).Select(q => q.idx).ToList();
			// now move the easiest element to the front to avoid a discouraging start
			toAsk.Insert (0, toAsk.Last ());
			toAsk.RemoveAt (toAsk.Count - 1);
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
