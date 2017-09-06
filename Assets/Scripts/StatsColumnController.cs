using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsColumnController : MonoBehaviour {
	[SerializeField] UnityEngine.UI.Image[] cells = null;
	[SerializeField] float premasteredAlpha = 0.5f;
	[SerializeField] float fadeTime = 0.5f;
	[SerializeField] Color highlightColor = Color.yellow;

	public int SetMasteryLevel(int row, Question.Stage stage, int correctInARow, int levelSeen) {
		int newLevelSeen = levelSeen;
		float alpha = 1.0f;
		if (stage != Question.Stage.Inactive) {
			if (stage == Question.Stage.Mastered) {
				alpha = 0;
				newLevelSeen = Question.CORRECT_BEFORE_MASTERED + 1;
			} else if (stage == Question.Stage.Fast) {
				alpha = premasteredAlpha;
				newLevelSeen = Question.CORRECT_BEFORE_MASTERED;
			} else {
				alpha = 1.0f - (1.0f - premasteredAlpha) * GetMasteryFraction(correctInARow);
				newLevelSeen = correctInARow;
			}
		}
		if (newLevelSeen <= levelSeen) {
			cells [row].CrossFadeAlpha (alpha, 0, false);	
		} else {
			cells [row].color = highlightColor;
			cells [row].CrossFadeAlpha (alpha, fadeTime, false);
		}
		return newLevelSeen;
	}
		
	public float GetMasteryFraction(int correctInARow) {
		return correctInARow / (float)Question.CORRECT_BEFORE_MASTERED;
	}
}
