using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour {
	[SerializeField] StatsColumnController[] columns = null;
	Questions questions;


	void Start () {
		questions = new Questions ();
		foreach (Question question in questions.questions) {
			print (question);
			columns [question.a - 1].SetMasteryLevel (question.b - 1, question.stage, question.GetMasteryFraction());
			columns [question.b - 1].SetMasteryLevel (question.a - 1, question.stage, question.GetMasteryFraction());
		}
	}
}
