using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPicker : MonoBehaviour {
	private Questions questions;
	private Question curQuestion;
	[SerializeField] GameObject[] subscribers;
	List<OnQuestionChanged> onQuestionChangedSubscribers;
	List<OnCorrectAnswer> onCorrectAnswerSubscribers;
	List<OnWrongAnswer> onWrongAnswerSubscribers;

	void Start () {
		questions = new Questions();
		SplitSubscribers ();
		NextQuestion ();
	}

	public void NextQuestion() {
		curQuestion = questions.GetNextQuestion ();
		foreach (OnQuestionChanged subscriber in onQuestionChangedSubscribers) {
			subscriber.OnQuestionChanged (curQuestion);
		}
	}

	public void OnAnswer(string answer) {
		if (curQuestion == null || answer.Length == 0) {
			return;
		}
		bool isCorrect = curQuestion.IsAnswerCorrect (answer);
		curQuestion.UpdateInterval (isCorrect);
		if (isCorrect) {
			foreach (OnCorrectAnswer subscriber in onCorrectAnswerSubscribers) {
				subscriber.OnCorrectAnswer ();
			}

		} else {
			foreach (OnWrongAnswer subscriber in onWrongAnswerSubscribers) {
				subscriber.OnWrongAnswer ();
			}
		}
	}

	// I can't figure out a way to get the editor to display a list of OnQuestionChangeds (since an Interface can't be Serializable)...
	private void SplitSubscribers() {
		onQuestionChangedSubscribers = new List<OnQuestionChanged> ();
		onCorrectAnswerSubscribers = new List<OnCorrectAnswer> ();
		onWrongAnswerSubscribers = new List<OnWrongAnswer> ();
		foreach(GameObject subscriber in subscribers) {
			OnQuestionChanged onQuestionChanged = subscriber.GetComponent<OnQuestionChanged>();
			if (onQuestionChanged != null) {
				onQuestionChangedSubscribers.Add (onQuestionChanged);
			}
			OnCorrectAnswer onCorrectAnswer = subscriber.GetComponent<OnCorrectAnswer>();
			if (onCorrectAnswer != null) {
				onCorrectAnswerSubscribers.Add (onCorrectAnswer);
			}
			OnWrongAnswer onWrongAnswer = subscriber.GetComponent<OnWrongAnswer>();
			if (onWrongAnswer != null) {
				onWrongAnswerSubscribers.Add (onWrongAnswer);
			}
		}
	}
}
