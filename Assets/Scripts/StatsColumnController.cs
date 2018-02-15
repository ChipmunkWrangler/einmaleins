using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsColumnController : MonoBehaviour {
	[SerializeField] UnityEngine.UI.Image[] cells = null;
	[SerializeField] UnityEngine.UI.Image header = null;
	[SerializeField] UnityEngine.UI.Image rowHeader = null;
	[SerializeField] float fadeTime = 0.5F;
	[SerializeField] Color highlightColor = Color.yellow;

	int numMastered;
	bool isSomethingHighlighed;

	public bool SetMasteryLevel(int row, Question q, bool seenMastered) {
		if (q.IsMastered()) {
			numMastered++;
			if (seenMastered) {
				cells [row].CrossFadeAlpha (0, 0, false);	
			} else {
				cells [row].color = highlightColor;
				cells [row].CrossFadeAlpha (0, fadeTime, false);
				isSomethingHighlighed = true;
			}
			seenMastered = true;
		}
		return seenMastered;
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
