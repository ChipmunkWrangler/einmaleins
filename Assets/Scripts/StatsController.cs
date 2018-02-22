using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour {
    [SerializeField] StatsColumnController[] columns = null;
    [SerializeField] Questions questions = null;
    readonly StatsControllerPersistentData Data = new StatsControllerPersistentData();

    void Start () {
		Data.Load (columns.Length);
		foreach (Question question in questions.QuestionArray) {
			int i = question.A - 1;
			int j = question.B - 1;
			Data.seenMastered[i][j] = columns [i].SetMasteryLevel (j, question, Data.seenMastered[i][j]);
			if (i != j) {
				Data.seenMastered [j] [i] = columns [j].SetMasteryLevel (i, question, Data.seenMastered [j] [i]);
			}
		}
		foreach (StatsColumnController column in columns) {
			column.DoneSettingMasteryLevels ();
		}
		Data.Save (columns.Length);
	}
}

public class StatsControllerPersistentData {
	public bool[][] seenMastered;

	const string prefsKey = "seenMastered";

	public void Load(int numMax) {
		seenMastered = new bool[numMax] [];
		for (int i = 0; i < numMax; ++i) {
			string key = prefsKey + ":" + i;
			seenMastered [i] = MDPrefs.GetBoolArray (key);
			if (seenMastered [i].Length == 0) {
				seenMastered [i] = new bool[numMax];
			}
		}
	}

	public void Save(int numMax) {
		for (int i = 0; i < numMax; ++i) {
			MDPrefs.SetBoolArray (prefsKey + ":" + i, seenMastered [i]);
		}
	}
}