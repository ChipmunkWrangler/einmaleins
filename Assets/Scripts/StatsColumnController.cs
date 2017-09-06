using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsColumnController : MonoBehaviour {
	[SerializeField] UnityEngine.UI.Image[] cells = null;
	[SerializeField] float premasteredAlpha = 0.5f;
	[SerializeField] float fadeTime = 0.5f;

	public void SetMasteryLevel(int row, Question.Stage stage, float masteryFraction) {
		float alpha = 1.0f;
		if (stage != Question.Stage.Inactive) {
			if (stage == Question.Stage.Mastered) {
				alpha = 0;
			} else if (stage == Question.Stage.Fast) {
				alpha = premasteredAlpha;
			} else {
				alpha = 1.0f - (1.0f - premasteredAlpha) * masteryFraction;
			}
		}
		Debug.Log ("row = " + row + " stage = " + stage.ToString () + " m = " + masteryFraction + " alpha = " + alpha);
		cells [row].CrossFadeAlpha (alpha, fadeTime, false);
	}
}
