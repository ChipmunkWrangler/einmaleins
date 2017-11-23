using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using I2;

public class QuestionDisplay : TextDisplay, OnQuestionChanged, OnQuizAborted, OnCorrectAnswer {
	public void OnQuestionChanged(Question question) {
		string s = "";
		if (question != null) {			
			int x = Random.Range (0, 2);
			string dot = I2.Loc.LocalizationManager.GetTermTranslation ("multiplicationDot");
			s = (x == 0) ? question.a + dot + question.b : question.b + dot + question.a;
			s += " = ";
		}
		SetText(s);
	}

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		SetText ("");
	}

	public void OnQuizAborted() {
		SetText("");
	}


}
