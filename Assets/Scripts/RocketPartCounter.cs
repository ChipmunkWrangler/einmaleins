using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketPartCounter : MonoBehaviour, IOnQuestionChanged, IOnQuizAborted {
    [SerializeField] Text NumText = null;
    [SerializeField] Image[] ImagesToHighlight = null;
    [SerializeField] Text[] TextsToHighlight = null;
    [SerializeField] Text[] TextsToFadeOut = null;
    [SerializeField] int BaseFontSize = 0;
    [SerializeField] Color HighlightColour = Color.yellow;
    [SerializeField] int HighlightFontSize = 0;
    [SerializeField] float HighlightFadeTime = 0.5F;
    [SerializeField] float ScoreCountdownDuration = 5.0F;
    [SerializeField] float ScoreCountdownDelay = 0.5F;

    Color[] BaseColor;

	void Awake() {
		BaseColor = new Color[TextsToHighlight.Length];
		int i = 0;
		foreach (Text text in TextsToHighlight) {
			BaseColor [i++] = text.color;
		}
	}

	void Start () {
		UpdateText (RocketParts.Instance.NumParts);
	}

	public void OnQuestionChanged(Question question) {
		Unhighlight ();
	}

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		if (isNewlyMastered) {
			RocketParts.Instance.Inc ();
			foreach (Image image in ImagesToHighlight) {
				StartCoroutine (FadeImage (HighlightColour, HighlightFadeTime, image));
			}
			foreach (Text text in TextsToHighlight) {
				StartCoroutine (FadeText (HighlightColour, HighlightFadeTime, text));
			}
			foreach (Text text in TextsToHighlight) {
				StartCoroutine (Scale (HighlightFontSize, HighlightFadeTime, text));
			}
			UpdateText (RocketParts.Instance.NumParts);
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
		foreach (Text text in TextsToHighlight) {
			StartCoroutine (FadeText (BaseColor [i++], HighlightFadeTime, text));
		}
		foreach (Text text in TextsToHighlight) {
			StartCoroutine (Scale (BaseFontSize, HighlightFadeTime, text));
		}
	}

	IEnumerator FadeText(Color end, float fadeTime, Text text) {
		Color startColor = text.color;
		float startTime = Time.time;
		float t = 0;
		while (t <= 1.0F) {
			text.color = Color.Lerp (startColor, end, t);
			t = (Time.time - startTime) / fadeTime;
			yield return null;
		}
	}

	IEnumerator Scale(int endFontSize, float tweenTime, Text text) {
		int startFontSize = text.fontSize;
		float startTime = Time.time;
		float t = 0;
		while (t <= 1.0F) {
			text.fontSize = Mathf.RoundToInt(Mathf.Lerp (startFontSize, endFontSize, t));
			t = (Time.time - startTime) / tweenTime;
			yield return null;
		}
	}

	IEnumerator FadeImage(Color end, float fadeTime, Image image) {
		Color startColor = image.color;
		float startTime = Time.time;
		float t = 0;
		while (t <= 1.0F) {
			image.color = Color.Lerp (startColor, end, t);
			t = (Time.time - startTime) / fadeTime;
			yield return null;
		}
	}

	void UpdateText (int numParts) {
		if (RocketParts.Instance.UpgradeLevel >= RocketParts.Instance.MaxUpgradeLevel - 1 && numParts <= 0) { // the numParts check is for counting down following the final upgrade. -1 is because the final upgrade had hidden rocket parts
			if (NumText.text.Length > 0) {
				NumText.text = "";
				foreach (Text text in TextsToFadeOut) {
					text.text = "";
				}
			}
		} else {
			NumText.text = I2.Loc.LocalizationManager.GetTermTranslation ("numRocketParts").Replace("{[numParts]}", numParts.ToString()).Replace("{[numPartsRequired]}", RocketParts.Instance.NumPartsRequired.ToString());
		}
	}

	IEnumerator CountTextDown(int oldScore, int newScore) {
		yield return new WaitForSeconds (ScoreCountdownDelay);
		float secsPerNum = Mathf.Abs(ScoreCountdownDuration / (float)(newScore - oldScore));
		for (int i = oldScore; i >= newScore; --i) {
			UpdateText( i );
			yield return new WaitForSeconds(secsPerNum);
		}
		UpdateText( newScore );
	}
}
