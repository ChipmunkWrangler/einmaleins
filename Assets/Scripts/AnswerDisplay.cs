using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerDisplay : TextDisplay, OnQuestionChanged, OnWrongAnswer {
	[SerializeField] QuestionPicker answerHandler;
	[SerializeField] float fadeTime;
	[SerializeField] int maxDigits;
	[SerializeField] GameObject[] subscribers;
	List<OnAnswerChanged> onAnswerChangedSubscribers;
	string answerTxt;
	Color oldColor;

	void Start() {
		oldColor = GetTextField ().color;
		SplitSubscribers ();
	}

	public void OnQuestionChanged(Question question) {
		answerTxt = "";
		UpdateText ();
	}

	public void OnWrongAnswer() {
		GetTextField().color = oldColor;
		StopAllCoroutines();
		StartCoroutine(Fade());
	}

	IEnumerator Fade() {
		GetTextField ().CrossFadeColor (Color.clear, fadeTime, false, true);
		yield return new WaitForSeconds (fadeTime);
		answerTxt = "";
		UpdateText ();
		GetTextField ().CrossFadeColor (oldColor, 0, false, true);
	}
		
	public void OnAnswerChanged(string nextDigit) {
		answerTxt += nextDigit;
		if (answerTxt.Length > maxDigits) {
			answerTxt = answerTxt.Substring (1, answerTxt.Length-1);
		}

		UpdateText ();
	}

	public void OnBackspace() {
		answerTxt = answerTxt.Substring(0, answerTxt.Length - 1);
		UpdateText ();
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
