using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPicker : MonoBehaviour {
	private Questions questions;
	[SerializeField] QuestionDisplay display;
	private Question curQuestion;
	[SerializeField] GameObject correctFx;
	[SerializeField] UnityEngine.UI.Text placeholder;

	// Use this for initialization
	void Start () {
		questions = new Questions();
		curQuestion = PickQuestion ();
		display.DisplayQuestion (curQuestion.GetQuestionString ());
	}

	Question PickQuestion() {
		Question[] qArray = questions.GetArray ();
		return qArray[Random.Range(0, qArray.Length)];
	}

	public void OnAnswer(UnityEngine.UI.InputField input) {
		if (curQuestion.IsAnswerCorrect (input.text)) {
			correctFx.SetActive (true);
		} else {
			placeholder.text = "Versuche es noch einmal...";
			input.text = "";
		}
	}
}
