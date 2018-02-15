using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketPartCounter : MonoBehaviour, OnCorrectAnswer, OnQuestionChanged, OnQuizAborted {
	[SerializeField] Text numText = null;
	[SerializeField] Image[] imagesToHighlight = null;
	[SerializeField] Text[] textsToHighlight = null;
	[SerializeField] Text[] textsToFadeOut = null;
	[SerializeField] int baseFontSize = 0;
    [SerializeField] Color highlightColour = Color.yellow;
	[SerializeField] int highlightFontSize = 0;
	[SerializeField] float highlightFadeTime = 0.5f;
	[SerializeField] float scoreCountdownDuration = 5.0f;
	[SerializeField] float scoreCountdownDelay = 0.5f;

	Color[] baseColor = null;

	void Awake() {
		baseColor = new Color[textsToHighlight.Length];
		int i = 0;
		foreach (Text text in textsToHighlight) {
			baseColor [i++] = text.color;
		}
	}

	void Start () {
		UpdateText (RocketParts.instance.numParts);
	}

	public void OnQuestionChanged(Question question) {
		Unhighlight ();
	}

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		if (isNewlyMastered) {
			RocketParts.instance.Inc ();
			foreach (Image image in imagesToHighlight) {
				StartCoroutine (FadeImage (highlightColour, highlightFadeTime, image));
			}
			foreach (Text text in textsToHighlight) {
				StartCoroutine (FadeText (highlightColour, highlightFadeTime, text));
			}
			foreach (Text text in textsToHighlight) {
				StartCoroutine (Scale (highlightFontSize, highlightFadeTime, text));
			}
			UpdateText (RocketParts.instance.numParts);
		}
	}

	public void OnQuizAborted() {
		Unhighlight ();
	}

	public void OnSpend(int oldNumParts, int newNumParts) {
		StopAllCoroutines ();
		if (gameObject.activeInHierarchy) {
			StartCoroutine (CountTextDown (oldNumParts, newNumParts));
		}
	}

	void Unhighlight ()
	{
		int i = 0;
		foreach (Text text in textsToHighlight) {
			StartCoroutine (FadeText (baseColor [i++], highlightFadeTime, text));
		}
		foreach (Text text in textsToHighlight) {
			StartCoroutine (Scale (baseFontSize, highlightFadeTime, text));
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

	IEnumerator Scale(int endFontSize, float tweenTime, Text text) {
		int startFontSize = text.fontSize;
		float startTime = Time.time;
		float t = 0;
		while (t <= 1.0f) {
			text.fontSize = Mathf.RoundToInt(Mathf.Lerp (startFontSize, endFontSize, t));
			t = (Time.time - startTime) / tweenTime;
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

	void UpdateText (int numParts) {
		if (RocketParts.instance.upgradeLevel >= RocketParts.instance.maxUpgradeLevel - 1 && numParts <= 0) { // the numParts check is for counting down following the final upgrade. -1 is because the final upgrade had hidden rocket parts
			if (numText.text.Length > 0) {
				numText.text = "";
				foreach (Text text in textsToFadeOut) {
					text.text = "";
				}
			}
		} else {
			numText.text = I2.Loc.LocalizationManager.GetTermTranslation ("numRocketParts").Replace("{[numParts]}", numParts.ToString()).Replace("{[numPartsRequired]}", RocketParts.instance.numPartsRequired.ToString());
		}
	}

	IEnumerator CountTextDown(int oldScore, int newScore) {
		yield return new WaitForSeconds (scoreCountdownDelay);
		float secsPerNum = Mathf.Abs(scoreCountdownDuration / (float)(newScore - oldScore));
		for (int i = oldScore; i >= newScore; --i) {
			UpdateText( i );
			yield return new WaitForSeconds(secsPerNum);
		}
		UpdateText( newScore );
	}
}
