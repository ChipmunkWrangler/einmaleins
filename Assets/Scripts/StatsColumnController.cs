using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsColumnController : MonoBehaviour {
	[SerializeField] UnityEngine.UI.Image[] cells = null;
	[SerializeField] UnityEngine.UI.Image header = null;
	[SerializeField] UnityEngine.UI.Image rowHeader = null;
	[SerializeField] float fadeTime = 0.5f;
	[SerializeField] Color highlightColor = Color.yellow;

	int numMastered;
	bool isSomethingHighlighed;

	public int SetMasteryLevel(int row, int difficulty, int minDifficultySeen) {
		int newDifficultySeen = minDifficultySeen;
		float alpha = Mathf.Clamp01 ((float)difficulty / Question.NEW_CARD_DIFFICULTY); // or clamp to premasteredAlpha below
		if (difficulty <= Question.MASTERED_DIFFICULTY) {
			numMastered++;
		}
		if (difficulty < minDifficultySeen) {
			newDifficultySeen = difficulty;
			cells [row].color = highlightColor;
			cells [row].CrossFadeAlpha (alpha, fadeTime, false);
			isSomethingHighlighed = true;
		} else {
			cells [row].CrossFadeAlpha (alpha, 0, false);	
		}
		return newDifficultySeen;
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
}
