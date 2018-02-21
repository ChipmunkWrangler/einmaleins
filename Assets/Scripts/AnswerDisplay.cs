using System.Collections;
using UnityEngine;

public class AnswerDisplay : TextDisplay, IOnQuestionChanged, IOnWrongAnswer, IOnQuizAborted, IOnGiveUp {
    [SerializeField] QuestionPicker AnswerHandler = null;
    [SerializeField] int MaxDigits = 0;
    [SerializeField] BoolEvent AnswerChanged = new BoolEvent();
    string QueuedTxt;
    bool IsFading;
    Color OldColor;

	const float fadeTime = EnterAnswerButtonController.TransitionTime;

	void Start() {
		OldColor = GetTextField ().color;
		SetText ("");
	}

	public void OnQuizAborted() {
		SetText("");
	}

	public void OnQuestionChanged(Question question) {
		SetText ("");
	}

	public void OnWrongAnswer(bool wasNew) {
		GetTextField().color = OldColor;
		StopAllCoroutines();
		StartCoroutine(Fade());
	}

	public void OnCorrectAnswer() {
		SetText ("");
	}

	public void OnGiveUp(Question question) {
		SetText (question.GetAnswer ().ToString());
	}

	IEnumerator Fade() {
		IsFading = true;
		QueuedTxt = "";
		GetTextField ().CrossFadeColor (Color.clear, fadeTime, false, true);
		yield return new WaitForSeconds (fadeTime);
		SetText(QueuedTxt);
		QueuedTxt = "";
		GetTextField ().CrossFadeColor (OldColor, 0, false, true);
		IsFading = false;
	}
		
    public void OnAddDigit(string nextDigit) {
		string s = IsFading ? QueuedTxt : GetText();
		s += nextDigit;
		if (s.Length > MaxDigits) {
			s = s.Substring (1, s.Length-1);
		}
		if (IsFading) {
			QueuedTxt = s;
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
		AnswerHandler.OnAnswer (GetText());
	}

	void NotifySubscribers() {
        AnswerChanged.Invoke(GetText().Length == 0);
	}
}
