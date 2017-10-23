using UnityEngine;

public class SlowQuestions : Questions, OnWrongAnswer, OnCorrectAnswer {
	int previousQuestionIdx = -1;
	[SerializeField] Goal goal;

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		if (question.LastAnswerWasFast ()) {
			effortTracker.HandleFastAnswer ();
		} else {
			effortTracker.HandleSlowAnswer ();
		}
	}

	public void OnWrongAnswer(bool wasNew) {
		effortTracker.HandleWrongAnswer (wasNew);
	}
		
	public override void Reset() {}

	public override void Abort() {}

	protected override void FillToAsk() {
		if (toAsk.Count > 0) {
			Debug.Log ("AskList count " + toAsk.Count);
			return;
		}
		previousQuestionIdx = -1;
		if (goal != null && goal.curGoal == Goal.CurGoal.COLLECT_PARTS) {
			Question question = effortTracker.GetQuestion (questions, previousQuestionIdx);
			if (question != null) {
				previousQuestionIdx = question.idx;
				toAsk.Add (previousQuestionIdx);
			} 
		}
	}
}
