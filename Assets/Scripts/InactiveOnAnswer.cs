using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveOnAnswer : MonoBehaviour, OnCorrectAnswer, OnWrongAnswer, OnAnswerChanged, OnQuizAborted, OnQuestionChanged, OnGiveUp {
	[SerializeField] UnityEngine.UI.Button button = null;
	[SerializeField] bool showOnEmptyAnswer;

	public const float transitionTime = 0.25f;

	public void OnQuizAborted() {
		SetInteractibility (false);
	}

	public void OnCorrectAnswer (Question question, bool isNewlyMastered) {
		SetInteractibility (false);
	}

	public void OnWrongAnswer (bool wasNew) {
		SetInteractibility (showOnEmptyAnswer);
	}

	public void OnQuestionChanged(Question question) {
		SetInteractibility (question != null && showOnEmptyAnswer);
	}

	public void OnAnswerChanged(bool isAnswerEmpty) {
		SetInteractibility( isAnswerEmpty == showOnEmptyAnswer );
	}

	public void OnGiveUp(Question question) {
		SetInteractibility (false);
	}
		
	void SetInteractibility(bool b) {
		if (button.interactable != b) {
			button.interactable = b;
			ScaleTo (b ? Vector3.one : Vector3.zero);
		}
	}

	void ScaleTo(Vector3 tgtScale) {
		iTween.ScaleTo(gameObject, iTween.Hash( "scale", tgtScale, "easeType", iTween.EaseType.easeInSine, "time", transitionTime));
	}
}
