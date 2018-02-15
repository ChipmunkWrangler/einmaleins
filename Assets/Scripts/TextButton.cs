using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextButton : MonoBehaviour {
	[SerializeField] Button button = null;
	[SerializeField] float fadedAlpha = 0.5F;
	[SerializeField] float transitionTime = 0.1F;
	[SerializeField] Text text = null;
	bool wasInteractable = true;

	public void SetActive(bool b) {
		gameObject.SetActive (b);
	}

	public void SetText(string newText) {
		GetComponentInChildren<UnityEngine.UI.Text> ().text = newText;
	}

	void Update() {
		if (wasInteractable != button.interactable) {
			wasInteractable = button.interactable;
			FadeTo (wasInteractable ? 1.0F : fadedAlpha);
		}
	}

	void FadeTo(float tgtAlpha) {		
		text.CrossFadeAlpha (tgtAlpha, transitionTime, false);
	}
}



