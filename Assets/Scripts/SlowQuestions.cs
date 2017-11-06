using UnityEngine;

public class SlowQuestions : Questions {
	[SerializeField] Goal goal;

	public override void Reset() {}

	public override void Abort() {}

	protected override void FillToAsk() {
		if (toAsk.Count > 0) {
			Debug.Log ("AskList count " + toAsk.Count);
			return;
		}
		Question question = null;
		if (goal != null && goal.calcCurGoal() == Goal.CurGoal.COLLECT_PARTS) {
			question = effortTracker.GetQuestion (questions);
		}
		if (question != null) {
			effortTracker.SetPreviousQuestionIdx (question.idx); 
			toAsk.Add (question.idx);
		} 
	}
}
