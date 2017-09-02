using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextButton : MonoBehaviour {
	public void SetActive(bool b) {
		gameObject.SetActive (b);
	}

	public void SetText(string newText) {
		GetComponentInChildren<UnityEngine.UI.Text> ().text = newText;
	}
}
