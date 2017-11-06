using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Questions : MonoBehaviour {
	public Question[] questions { get; private set; }
	public EffortTracker effortTracker = null;

	public const int maxNum = 10;

	protected List<int> toAsk = new List<int>(); // list of indices in questions[]
	protected const string prefsKey = "questions";

	void Awake() {
		CreateQuestions ();
		Load ();
	}
		
	public static int GetNumQuestions() {
		return maxNum * (maxNum + 1) / 2;
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

	public int GetAskListLength() {
		FillToAsk ();
		return toAsk.Count ();
	}

	public virtual void Save() {
		SaveQuestionsList ();
		effortTracker.Save ();
	}

	public static void OnBuildOrUpgrade() {
		MDPrefs.SetBool (prefsKey + ":wasJustUpgraded", true);
	}

	public abstract void Reset();
	public abstract void Abort();

	protected virtual void Load() {
		LoadQuestionsList ();
		effortTracker.Load ();
	}

	protected abstract void FillToAsk ();

	protected void LoadQuestionsList ()
	{
		UnityEngine.Assertions.Assert.AreEqual (MDPrefs.GetInt (prefsKey + ":ArrayLen", questions.Length), questions.Length);
		if (MDPrefs.HasKey (prefsKey + ":ArrayLen")) {
			for (int i = 0; i < questions.Length; ++i) {
				questions [i].Load (prefsKey + ":" + i, i);
			}
			if (MDPrefs.GetBool (prefsKey + ":wasJustUpgraded")) {
				ConvertMasteredToFlash ();
			}
		} else {
			for (int i = 0; i < questions.Length; ++i) {
				questions [i].Create (prefsKey + ":" + i, i);
			}
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

	void ConvertMasteredToFlash() {
		foreach( Question question in questions.Where(q => q.wasMastered && !q.isFlashQuestion)) {
			question.isFlashQuestion = true;
		}
		SaveQuestionsList ();
		MDPrefs.SetBool (prefsKey + ":wasJustUpgraded", false);
	}
}
