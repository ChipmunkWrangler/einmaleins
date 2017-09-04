using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Questions {
	private Question[] questions;
	private const string prefsKey = "questions";
	private int maxNum = 10;

	// Use this for initialization
	public Questions() {
		CreateQuestions ();
		Load ();
		for (int i = 0; i < questions.Length; ++i) {
			Debug.Log (questions [i]);
		}
		Save ();
	}

	public Question GetNextQuestion(bool activeNewQuestionIfNecessary) {
		Question question = GetExistingQuestion ();
		if (question == null && activeNewQuestionIfNecessary) {
			question = GetNewQuestion ();
		}
		return question;
	}

	Question GetExistingQuestion () {
		Question question = null;
		foreach (Question otherQuestion in questions) {
			if (PreferSecondQuestion (question, otherQuestion)) {
				question = otherQuestion;
			}
		}
		return question;
	}

	bool PreferSecondQuestion(Question firstQuestion, Question secondQuestion, System.DateTime? time = null) {
		return secondQuestion.stage == Question.Stage.Active
		&& secondQuestion.GetNextTime () <= (time ?? System.DateTime.UtcNow)
		&& (firstQuestion == null || secondQuestion.GetNextTime () < firstQuestion.GetNextTime ());
	}


	Question GetNewQuestion() {
		Question ret = null;
		foreach (Question question in questions) {
			if (question.stage == Question.Stage.Inactive) {
				question.stage = Question.Stage.Active;
				ret = question;
				break;
			}
		}
		if (ret != null) {
			Debug.Log ("Adding new question " + ret);
		}
		return ret;
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
//		MDPrefs.DeleteKey (prefsKey);
//	}

	void Load() {
		if (MDPrefs.HasKey(prefsKey)) {
			UnityEngine.Assertions.Assert.AreEqual (MDPrefs.GetInt (prefsKey + ":ArrayLen"), questions.Length);
		}
		for (int i = 0; i < questions.Length; ++i) {
			questions [i].Load (prefsKey + ":" + i.ToString ());
		}
	}

	void Save() {
		MDPrefs.SetInt(prefsKey + ":ArrayLen", questions.Length);
		for(int i = 0; i < questions.Length; ++i) {
			questions [i].Save ();
		}
	}

}
