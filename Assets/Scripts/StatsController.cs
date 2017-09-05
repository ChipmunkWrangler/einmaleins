using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour {
	[SerializeField] StatsColumnController[] columns;
	Questions questions;


	void Start () {
		questions = new Questions ();
		foreach (Question question in questions.questions) {
			columns [question.a - 1].SetMasteryLevel (question.b - 1, question.intervalIdx);
			columns [question.b - 1].SetMasteryLevel (question.a - 1, question.intervalIdx);
		}
	}
}
