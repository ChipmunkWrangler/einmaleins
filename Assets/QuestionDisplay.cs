using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionDisplay : MonoBehaviour, OnQuestionChanged {
	UnityEngine.UI.Text textField_;

	public void OnQuestionChanged(Question question) {
		if (question == null) {
			GetTextField().text = "You are done for now!";
		} else {
			GetTextField().text = question.GetQuestionString();
		}
	}

	UnityEngine.UI.Text GetTextField() {
		if (textField_ == null) {
			textField_ = gameObject.GetComponent<UnityEngine.UI.Text> ();
			UnityEngine.Assertions.Assert.IsNotNull (textField_);
		}
		return textField_;
	}

}
