using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Questions {
	private Question[] questions;
	private const string prefsKey = "questions";
	[SerializeField] private int maxNum = 3;

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
		int minPos = 0;
		for (int i = 1; i < questions.Length; ++i)
		{
			if (questions[i].GetNextTime() < questions[minPos].GetNextTime()) { minPos = i; }
		}
		return questions [minPos];
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
