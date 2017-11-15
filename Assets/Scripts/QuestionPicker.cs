﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPicker : MonoBehaviour {
	[SerializeField] GameObject[] subscribers = null;
	[SerializeField] EffortTracker effortTracker = null;

	Question curQuestion;
	List<OnQuestionChanged> onQuestionChangedSubscribers;
	List<OnCorrectAnswer> onCorrectAnswerSubscribers;
	List<OnWrongAnswer> onWrongAnswerSubscribers;
	List<OnQuizAborted> onQuizAbortedSubscribers;
	float questionTime;

	void Start () {
		SplitSubscribers ();
	}

	public void AbortQuiz() {
		foreach (OnQuizAborted subscriber in onQuizAbortedSubscribers) {
			subscriber.OnQuizAborted ();
		}
		StopAllCoroutines ();
	}
		
	public void NextQuestion() {
		curQuestion = effortTracker.GetQuestion ();
		foreach (OnQuestionChanged subscriber in onQuestionChangedSubscribers) {
			subscriber.OnQuestionChanged (curQuestion);
		}
		questionTime = Time.time;
//			StartCoroutine (AutoAnswer());
	}

//	IEnumerator AutoAnswer() {
//		yield return new WaitForSeconds (Question.FAST_TIME);
//		if (curQuestion != null) {
//			OnAnswer ((curQuestion.a * curQuestion.b).ToString ());
//		}
//	}

	public void OnAnswer(string answer) {
		if (curQuestion == null || answer.Length == 0) {
			return;
		}
		bool isCorrect = curQuestion.IsAnswerCorrect (answer);
		float answerTime = Time.time - questionTime;
		HandleAnswer (isCorrect, answerTime);
		effortTracker.Save ();
	}
		
	// I can't figure out a way to get the editor to display a list of OnQuestionChangeds (since an Interface can't be Serializable)...
	private void SplitSubscribers() {
		onQuestionChangedSubscribers = new List<OnQuestionChanged> ();
		onCorrectAnswerSubscribers = new List<OnCorrectAnswer> ();
		onWrongAnswerSubscribers = new List<OnWrongAnswer> ();
		onQuizAbortedSubscribers = new List<OnQuizAborted> ();
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
			OnQuizAborted[] onQuizAborteds = subscriber.GetComponents<OnQuizAborted>();
			foreach(OnQuizAborted s in onQuizAborteds) {
				onQuizAbortedSubscribers.Add (s);
			}

		}
	}

	void HandleAnswer (bool isCorrect, float answerTime)
	{
		bool wasNew = curQuestion.isNew;
		bool isNewlyMastered = curQuestion.Answer (isCorrect, answerTime);
		if (isCorrect) {
			foreach (OnCorrectAnswer subscriber in onCorrectAnswerSubscribers) {
				subscriber.OnCorrectAnswer (curQuestion, isNewlyMastered);
			}
		}
		else {
			foreach (OnWrongAnswer subscriber in onWrongAnswerSubscribers) {
				subscriber.OnWrongAnswer (wasNew);
			}
		}
	} 
}
