using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextButton : MonoBehaviour {
    [SerializeField] Button Button = null;
    [SerializeField] float FadedAlpha = 0.5F;
    [SerializeField] float TransitionTime = 0.1F;
    [SerializeField] Text Text = null;
    bool WasInteractable = true;

	public void SetActive(bool b) {
		gameObject.SetActive (b);
	}

	public void SetText(string newText) {
		GetComponentInChildren<UnityEngine.UI.Text> ().text = newText;
	}

	void Update() {
		if (WasInteractable != Button.interactable) {
			WasInteractable = Button.interactable;
			FadeTo (WasInteractable ? 1.0F : FadedAlpha);
		}
	}

	void FadeTo(float tgtAlpha) {		
		Text.CrossFadeAlpha (tgtAlpha, TransitionTime, false);
	}
}



