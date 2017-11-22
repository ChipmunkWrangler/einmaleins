using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using I2;

public class QuestionDisplay : TextDisplay, OnQuestionChanged, OnQuizAborted, OnCorrectAnswer {
	public void OnQuestionChanged(Question question) {
		string s = "";
		if (question != null) {			
			int x = Random.Range (0, 2);
			s = (x == 0) ? question.a + I2.Loc.LocalizationManager.GetTermTranslation( "multiplicationDot" ) + question.b : question.b + " · " + question.a;
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
