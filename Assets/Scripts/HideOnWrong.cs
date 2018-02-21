using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnWrong : MonoBehaviour, IOnWrongAnswer, IOnGiveUp {
    [SerializeField] float TimeToHide = 0;

    const float TransitionTime = EnterAnswerButtonController.TransitionTime;

	public void OnWrongAnswer(bool wasNew) {
		ScaleDown ();
		ScaleUpAfterDelay ();
	}

	public void OnGiveUp(Question question) {
		ScaleDown ();
	}

	void ScaleDown() {
		iTween.ScaleTo (gameObject, iTween.Hash ("scale", Vector3.zero, "easeType", iTween.EaseType.easeInSine, "time", TransitionTime));
	}

	void ScaleUpAfterDelay() {
		iTween.ScaleTo (gameObject, iTween.Hash ("scale", Vector3.one, "easeType", iTween.EaseType.easeInSine, "time", TransitionTime, "delay", TimeToHide + TransitionTime));
	}
}
