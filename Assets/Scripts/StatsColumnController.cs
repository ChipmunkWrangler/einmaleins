using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsColumnController : MonoBehaviour {
	[SerializeField] UnityEngine.UI.Image[] cells = null;
	[SerializeField] UnityEngine.UI.Image header = null;
	[SerializeField] UnityEngine.UI.Image rowHeader = null;
	[SerializeField] float premasteredAlpha = 0.5f;
	[SerializeField] float fadeTime = 0.5f;
	[SerializeField] Color highlightColor = Color.yellow;

	int numMastered;
	bool isSomethingHighlighed;

	public int SetMasteryLevel(int row, Question.Stage stage, int correctInARow, int levelSeen) {
		int newLevelSeen = levelSeen;
		float alpha = 1.0f;
		if (stage != Question.Stage.Inactive) {
			if (stage == Question.Stage.Mastered) {
				alpha = 0;
				newLevelSeen = Question.CORRECT_BEFORE_MASTERED + 1;
				++numMastered;
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
			isSomethingHighlighed = true;
		}
		return newLevelSeen;
	}

	public void DoneSettingMasteryLevels() {
		if (numMastered >= Questions.maxNum) {
			UnityEngine.UI.Text text = header.gameObject.GetComponentInChildren<UnityEngine.UI.Text> ();
			UnityEngine.UI.Text rowHeaderText = rowHeader.gameObject.GetComponentInChildren<UnityEngine.UI.Text> ();
			if (isSomethingHighlighed) {
				header.color = highlightColor;
				header.CrossFadeAlpha (0, fadeTime, false);
				rowHeader.color = highlightColor;
				rowHeader.CrossFadeAlpha (0, fadeTime, false);
				text.CrossFadeAlpha (0, fadeTime, false);
				rowHeaderText.CrossFadeAlpha (0, fadeTime, false);
			} else {
				header.CrossFadeAlpha (0, 0, false);
				rowHeader.CrossFadeAlpha (0, 0, false);
				text.text = "";
				rowHeaderText.text = "";
			}
		}
	}
		
	public float GetMasteryFraction(int correctInARow) {
		return correctInARow / (float)Question.CORRECT_BEFORE_MASTERED;
	}
}
