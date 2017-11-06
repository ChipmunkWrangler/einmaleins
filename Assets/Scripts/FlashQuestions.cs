using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlashQuestions : Questions {
	[SerializeField] Goal goal = null;

	public const int ASK_LIST_LENGTH = 10;
	const int GAUNTLET_ASK_LIST_LENGTH = 45;
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
		Goal.CurGoal curGoal = goal.calcCurGoal();
		UnityEngine.Assertions.Assert.IsTrue (curGoal == Goal.CurGoal.FLY_TO_PLANET || curGoal == Goal.CurGoal.GAUNTLET || curGoal == Goal.CurGoal.WON || curGoal == Goal.CurGoal.COLLECT_PARTS, "unexpected goal " + curGoal);
		int askListLength = (curGoal == Goal.CurGoal.GAUNTLET) ? GAUNTLET_ASK_LIST_LENGTH : ASK_LIST_LENGTH;
		bool allowFlashMastered = curGoal == Goal.CurGoal.GAUNTLET || curGoal == Goal.CurGoal.WON;
//		if (!allowFlashMastered) {
//			askListLength -= numFlashMasteredForThisPlanet; URGH
//		}
		toAsk = questions.Where(q => q.isFlashQuestion && (allowFlashMastered || !IsFlashMastered(q))).OrderBy (q => q.IsMastered()).ThenByDescending(q => q.GetAverageAnswerTime ()).Take (askListLength).Select(q => q.idx).ToList();
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

	bool IsFlashMastered (Question q) {
		return q.GetAverageAnswerTime () <= FlashThrust.targetAnswerTime;
	}


}
