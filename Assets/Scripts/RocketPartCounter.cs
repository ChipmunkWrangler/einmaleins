using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketPartCounter : MonoBehaviour, OnCorrectAnswer, OnQuestionChanged {
	[SerializeField] Text numText = null;
	[SerializeField] Image[] imagesToHighlight;
	[SerializeField] Text[] textsToHighlight;
	[SerializeField] Color highlightColour;
	[SerializeField] float highlightFadeTime;
	int rocketParts;
	const string prefsKey = "rocketParts";
	Color[] baseColor;

	void Awake() {
		baseColor = new Color[textsToHighlight.Length];
		int i = 0;
		foreach (Text text in textsToHighlight) {
			baseColor [i++] = text.color;
		}
	}

	void Start () {
		rocketParts  = MDPrefs.GetInt (prefsKey, 0);
		UpdateText ();
	}

	public void OnQuestionChanged(Question question) {
		int i = 0;
		foreach (Text text in textsToHighlight) {
			StartCoroutine (FadeText (baseColor[i++], highlightFadeTime, text));
		}
	}

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		if (isNewlyMastered) {
			++rocketParts;
			foreach (Image image in imagesToHighlight) {
				StartCoroutine (FadeImage (highlightColour, highlightFadeTime, image));
			}
			foreach (Text text in textsToHighlight) {
				StartCoroutine (FadeText (highlightColour, highlightFadeTime, text));
			}
			MDPrefs.SetInt (prefsKey, rocketParts);
			UpdateText ();
		}
	}

	IEnumerator FadeText(Color end, float fadeTime, Text text) {
		Color startColor = text.color;
		float startTime = Time.time;
		float t = 0;
		while (t <= 1.0f) {
			text.color = Color.Lerp (startColor, end, t);
			t = (Time.time - startTime) / fadeTime;
			yield return null;
		}
	}

	IEnumerator FadeImage(Color end, float fadeTime, Image image) {
		Color startColor = image.color;
		float startTime = Time.time;
		float t = 0;
		while (t <= 1.0f) {
			image.color = Color.Lerp (startColor, end, t);
			t = (Time.time - startTime) / fadeTime;
			yield return null;
		}
	}

	void UpdateText () {
		numText.text = rocketParts.ToString();
	}


}
