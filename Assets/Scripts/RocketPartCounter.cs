using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketPartCounter : MonoBehaviour, OnCorrectAnswer, OnQuestionChanged {
	[SerializeField] Text numText = null;
	[SerializeField] Image[] imagesToHighlight = null;
	[SerializeField] Text[] textsToHighlight = null;
	[SerializeField] Color highlightColour;
	[SerializeField] Vector3 highlightScale;
	[SerializeField] float highlightFadeTime = 0.5f;
	Color[] baseColor = null;

	void Awake() {
		baseColor = new Color[textsToHighlight.Length];
		int i = 0;
		foreach (Text text in textsToHighlight) {
			baseColor [i++] = text.color;
		}
	}

	void Start () {
		UpdateText ();
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
		numText.text = RocketParts.GetNumRocketParts().ToString();
	}


}
