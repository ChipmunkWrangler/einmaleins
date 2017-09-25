using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour {
	[SerializeField] StatsColumnController[] columns = null;
	[SerializeField] Questions questions;
	const string prefsKey = "statsSeen";
	int[][] minDifficultySeen;

	void Start () {
		Load ();
		foreach (Question question in questions.questions) {
			int i = question.a - 1;
			int j = question.b - 1;
			minDifficultySeen[i][j] = columns [i].SetMasteryLevel (j, question.difficulty, minDifficultySeen[i][j]);
			if (i != j) {
				minDifficultySeen [j] [i] = columns [j].SetMasteryLevel (i, question.difficulty, minDifficultySeen [j] [i]);
			}
		}
		foreach (StatsColumnController column in columns) {
			column.DoneSettingMasteryLevels ();
		}
		Save ();
	}

	void Load() {
		minDifficultySeen = new int[columns.Length] [];
		for (int i = 0; i < columns.Length; ++i) {
			string key = prefsKey + ":" + i;
			minDifficultySeen [i] = MDPrefs.GetIntArray (key, Question.NEW_CARD_DIFFICULTY);
			if (minDifficultySeen [i].Length == 0) {
				minDifficultySeen [i] = new int[columns.Length];
			}
		}
	}

	void Save() {
		for (int i = 0; i < columns.Length; ++i) {
			MDPrefs.SetIntArray (prefsKey + ":" + i, minDifficultySeen [i]);
		}
	}
}
