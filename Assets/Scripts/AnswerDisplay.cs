using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerDisplay : TextDisplay, OnQuestionChanged, OnWrongAnswer, OnCorrectAnswer, OnQuizAborted, OnGiveUp {
	[SerializeField] QuestionPicker answerHandler = null;
	[SerializeField] int maxDigits = 0;
	[SerializeField] GameObject[] subscribers = null;
	List<OnAnswerChanged> onAnswerChangedSubscribers;
	string queuedTxt;
	bool isFading;
	Color oldColor;

	const float fadeTime = EnterAnswerButtonController.transitionTime;

	void Start() {
		oldColor = GetTextField ().color;
		SplitSubscribers ();
		SetText ("");
	}

	public void OnQuizAborted() {
		SetText("");
	}

	public void OnQuestionChanged(Question question) {
		SetText ("");
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
		SetText(queuedTxt);
		queuedTxt = "";
		GetTextField ().CrossFadeColor (oldColor, 0, false, true);
		isFading = false;
	}
		
	public void OnAnswerChanged(string nextDigit) {
		string s = isFading ? queuedTxt : GetText();
		s += nextDigit;
		if (s.Length > maxDigits) {
			s = s.Substring (1, s.Length-1);
		}
		if (isFading) {
			queuedTxt = s;
		} else {
			SetText(s);
			NotifySubscribers ();
		}
	}

	public void OnBackspace() {
		string answerTxt = GetText ();
		if (answerTxt.Length > 0) {
			SetText( answerTxt.Substring (0, answerTxt.Length - 1) );
			NotifySubscribers ();
		}
	}
		
	public void OnSubmitAnswer() {
		answerHandler.OnAnswer (GetText());
	}

	void NotifySubscribers() {
		bool isEmpty = GetText ().Length == 0;
		foreach (OnAnswerChanged subscriber in onAnswerChangedSubscribers) {
			subscriber.OnAnswerChanged (isEmpty);
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
