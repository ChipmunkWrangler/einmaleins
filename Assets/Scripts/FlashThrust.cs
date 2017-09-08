using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashThrust : MonoBehaviour, OnCorrectAnswer {
	[SerializeField] Text heightText = null;
	[SerializeField] Text recordHeightText = null;
	[SerializeField] Text speedText = null;
	[SerializeField] ScrollBackground background = null;
	[SerializeField] float maxAttainableHeight = 1287000000.0f; // Saturn
	[SerializeField] float targetAnswerTime = 6.0f; // If a player answers all questions correctly, each in targetAnswerTime, she reaches maxAttainableHeight -- remember to include celebration time!
	[SerializeField] float gravity = 9.8f;
	float accelerationOnCorrect; // total speed increase per correct answer.
	float height; // km
	float recordHeight;
	float speed; // km per second
	const string prefsKey = "recordHeight";
	const string numFormat = "N0";
	const string unit = " km";

	float timeForNextAnswer;
	int numAnswersGiven = 0;
	void Start() {
		heightText.text = "0";
		speedText.text = "0";
		recordHeight = MDPrefs.GetFloat (prefsKey, 0);
		recordHeightText.text = recordHeight.ToString (numFormat) + unit;
		accelerationOnCorrect = CalcAcceleration(maxAttainableHeight, FlashQuestions.ASK_LIST_LENGTH);
		TestEquations ();
	}

	void Update() {
		if (timeForNextAnswer <= Time.time && numAnswersGiven < FlashQuestions.ASK_LIST_LENGTH) {
			Debug.Log ("Actual speed = " + speed + " height = " + height);
			++numAnswersGiven;
			speed += accelerationOnCorrect;
			timeForNextAnswer = Time.time + targetAnswerTime;
		}

		speed -= gravity * Time.deltaTime;
		if (speed < 0) {
			speed = 0;
		}
		background.SetRocketSpeed(speed);
		height += speed * Time.deltaTime;
		if (height > recordHeight) {
			recordHeight = height;
			MDPrefs.SetFloat (prefsKey, recordHeight);
		}
		speedText.text = speed.ToString ();
		heightText.text = height.ToString (numFormat) + unit;
		recordHeightText.text = recordHeight.ToString (numFormat) + unit;
	}

	public void OnCorrectAnswer(Question question) {
		speed += accelerationOnCorrect;
	}

	float CalcAcceleration(float maxHeight, int numChancesToAccelerate) {
		const int n = FlashQuestions.ASK_LIST_LENGTH;
		float h = maxAttainableHeight;
		float g = gravity;
		float t = targetAnswerTime;
		float k = n * (n-1) * t / 2; // or ((n*n * t)/2 - (n * t)/2);
		// solve h = (n * x * (g * (t - n * t) + n * x)) / (2 * g) for x  // equation is from TestEquations()
		// x = -(g (sqrt((2 h n^2)/g + ((n^2 t)/2 - (n t)/2)^2) - (n^2 t)/2 + (n t)/2))/n^2
		//   = -(g (sqrt((2 h n^2)/g + k^2) - k))/n^2
		//   = -(g * (sqrt      ((2 * h * n^2)  / g + k^2)   - k))/n^2
		//   = -(g * (sqrt      ( 2 * h * n^2   / g + k^2)   - k))/n^2
		//   = -(g * (sqrt      ( 2 * h * n^2   / g + k^2)   - k))/n^2
		float root = Mathf.Sqrt( 2 * h * n * n / g + k * k);
		float accel = g * (k + root) / (n * n);
		if (accel < 0) {
			accel = g * (k - root) / (n * n);
		}
		UnityEngine.Assertions.Assert.IsTrue (accel > 0);
		return accel;
	}

	void TestEquations() {
//		accelerationOnCorrect = 2.0f * targetAnswerTime * gravity;
		Debug.Log("Acceleration = " + accelerationOnCorrect);
		float g = gravity;
		float t = targetAnswerTime;
		float decelarationByNextQuestion = g * t;
		const int n = FlashQuestions.ASK_LIST_LENGTH;
		for (int i = 0; i < n; ++i) {
			// right after you answer the question:
			speed += accelerationOnCorrect; 
			// by the time you answer the next question in targetAnswerTime:
			height += (speed - 0.5f * decelarationByNextQuestion) * t; 
			speed -= decelarationByNextQuestion;
			Debug.Log ("speed = " + speed + " height = " + height);
		}
		// height and speed at t = (targetAnswerTime after you answer the last question) = FlashQuestions.ASK_LIST_LENGTH * targetAnswerTime
		float totalAnswerTime = n * t;
		const int n1 = n + 1;
		const int gauss	 = n1 * (n1 + 1) / 2;
		height = decelarationByNextQuestion * totalAnswerTime * 0.5f + t * (accelerationOnCorrect - decelarationByNextQuestion) * (n1 * n1 - gauss);
		speed = n * accelerationOnCorrect - g * totalAnswerTime; 
		Debug.Log ("Closed form speed = " + speed + " height = " + height);
		height += speed * speed / (2.0f * g);
		float x = accelerationOnCorrect;
		float altHeight = (n * x * (g * (t - n * t) + n * x)) / (2 * g);
		float timeToStop = speed / g + totalAnswerTime;
		Debug.Log ("final height = " + height + " or " + altHeight + " time = " + timeToStop + " of which freefall = " + speed / g);
		height = 0;
		speed = 0;
	
	}
}
