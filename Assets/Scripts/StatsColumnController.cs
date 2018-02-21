using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsColumnController : MonoBehaviour {
    [SerializeField] UnityEngine.UI.Image[] Cells = null;
    [SerializeField] UnityEngine.UI.Image Header = null;
    [SerializeField] UnityEngine.UI.Image RowHeader = null;
    [SerializeField] float FadeTime = 0.5F;
    [SerializeField] Color HighlightColor = Color.yellow;

    int NumMastered;
    bool IsSomethingHighlighed;

	public bool SetMasteryLevel(int row, Question q, bool seenMastered) {
		if (q.IsMastered()) {
			NumMastered++;
			if (seenMastered) {
				Cells [row].CrossFadeAlpha (0, 0, false);	
			} else {
				Cells [row].color = HighlightColor;
				Cells [row].CrossFadeAlpha (0, FadeTime, false);
				IsSomethingHighlighed = true;
			}
			seenMastered = true;
		}
		return seenMastered;
	}

	public void DoneSettingMasteryLevels() {
		if (NumMastered >= Questions.MaxNum) {
			UnityEngine.UI.Text text = Header.gameObject.GetComponentInChildren<UnityEngine.UI.Text> ();
			UnityEngine.UI.Text rowHeaderText = RowHeader.gameObject.GetComponentInChildren<UnityEngine.UI.Text> ();
			if (IsSomethingHighlighed) {
				Header.color = HighlightColor;
				Header.CrossFadeAlpha (0, FadeTime, false);
				RowHeader.color = HighlightColor;
				RowHeader.CrossFadeAlpha (0, FadeTime, false);
				text.CrossFadeAlpha (0, FadeTime, false);
				rowHeaderText.CrossFadeAlpha (0, FadeTime, false);
			} else {
				Header.CrossFadeAlpha (0, 0, false);
				RowHeader.CrossFadeAlpha (0, 0, false);
				text.text = "";
				rowHeaderText.text = "";
			}
		}
	}
}
