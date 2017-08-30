using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Questions {
	private Question[] questions;
	private const string prefsKey = "questions";
	[SerializeField] private int maxNum = 10;

	// Use this for initialization
	public Questions() {
		CreateQuestions ();
		Load ();
		for (int i = 0; i < questions.Length; ++i) {
			Debug.Log (questions [i]);
		}
		Save ();
	}

	public Question GetNextQuestion() {
		Question question = null;
		foreach (Question otherQuestion in questions) {
			if (otherQuestion.stage == Question.Stage.Active 
				&& otherQuestion.GetNextTime() <= System.DateTime.UtcNow
				&& (question == null || otherQuestion.GetNextTime() < question.GetNextTime())
			) { 
				question = otherQuestion;
			}
		}
		return question;
	}

	void CreateQuestions() {
		questions = new Question[maxNum * (maxNum+1) /2];
		int idx = 0;
		for (int a = 1; a <= maxNum; ++a) {
			for (int b = 1; b <= a; ++b) {
				questions [idx] = new Question (a, b);
				++idx;
			}
		}
		questions [0].stage = Question.Stage.Active;
	}

//	public void Reset() {
//		PlayerPrefs.DeleteKey (prefsKey);
//	}

	void Load() {
		if (PlayerPrefs.HasKey(prefsKey)) {
			UnityEngine.Assertions.Assert.AreEqual (PlayerPrefs.GetInt (prefsKey + ":ArrayLen"), questions.Length);
		}
		for (int i = 0; i < questions.Length; ++i) {
			questions [i].Load (prefsKey + ":" + i.ToString ());
		}
	}

	void Save() {
		PlayerPrefs.SetInt(prefsKey + ":ArrayLen", questions.Length);
		for(int i = 0; i < questions.Length; ++i) {
			questions [i].Save ();
		}
	}

}
