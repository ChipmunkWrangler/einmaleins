using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerInput : MonoBehaviour, OnQuestionChanged, OnCorrectAnswer, OnWrongAnswer {
	UnityEngine.UI.InputField inputField;

	void Start() {
		inputField = gameObject.GetComponent<UnityEngine.UI.InputField> ();
		UnityEngine.Assertions.Assert.IsNotNull (inputField);
	}

	public void OnQuestionChanged(Question question) {
		if (question == null) {
			gameObject.SetActive (false);
		} else {
			gameObject.SetActive (true);
			Reset ();
		}
	}

	public void OnWrongAnswer() {
		inputField.text = "";
	}

	public void OnCorrectAnswer() {
		inputField.interactable = false;
	}

	private void Reset() {
		inputField.text = "";
		inputField.interactable = true;
	}
}
