using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour, OnWrongAnswer, OnCorrectAnswer {
	[SerializeField] UnityEngine.UI.Text scoreText = null;
	[SerializeField] UnityEngine.UI.Text highScoreText = null;
	[SerializeField] float delay = 0.5f;
	[SerializeField] float duration = 1.0f;
	[SerializeField] int maxMultiplier = 4;
	int score;
	int multiplier;
	int highScore;
	const string prefsKey = "highScore";
	Coroutine highScoreCoroutine;
	Coroutine scoreCoroutine;

	void Start() {
		scoreText.text = "0";
		highScoreText.text = "0";
		SetHighScore (PlayerPrefs.GetInt (prefsKey));
	}

	public void OnWrongAnswer() {
		multiplier = 0;
	}

	public void OnCorrectAnswer(Question question) {
		IncrementMultiplier ();
		IncreaseScoreBy (multiplier * question.a * question.b);
	}

	void IncrementMultiplier() {
		if (multiplier < maxMultiplier) {
			++multiplier;
		}
	}

	void IncreaseScoreBy( int amt) {
		score += amt;
		if (scoreCoroutine != null) {
			StopCoroutine (scoreCoroutine);
		}
		scoreCoroutine = UpdateScoreDisplay (scoreText, score);
		if (score > highScore) {
			SetHighScore (score);
		}
	}

	void SetHighScore(int newHighScore) {
		print ("newHighScore" + newHighScore);
		highScore = newHighScore;
		PlayerPrefs.SetInt (prefsKey, highScore);
		if (highScoreCoroutine != null) {
			StopCoroutine (highScoreCoroutine);
		}
		highScoreCoroutine = UpdateScoreDisplay (highScoreText, highScore);
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
		print ("CountTextUp" + newScore);
		yield return new WaitForSeconds (delay);
		float numbersPerSec = (newScore - oldScore) / duration;
		for (int i = oldScore; i <= newScore; i += Mathf.CeilToInt(Time.deltaTime * numbersPerSec)) {
			text.text = i.ToString();
			print ("counting " + i + " of " + newScore);
			yield return null;
		}
	}
}
