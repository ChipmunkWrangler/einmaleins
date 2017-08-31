using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerDisplay : TextDisplay, OnQuestionChanged, OnWrongAnswer {
	[SerializeField] QuestionPicker answerHandler;
	[SerializeField] float fadeTime;
	[SerializeField] int maxDigits;
	string answerTxt;
	Color oldColor;

	void Start() {
		oldColor = GetTextField ().color;
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
		print ("newAnswer" + answerTxt);

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
	}

}
