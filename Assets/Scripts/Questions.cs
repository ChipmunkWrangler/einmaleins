using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Questions : MonoBehaviour {
	public Question[] questions { get; private set; }

	protected const int maxNum = 10;
	protected List<int> toAsk = new List<int>(); // list of indices in questions[]
	protected const string prefsKey = "questions";

	void Awake() {
		print ("Awake");
		CreateQuestions ();
		Load ();
		FillToAsk ();
		Save ();
	}

	public Question GetNextQuestion() {
		FillToAsk ();
		Question retVal = null;
		if (toAsk.Count > 0) {
			retVal = questions[toAsk [0]];
			toAsk.RemoveAt (0);
		}
		return retVal;
	}
		
	public virtual void Save() {
		SaveQuestionsList ();
	}

	protected virtual void Load() {
		LoadQuestionsList ();
	}
	protected abstract void FillToAsk ();

	protected void LoadQuestionsList ()
	{
		UnityEngine.Assertions.Assert.AreEqual (MDPrefs.GetInt (prefsKey + ":ArrayLen", questions.Length), questions.Length);
		for (int i = 0; i < questions.Length; ++i) {
			questions [i].Load (prefsKey + ":" + i, i);
//			questions [i].Load (i);
		}
	}

	protected void SaveQuestionsList ()
	{
		MDPrefs.SetInt (prefsKey + ":ArrayLen", questions.Length);
		for (int i = 0; i < questions.Length; ++i) {
			questions [i].Save ();
		}
	}

	void CreateQuestions() {
		questions = new Question[maxNum * (maxNum+1) /2];
		int idx = 0;
		for (int a = 1; a <= maxNum; ++a) {
			for (int b = a; b <= maxNum; ++b) {
				questions [idx] = new Question (a, b);
				++idx;
			}
		}
	}
}
