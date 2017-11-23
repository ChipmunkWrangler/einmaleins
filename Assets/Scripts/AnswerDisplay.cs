using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerDisplay : TextDisplay, OnQuestionChanged, OnWrongAnswer, OnCorrectAnswer, OnQuizAborted, OnGiveUp {
	[SerializeField] QuestionPicker answerHandler;
	[SerializeField] float fadeTime;
	[SerializeField] int maxDigits;
	[SerializeField] GameObject[] subscribers;
	List<OnAnswerChanged> onAnswerChangedSubscribers;
	string answerTxt;
	string queuedTxt;
	bool isFading;
	Color oldColor;

	void Start() {
		oldColor = GetTextField ().color;
		SplitSubscribers ();
		SetText ("");
	}

	public void OnQuizAborted() {
		SetText("");
	}

	public void OnQuestionChanged(Question question) {
		answerTxt = "";
		UpdateText ();
	}

	public void OnWrongAnswer(bool wasNew) {
		GetTextField().color = oldColor;
		StopAllCoroutines();
		StartCoroutine(Fade());
	}

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		SetText ("");
	}

	public void OnGiveUp(Question question) {
		SetText (question.GetAnswer ().ToString());
	}

	IEnumerator Fade() {
		isFading = true;
		queuedTxt = "";
		GetTextField ().CrossFadeColor (Color.clear, fadeTime, false, true);
		yield return new WaitForSeconds (fadeTime);
		answerTxt = queuedTxt;
		UpdateText ();
		GetTextField ().CrossFadeColor (oldColor, 0, false, true);
		isFading = false;
	}
		
	public void OnAnswerChanged(string nextDigit) {
		string s = isFading ? queuedTxt : answerTxt;
		s += nextDigit;
		if (s.Length > maxDigits) {
			s = s.Substring (1, s.Length-1);
		}
		if (isFading) {
			queuedTxt = s;
		} else {
			answerTxt = s;
			UpdateText ();
		}
	}

	public void OnBackspace() {
		if (answerTxt.Length > 0) {
			answerTxt = answerTxt.Substring (0, answerTxt.Length - 1);
			UpdateText ();
		}
	}
		
	public void OnSubmitAnswer() {
		answerHandler.OnAnswer (answerTxt);
	}

	void UpdateText() {
		SetText (answerTxt);
		foreach (OnAnswerChanged subscriber in onAnswerChangedSubscribers) {
			subscriber.OnAnswerChanged (answerTxt.Length == 0);
		}
	}

	// I can't figure out a way to get the editor to display a list of OnQuestionChangeds (since an Interface can't be Serializable)...
	private void SplitSubscribers() {
		onAnswerChangedSubscribers = new List<OnAnswerChanged> ();
		foreach(GameObject subscriber in subscribers) {
			OnAnswerChanged[] onAnswerChangeds = subscriber.GetComponents<OnAnswerChanged>();
			UnityEngine.Assertions.Assert.AreNotEqual (onAnswerChangeds.Length, 0);				
			foreach(OnAnswerChanged s in onAnswerChangeds) {
				onAnswerChangedSubscribers.Add (s);
			}
		}
	}
}
