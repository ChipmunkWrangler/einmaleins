using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnWrong : MonoBehaviour, OnWrongAnswer, OnGiveUp {
	[SerializeField] float timeToHide = 0;

	const float transitionTime = EnterAnswerButtonController.transitionTime;

	public void OnWrongAnswer(bool wasNew) {
		ScaleDown ();
		ScaleUpAfterDelay ();
	}

	public void OnGiveUp(Question question) {
		ScaleDown ();
	}

	void ScaleDown() {
		iTween.ScaleTo (gameObject, iTween.Hash ("scale", Vector3.zero, "easeType", iTween.EaseType.easeInSine, "time", transitionTime));
	}

	void ScaleUpAfterDelay() {
		iTween.ScaleTo (gameObject, iTween.Hash ("scale", Vector3.one, "easeType", iTween.EaseType.easeInSine, "time", transitionTime, "delay", timeToHide + transitionTime));
	}
}
