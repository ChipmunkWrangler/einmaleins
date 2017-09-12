using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPicker : MonoBehaviour {
	[SerializeField] GameObject[] subscribers;
	[SerializeField] Questions questions;

	Question curQuestion;
	List<OnQuestionChanged> onQuestionChangedSubscribers;
	List<OnCorrectAnswer> onCorrectAnswerSubscribers;
	List<OnWrongAnswer> onWrongAnswerSubscribers;
	float questionTime;

	void Start () {
		SplitSubscribers ();
	}

	public void NextQuestion() {
		curQuestion = questions.GetNextQuestion ();
		foreach (OnQuestionChanged subscriber in onQuestionChangedSubscribers) {
			subscriber.OnQuestionChanged (curQuestion);
		}
		questionTime = Time.time;
	}

	public void OnAnswer(string answer) {
		if (curQuestion == null || answer.Length == 0) {
			return;
		}
		bool isCorrect = curQuestion.IsAnswerCorrect (answer);
		bool isNewlyMastered = curQuestion.UpdateState (isCorrect, Time.time - questionTime);
		if (isCorrect) {
			foreach (OnCorrectAnswer subscriber in onCorrectAnswerSubscribers) {
				subscriber.OnCorrectAnswer (curQuestion, isNewlyMastered);
			}
		} else {
			foreach (OnWrongAnswer subscriber in onWrongAnswerSubscribers) {
				subscriber.OnWrongAnswer ();
			}
		}
		questions.Save ();
	}
		
	// I can't figure out a way to get the editor to display a list of OnQuestionChangeds (since an Interface can't be Serializable)...
	private void SplitSubscribers() {
		onQuestionChangedSubscribers = new List<OnQuestionChanged> ();
		onCorrectAnswerSubscribers = new List<OnCorrectAnswer> ();
		onWrongAnswerSubscribers = new List<OnWrongAnswer> ();
		foreach(GameObject subscriber in subscribers) {
			OnQuestionChanged[] onQuestionChangeds = subscriber.GetComponents<OnQuestionChanged>();
			foreach(OnQuestionChanged s in onQuestionChangeds) {
				onQuestionChangedSubscribers.Add (s);
			}
			OnCorrectAnswer[] onCorrectAnswers = subscriber.GetComponents<OnCorrectAnswer>();
			foreach(OnCorrectAnswer s in onCorrectAnswers) {
				onCorrectAnswerSubscribers.Add (s);
			}
			OnWrongAnswer[] onWrongAnswers = subscriber.GetComponents<OnWrongAnswer>();
			foreach(OnWrongAnswer s in onWrongAnswers) {
				onWrongAnswerSubscribers.Add (s);
			}
		}
	}
}
