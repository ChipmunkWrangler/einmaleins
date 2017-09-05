using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour, OnWrongAnswer, OnCorrectAnswer {
	[SerializeField] UnityEngine.UI.Text scoreText = null;
	[SerializeField] float delay = 0.5f;
	[SerializeField] float scoreCountUpDuration = 2.5f;
	[SerializeField] UnityEngine.UI.Image[] multiplierIcons = null;
	[SerializeField] float multiplierFadeDuration = 0.5f;
	int score;
	int multiplier;
	const string prefsKey = "score";
	Coroutine scoreCoroutine;

	void Start() {
		scoreText.text = "0";
		IncreaseScoreBy (MDPrefs.GetInt (prefsKey, 0));
		foreach (var multiplierIcon in multiplierIcons) {
			multiplierIcon.CrossFadeAlpha (0f, 0, false);
		}
	}

	public void OnWrongAnswer() {
		multiplier = 0;
		foreach (var multiplierIcon in multiplierIcons) {
			multiplierIcon.CrossFadeAlpha (0.0f, multiplierFadeDuration, false);
		}
	}

	public void OnCorrectAnswer(Question question) {
		IncrementMultiplier ();
		print (question);
		IncreaseScoreBy (multiplier * question.a * question.b);
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
		SaveScore (score);
	}

	void SaveScore(int newScore) {
		MDPrefs.SetInt (prefsKey, newScore);
	}

	Coroutine UpdateScoreDisplay(UnityEngine.UI.Text text, int newScore) {
		int oldScore = 0;
		if (int.TryParse(text.text, out oldScore)) {
			return StartCoroutine (CountTextUp (text, oldScore, newScore));
		} else {
			text.text = newScore.ToString();
		}
		return null;
	}

	IEnumerator CountTextUp(UnityEngine.UI.Text text, int oldScore, int newScore) {
		if (newScore > oldScore) {
			yield return new WaitForSeconds (delay);
			float numbersPerSec = (newScore - oldScore) / scoreCountUpDuration;
			for (int i = oldScore; i <= newScore; i += Mathf.CeilToInt (Time.deltaTime * numbersPerSec)) {
				text.text = i.ToString ();
				yield return null;
			}
		}
	}
}
