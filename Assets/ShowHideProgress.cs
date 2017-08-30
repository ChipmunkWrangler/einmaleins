using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideProgress : MonoBehaviour, OnCorrectAnswer, OnQuestionChanged, OnWrongAnswer {
	[SerializeField] float transitionTime;
	bool wasWrong;

	void Start() {
		gameObject.transform.localScale = Vector3.zero;
	}

	public void OnCorrectAnswer (Question question) {
		if (!wasWrong) {
			Show ();
		}
	}

	public void OnQuestionChanged(Question question) {
		wasWrong = false;
		Hide ();
	}

	public void OnWrongAnswer() {
		wasWrong = true;
	}

	void Hide() {
		iTween.ScaleTo(gameObject, iTween.Hash( "scale", Vector3.zero, "easeType", iTween.EaseType.easeInSine, "time", transitionTime));
	}

	void Show() {
		iTween.ScaleTo(gameObject, iTween.Hash( "scale", Vector3.one, "easeType", iTween.EaseType.easeInSine, "time", transitionTime));
	}


}
