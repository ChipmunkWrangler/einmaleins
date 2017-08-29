using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPicker : MonoBehaviour {
	private Questions questions;
	[SerializeField] QuestionDisplay display;
	private Question curQuestion;

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



}
