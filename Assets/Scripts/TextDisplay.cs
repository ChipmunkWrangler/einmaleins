using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisplay : MonoBehaviour {
	UnityEngine.UI.Text textField;

	protected void SetText(string text) {
		GetTextField ().text = text;
	}

	protected UnityEngine.UI.Text GetTextField() {
		if (textField == null) {
			textField = gameObject.GetComponent<UnityEngine.UI.Text> ();
			UnityEngine.Assertions.Assert.IsNotNull (textField);
		}
		return textField;
	}
}
