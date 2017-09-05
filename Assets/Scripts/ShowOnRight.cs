﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnRight : MonoBehaviour, OnCorrectAnswer, OnQuestionChanged, OnWrongAnswer {
	[SerializeField] float transitionTime;
	[SerializeField] bool hideOnRight;
	[SerializeField] bool evenIfWrongFirst;
	bool wasWrong;

	void Start() {
		gameObject.transform.localScale = hideOnRight ? Vector3.one : Vector3.zero;
	}

	public void OnCorrectAnswer (Question question) {
		if (evenIfWrongFirst || !wasWrong) {
			ScaleTo( hideOnRight ? Vector3.zero : Vector3.one );
		}
	}

	public void OnQuestionChanged(Question question) {
		wasWrong = false;
		ScaleTo( hideOnRight == (question != null) ? Vector3.one : Vector3.zero );
	}

	public void OnWrongAnswer() {
		wasWrong = true;
	}

	void ScaleTo(Vector3 tgtScale) {
		iTween.ScaleTo(gameObject, iTween.Hash( "scale", tgtScale, "easeType", iTween.EaseType.easeInSine, "time", transitionTime));
	}
}
