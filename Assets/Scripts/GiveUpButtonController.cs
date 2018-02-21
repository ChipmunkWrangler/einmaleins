using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveUpButtonController : MonoBehaviour, OnWrongAnswer, OnQuizAborted, OnQuestionChanged, OnGiveUp {
	[SerializeField] UnityEngine.UI.Button button = null;
	[SerializeField] UnityEngine.UI.Image image = null;

	const float transitionTime = EnterAnswerButtonController.transitionTime;

	public void OnQuizAborted() {
		SetInteractibility (false);
	}

	public void OnCorrectAnswer (Question question, bool isNewlyMastered) {
		SetInteractibility (false);
	}

	public void OnWrongAnswer (bool wasNew) {
		SetInteractibility (true);
	}

	public void OnQuestionChanged(Question question) {
		SetInteractibility (question != null);
	}

	public void OnAnswerChanged(bool isAnswerEmpty) {
		SetInteractibility (isAnswerEmpty);
	}

	public void OnGiveUp(Question question) {
		SetInteractibility (false);
	}

	void SetInteractibility(bool b) {
		if (button.interactable != b) {
			button.interactable = b;
			image.raycastTarget = b; // want to be able to tap the ok button, which is behind this one, immediately
			ScaleTo (b ? Vector3.one : Vector3.zero);
		}
	}

	void ScaleTo(Vector3 tgtScale) {
		iTween.ScaleTo(gameObject, iTween.Hash( "scale", tgtScale, "easeType", iTween.EaseType.easeInSine, "time", transitionTime));
	}

}
