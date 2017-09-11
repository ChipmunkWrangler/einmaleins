using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour, OnWrongAnswer, OnCorrectAnswer {
	[SerializeField] UnityEngine.UI.Text scoreText = null;
	[SerializeField] float delay = 0.5f;
	[SerializeField] float scoreCountUpDuration = 2.5f;
	[SerializeField] UnityEngine.UI.Image[] multiplierIcons = null;
	[SerializeField] float multiplierFadeDuration = 0.5f;
	const string numFormat = "N0";
	int score;
	int shownScore;
	int multiplier;
	const string prefsKey = "score";
	Coroutine scoreCoroutine;
	System.IFormatProvider formatProvider;

	void Start() {
		scoreText.text = "0";
		IncreaseScoreBy (MDPrefs.GetInt (prefsKey, 0));
		foreach (var multiplierIcon in multiplierIcons) {
			multiplierIcon.CrossFadeAlpha (0f, 0, false);
		}
		int newMult = MDPrefs.GetInt (prefsKey + ":mult", 0);
		for (int i = 0; i < newMult; ++i) {
			IncrementMultiplier ();
		}
		formatProvider = MDCulture.GetCulture();
	}

	public void OnWrongAnswer() {
		multiplier = 0;
		foreach (var multiplierIcon in multiplierIcons) {
			multiplierIcon.CrossFadeAlpha (0.0f, multiplierFadeDuration, false);
		}
		Save (score);
	}

	public void OnCorrectAnswer(Question question) {
		IncrementMultiplier ();
		IncreaseScoreBy (multiplier * question.a * question.b);
		Save (score);
	}

	void IncrementMultiplier() {
		if (multiplier < multiplierIcons.Length) {
			++multiplier;
		}
		multiplierIcons [multiplier - 1].CrossFadeAlpha (1.0f, multiplierFadeDuration, false);
	}

	void IncreaseScoreBy( int amt) {
		score += amt;
		if (scoreCoroutine != null) {
			StopCoroutine (scoreCoroutine);
		}
		scoreCoroutine = UpdateScoreDisplay (scoreText, score);
	}

	void Save(int newScore) {
		MDPrefs.SetInt (prefsKey, newScore);
		MDPrefs.SetInt (prefsKey + ":mult", multiplier);
	}

	Coroutine UpdateScoreDisplay(UnityEngine.UI.Text text, int newScore) {
		return StartCoroutine (CountTextUp (text, newScore));
	}

	IEnumerator CountTextUp(UnityEngine.UI.Text text, int newScore) {
		if (newScore > shownScore) {
			yield return new WaitForSeconds (delay);
			float numbersPerSec = (newScore - shownScore) / scoreCountUpDuration;
			for (int i = shownScore; i <= newScore; i += Mathf.CeilToInt (Time.deltaTime * numbersPerSec)) {
				text.text = i.ToString (numFormat, formatProvider);
				shownScore = i;
				yield return null;
			}
			text.text = newScore.ToString(numFormat, formatProvider);
			shownScore = newScore;
		}
	}
}
