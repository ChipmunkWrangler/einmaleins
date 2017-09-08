using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashThrust : MonoBehaviour, OnCorrectAnswer {
	[SerializeField] Text heightText = null;
	[SerializeField] Text maxHeightText = null;
	[SerializeField] Text speedText = null;
	[SerializeField] ScrollBackground background = null;
	[SerializeField] float accelerationOnCorrect = 10; // total speed increase per correct answer.
	float height; // km
	float maxHeight; // km
	float speed; // km per second
	const string prefsKey = "maxHeight";
	const string numFormat = "N0";
	const string unit = " km";

	void Start() {
		heightText.text = "0";
		speedText.text = "0";
		maxHeight = MDPrefs.GetFloat (prefsKey, 0);
		maxHeightText.text = maxHeight.ToString (numFormat) + unit;
	}

	void Update() {
		speedText.text = speed.ToString ();
		height += speed * Time.deltaTime;
		heightText.text = height.ToString (numFormat) + unit;
		if (height > maxHeight) {
			maxHeight = height;
			MDPrefs.SetFloat (prefsKey, maxHeight);
			maxHeightText.text = maxHeight.ToString (numFormat) + unit;
		}
	}

	public void OnCorrectAnswer(Question question) {
//		StopAllCoroutines ();
//		int oldSpeed = speed;
		speed += accelerationOnCorrect;
		background.SetRocketSpeed(speed);
//		IncreaseHeightBy (multiplier * question.a * question.b);
	}


//	Coroutine UpdateScoreDisplay(Text text, int newScore) {
//		int oldScore = 0;
//		if (int.TryParse(text.text, out oldScore)) {
//			return StartCoroutine (CountTextUp (text, oldScore, newScore));
//		} else {
//			text.text = newScore.ToString();
//		}
//		return null;
//	}
//
//	IEnumerator CountTextUp(Text text, int oldScore, int newScore) {
//		if (newScore > oldScore) {
//			yield return new WaitForSeconds (delay);
//			float numbersPerSec = (newScore - oldScore) / thrustDuration;
//			for (int i = oldScore; i <= newScore; i += Mathf.CeilToInt (Time.deltaTime * numbersPerSec)) {
//				text.text = i.ToString ();
//				yield return null;
//			}
//			text.text = newScore.ToString();
//		}
//	}
}
