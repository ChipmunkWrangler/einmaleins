using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour {
	[SerializeField] StatsColumnController[] columns = null;
	[SerializeField] Questions questions;
	const string prefsKey = "seenMastered";
	bool[][] seenMastered;

	void Start () {
		Load ();
		foreach (Question question in questions.questions) {
			int i = question.a - 1;
			int j = question.b - 1;
			seenMastered[i][j] = columns [i].SetMasteryLevel (j, question, seenMastered[i][j]);
			if (i != j) {
				seenMastered [j] [i] = columns [j].SetMasteryLevel (i, question, seenMastered [j] [i]);
			}
		}
		foreach (StatsColumnController column in columns) {
			column.DoneSettingMasteryLevels ();
		}
		Save ();
	}

	void Load() {
		seenMastered = new bool[columns.Length] [];
		for (int i = 0; i < columns.Length; ++i) {
			string key = prefsKey + ":" + i;
			seenMastered [i] = MDPrefs.GetBoolArray (key);
			if (seenMastered [i].Length == 0) {
				seenMastered [i] = new bool[columns.Length];
			}
		}
	}

	void Save() {
		for (int i = 0; i < columns.Length; ++i) {
			MDPrefs.SetBoolArray (prefsKey + ":" + i, seenMastered [i]);
		}
	}
}
