using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionDisplay : TextDisplay, OnQuestionChanged {
	public void OnQuestionChanged(Question question) {
		string s = "";
		if (question != null) {			
			int x = Random.Range (0, 2);
			s = (x == 0) ? question.a + " · " + question.b : question.b + " x " + question.a;
			s += " =";
		}
		SetText(s);
	}
}
