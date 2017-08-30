using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerPlaceholder : MonoBehaviour, OnQuestionChanged, OnWrongAnswer {
//	System.Lazy<UnityEngine.UI.Text> textField = new System.Lazy<UnityEngine.UI.Text>(Init);

	UnityEngine.UI.Text textField_;

	public void OnQuestionChanged(Question question) {
		GetTextField().text = "ergibt...";
	}

	public void OnWrongAnswer() {
		GetTextField().text = "Versuche es noch einmal...";
	}

	UnityEngine.UI.Text GetTextField() {
		if (textField_ == null) {
			textField_ = gameObject.GetComponent<UnityEngine.UI.Text> ();
			UnityEngine.Assertions.Assert.IsNotNull (textField_);
		}
		return textField_;
	}
}
