using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPartCounter : MonoBehaviour, OnCorrectAnswer, OnQuestionChanged {
	[SerializeField] UnityEngine.UI.Text numText = null;
	[SerializeField] UnityEngine.UI.Text labelText = null;
	[SerializeField] Color highlightColour;
	[SerializeField] float highlightFadeTime;
	int rocketParts;
	const string prefsKey = "rocketParts";
	Color baseColor;

	void Awake() {
		baseColor = numText.color;
	}

	void Start () {
		rocketParts  = MDPrefs.GetInt (prefsKey, 0);
		UpdateText ();
	}

	public void OnQuestionChanged(Question question) {
		StartCoroutine (FadeText (baseColor, highlightFadeTime, numText));
		StartCoroutine (FadeText (baseColor, highlightFadeTime, labelText));
	}

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		if (isNewlyMastered) {
			++rocketParts;
			StartCoroutine (FadeText (highlightColour, highlightFadeTime, numText));
			StartCoroutine (FadeText (highlightColour, highlightFadeTime, labelText));
			MDPrefs.SetInt (prefsKey, rocketParts);
			UpdateText ();
		}
	}

	IEnumerator FadeText(Color end, float fadeTime, UnityEngine.UI.Text text) {
		Color start = text.color;
		float startTime = Time.time;
		float t = 0;
		while (t <= 1.0f) {
			text.color = Color.Lerp (start, end, t);
			t = (Time.time - startTime) / fadeTime;
			Debug.Log (text.color + " " + t);
			yield return null;
		}
	}

	void UpdateText () {
		numText.text = rocketParts.ToString();
	}


}
