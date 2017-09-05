using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsColumnController : MonoBehaviour {
	[SerializeField] UnityEngine.UI.Text[] cells = null;

	public void SetMasteryLevel(int row, Question.Stage stage, int numCorrect) {
		string s;
		if (stage == Question.Stage.Inactive) {
			s = "0";
			cells [row].CrossFadeAlpha (0, 0f, false);
		} else {
			cells [row].CrossFadeAlpha (1.0f, 0f, false);
			if (stage == Question.Stage.Mastered) {
				s = "*";
			} else if (stage == Question.Stage.Fast) {
				s = "F";
			} else {
				s = numCorrect.ToString ();
			}
		}
		cells [row].text = s;	
	}
}
