using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour, OnWrongAnswer, OnCorrectAnswer {
	[SerializeField] UnityEngine.UI.Text text = null;
	[SerializeField] float delay = 0.5f;
	[SerializeField] float duration = 1.0f;
	[SerializeField] int maxMultiplier = 4;
	int score;
	int multiplier;

	void Start() {
		text.text = "0";
	}

	public void OnWrongAnswer() {
		multiplier = 0;
	}

	public void OnCorrectAnswer(Question question) {
		if (multiplier < maxMultiplier) {
			++multiplier;
		}
		score += multiplier * question.a * question.b;
		UpdateScoreDisplay (score);
	}

	void UpdateScoreDisplay(int newScore) {
		int oldScore = 0;
		StopAllCoroutines ();
		if (int.TryParse(text.text, out oldScore)) {
			StartCoroutine (CountTextUp (oldScore, newScore));
		} else {
			text.text = newScore.ToString();
		}
	}

	IEnumerator CountTextUp(int oldScore, int newScore) {
		yield return new WaitForSeconds (delay);
		float numbersPerSec = (newScore - oldScore) / duration;
		for (int i = oldScore; i <= newScore; i += Mathf.CeilToInt(Time.deltaTime * numbersPerSec)) {
			text.text = i.ToString();
			yield return null;
		}
	}
}
