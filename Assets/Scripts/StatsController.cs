using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour {
	[SerializeField] StatsColumnController[] columns = null;
	Questions questions;
	const string prefsKey = "statsSeen";
	int[][] levelSeen;

	void Start () {
		Load ();
		questions = new Questions ();
		foreach (Question question in questions.questions) {
			int i = question.a - 1;
			int j = question.b - 1;
			levelSeen[i][j] = columns [i].SetMasteryLevel (j, question.stage, question.correctInARow, levelSeen[i][j]);
			levelSeen[j][i] = columns [j].SetMasteryLevel (i, question.stage, question.correctInARow, levelSeen[j][i]);
		}
		Save ();
	}

	void Load() {
		levelSeen = new int[columns.Length] [];
		for (int i = 0; i < columns.Length; ++i) {
			string key = prefsKey + ":" + i;
			levelSeen [i] = MDPrefs.GetIntArray (key);
			if (levelSeen [i].Length == 0) {
				levelSeen [i] = new int[columns.Length];
			}
		}
	}

	void Save() {
		for (int i = 0; i < columns.Length; ++i) {
			MDPrefs.SetIntArray (prefsKey + ":" + i, levelSeen [i]);
		}
	}
}
