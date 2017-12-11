using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour {
	[SerializeField] StatsColumnController[] columns = null;
	[SerializeField] Questions questions;

	StatsControllerPersistentData data = new StatsControllerPersistentData();

	void Start () {
		data.Load (columns.Length);
		foreach (Question question in questions.questions) {
			int i = question.a - 1;
			int j = question.b - 1;
			data.seenMastered[i][j] = columns [i].SetMasteryLevel (j, question, data.seenMastered[i][j]);
			if (i != j) {
				data.seenMastered [j] [i] = columns [j].SetMasteryLevel (i, question, data.seenMastered [j] [i]);
			}
		}
		foreach (StatsColumnController column in columns) {
			column.DoneSettingMasteryLevels ();
		}
		data.Save (columns.Length);
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