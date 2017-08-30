using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerInput : MonoBehaviour, OnQuestionChanged, OnCorrectAnswer, OnWrongAnswer {
	UnityEngine.UI.InputField inputField_;

	public void OnQuestionChanged(Question question) {
		if (question == null) {
			gameObject.SetActive (false);
		} else {
			gameObject.SetActive (true);
			Reset ();
		}
	}

	public void OnWrongAnswer() {
		GetInputField().text = "";
	}

	public void OnCorrectAnswer() {
		GetInputField().interactable = false;
	}

	void Reset() {
		GetInputField().text = "";
		GetInputField().interactable = true;
	}

	UnityEngine.UI.InputField GetInputField() {
		if (inputField_ == null) {
			inputField_ = gameObject.GetComponent<UnityEngine.UI.InputField> ();
			UnityEngine.Assertions.Assert.IsNotNull (inputField_);
		}
		return inputField_;
	}
}
