﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionDisplay : MonoBehaviour, OnQuestionChanged {
	private UnityEngine.UI.Text textField;

	void Start() {
		textField = gameObject.GetComponent<UnityEngine.UI.Text> ();
		UnityEngine.Assertions.Assert.IsNotNull (textField);
	}
		
	public void OnQuestionChanged(Question question) {
		if (question == null) {
			textField.text = "You are done for now!";
		} else {
			textField.text = question.GetQuestionString();
		}
	}
}
