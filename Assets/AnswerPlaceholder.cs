using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerPlaceholder : MonoBehaviour, OnQuestionChanged, OnWrongAnswer {
	UnityEngine.UI.Text textField;

	void Start() {
		textField = gameObject.GetComponent<UnityEngine.UI.Text> ();
		UnityEngine.Assertions.Assert.IsNotNull (textField);
	}

	public void OnQuestionChanged(Question question) {
		textField.text = "ergibt...";
	}

	public void OnWrongAnswer() {
		textField.text = "Versuche es noch einmal...";
	}
}
