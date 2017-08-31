using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnWrong : MonoBehaviour, OnWrongAnswer {
	[SerializeField] float transitionTime = 0.5f;
	[SerializeField] float timeToHide = 0;

	public void OnWrongAnswer() {
		iTween.ScaleTo(gameObject, iTween.Hash( "scale", Vector3.zero, "easeType", iTween.EaseType.easeInSine, "time", transitionTime));
		iTween.ScaleTo(gameObject, iTween.Hash( "scale", Vector3.one, "easeType", iTween.EaseType.easeInSine, "time", transitionTime, "delay", timeToHide + transitionTime));
	}
}
