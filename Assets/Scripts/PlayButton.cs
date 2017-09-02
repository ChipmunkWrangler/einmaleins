using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour {
	UnityEngine.UI.Button button;

	void Start() {
		button = GetComponent<UnityEngine.UI.Button> ();
		button.interactable = false;
	}

	public void OnPlayerNameChanged(string name) {
		button.interactable = name.Length > 0;
	}
}

