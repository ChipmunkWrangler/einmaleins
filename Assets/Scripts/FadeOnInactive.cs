using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (UnityEngine.UI.Image))]
public class FadeOnInactive : MonoBehaviour, OnQuestionChanged, OnWrongAnswer, OnAnswerChanged {
	[SerializeField] float transitionTime;
	[SerializeField] float fadedAlpha;
	UnityEngine.UI.Image image_;

	public void OnQuestionChanged(Question question) {
		FadeTo (fadedAlpha);
	}

	public void OnWrongAnswer () {
		FadeTo (fadedAlpha);
	}

	public void OnAnswerChanged(bool isAnswerEmpty) {
		FadeTo (isAnswerEmpty ? fadedAlpha : 1.0f);
	}

	void FadeTo(float tgtAlpha) {		
		GetImage().CrossFadeAlpha (tgtAlpha, transitionTime, false);
	}

	UnityEngine.UI.Image GetImage() {
		if (image_ == null) {
			image_ = GetComponent<UnityEngine.UI.Image> ();
		}
		return image_;
	}
}
