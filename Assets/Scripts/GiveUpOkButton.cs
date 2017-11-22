using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveUpOkButton : MonoBehaviour, OnGiveUp {
	[SerializeField] float transitionTime = 0.5f;
	[SerializeField] UnityEngine.UI.Button button;

	void Start() {
		Hide ();
	}

	public void OnGiveUp(Question question) {
		Show();
	}

	public void Hide() {
		button.enabled = false;
		ScaleTo (Vector3.zero);
	}

	void Show() {
		button.enabled = true;
		ScaleTo (Vector3.one);
	}

	void ScaleTo(Vector3 tgtScale) {
		iTween.ScaleTo(button.gameObject, iTween.Hash( "scale", tgtScale, "easeType", iTween.EaseType.easeInSine, "time", transitionTime));
	}
}
