using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketPartCounter : MonoBehaviour, OnCorrectAnswer, OnQuestionChanged {
	[SerializeField] Text numText = null;
	[SerializeField] Image[] imagesToHighlight = null;
	[SerializeField] Text[] textsToHighlight = null;
	[SerializeField] Text[] textsToFadeOut = null;
	[SerializeField] Color highlightColour;
	[SerializeField] Vector3 highlightScale;
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
		UpdateText (RocketParts.GetNumParts(), true);
	}

	public void OnQuestionChanged(Question question) {
		int i = 0;
		foreach (Text text in textsToHighlight) {
			StartCoroutine (FadeText (baseColor[i++], highlightFadeTime, text));
		}
		foreach (Text text in textsToHighlight) {
			StartCoroutine (Scale (Vector3.one, highlightFadeTime, text.gameObject));
		}
	}

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		if (isNewlyMastered) {
			RocketParts.Inc ();
			foreach (Image image in imagesToHighlight) {
				StartCoroutine (FadeImage (highlightColour, highlightFadeTime, image));
			}
			foreach (Text text in textsToHighlight) {
				StartCoroutine (FadeText (highlightColour, highlightFadeTime, text));
			}
			foreach (Text text in textsToHighlight) {
				StartCoroutine (Scale (highlightScale, highlightFadeTime, text.gameObject));
			}
			UpdateText (RocketParts.GetNumParts());
		}
	}

	public void OnSpend(int oldNumParts, int newNumParts) {
		StopAllCoroutines ();
		StartCoroutine(CountTextDown (oldNumParts, newNumParts));
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

	IEnumerator Scale(Vector3 endScale, float tweenTime, GameObject o) {
		Vector3 startScale = o.transform.localScale;
		float startTime = Time.time;
		float t = 0;
		while (t <= 1.0f) {
			o.transform.localScale = Vector3.Lerp (startScale, endScale, t);
			t = (Time.time - startTime) / tweenTime;
			yield return null;
		}
	}

	IEnumerator HideText(Text text) {
		yield return StartCoroutine (Scale (Vector3.zero, highlightFadeTime, text.gameObject));
		text.text = "";
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

	void UpdateText (int numParts, bool instant = false) {
		if (RocketParts.GetUpgradeLevel () >= RocketParts.GetNumUpgrades () && numParts == 0) {
			if (numText.text.Length > 0) {
				numText.text = "";
				foreach (Text text in textsToFadeOut) {
					if (instant) {
						text.text = "";
					} else {
						StartCoroutine (HideText (text));
					}
				}
			}
		} else {
			numText.text = numParts + " von " + RocketParts.GetNumPartsRequired ();
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
