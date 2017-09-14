using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashThrust : MonoBehaviour, OnCorrectAnswer, OnQuestionChanged {
	[SerializeField] Text heightText = null;
	[SerializeField] Text recordHeightText = null;
	[SerializeField] Text apogeeText = null;
	[SerializeField] ScrollBackground background = null;
	[SerializeField] float[] maxAttainableHeights = null;
	[SerializeField] float targetAnswerTime = 5.0f; // If a player answers all questions correctly, each in targetAnswerTime, she reaches maxAttainableHeight -- remember to include celebration time!
	[SerializeField] KickoffLaunch launch = null;
	[SerializeField] FlashQuestions flashQuestions = null;
	[SerializeField] Celebrate celebrate;

	public float speed { get; private set; } // km per second
	public float accelerationOnCorrect { get; private set; } // total speed increase per correct answer.
	public float height { get; private set; } // km

	float gravity = 9.8f;
	float recordHeight;
	float apogee;
	bool isRunning;
	System.IFormatProvider formatProvider;
	int numAnswersGiven;
	bool noMoreQuestions;

	const string prefsKey = "recordHeight";
	const string numFormat = "N0";
	const string unit = " km";

	void Start() {
		heightText.text = "0";
		apogeeText.text = "0";
		recordHeight = MDPrefs.GetFloat (prefsKey, 0);
		recordHeightText.text = recordHeight.ToString (numFormat) + unit;
//		accelerationOnCorrect = CalcAcceleration(maxAttainableHeight, FlashQuestions.ASK_LIST_LENGTH);
		UnityEngine.Assertions.Assert.AreEqual(RocketParts.GetNumUpgrades() + 1, maxAttainableHeights.Length);
		CalcParams(maxAttainableHeights[RocketParts.GetUpgradeLevel()], FlashQuestions.ASK_LIST_LENGTH);
//		TestEquations ();
		formatProvider = MDCulture.GetCulture();
	}

	void Update() {
		if (isRunning) {
//		if (timeForNextAnswer <= Time.time && numAnswersGiven < FlashQuestions.ASK_LIST_LENGTH) {
//			Debug.Log ("Actual speed = " + speed + " height = " + height);
//			OnCorrectAnswer (null);
//			timeForNextAnswer = Time.time + targetAnswerTime * 2.0f;
//		}

			speed -= gravity * Time.deltaTime;
			if (speed < 0 && noMoreQuestions) {
				speed = 0;
				isRunning = false;
			}
			height += speed * Time.deltaTime;
			if (height < 0) {
				height = 0;
				speed = 0;
			}
			background.SetRocketSpeed (speed);
			if (height > recordHeight) {
				recordHeight = height;
				MDPrefs.SetFloat (prefsKey, recordHeight);
			}
			if (height > apogee) { 
				apogee = height;
			}
			heightText.text = height.ToString (numFormat, formatProvider) + unit;
			recordHeightText.text = recordHeight.ToString (numFormat, formatProvider) + unit;
			apogeeText.text = apogee.ToString (numFormat, formatProvider) + unit;

			if (!isRunning) {
				OnDone ();
			}
		}
	}

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		speed += accelerationOnCorrect;
		++numAnswersGiven;
	}

	public void OnQuestionChanged(Question question) {
		if (question == null) {
			noMoreQuestions = true;
		}
	}

	void OnDone() {
		noMoreQuestions = false;
		flashQuestions.wasFilled = false;
		launch.ShowLaunchButton ();
	}

	public void OnCountdown() {
		isRunning = true;
		height = 0;
		apogee = 0;
		speed = 0;
		numAnswersGiven = 0;
		celebrate.curParticleIdx = RocketParts.GetUpgradeLevel ();
	}

	void CalcParams(float maxHeight, int numChancesToAccelerate) {
		// we want the player to reach v = 0 at targetAnswerTime (=:t) seconds after getting accelerated (1)
		// Let v = accelerationOnCorrect
		// Let h = the max height from one acceleration, starting at height = 0 and v = 0
		// Then h = v * v / 2g (2)
		// and time to reach h = v / g (3)
		// But (1) => time to reach h = t, so (3) => t = v / g => g = v / t (4)
		// (2) & (4) => h = v * v / 2(v/t) = v * t / 2 (5)
		// Also, if we accelerate each time from a speed of zero, then total height (=: H) is just
		// H = n * h => h = H / n (6)
		// (5) & (6) => H / n = v * t / 2 => 2H / nt = v
		accelerationOnCorrect = 2 * maxHeight / (numChancesToAccelerate * targetAnswerTime);
		gravity = accelerationOnCorrect / targetAnswerTime;
	}

//	float CalcAcceleration(float maxHeight, int numChancesToAccelerate) {
//		const int n = FlashQuestions.ASK_LIST_LENGTH;
//		float h = maxAttainableHeight;
//		float g = gravity;
//		float t = targetAnswerTime;
//		float k = n * (n-1) * t / 2; // or ((n*n * t)/2 - (n * t)/2);
//		// solve h = (n * x * (g * (t - n * t) + n * x)) / (2 * g) for x  // equation is from TestEquations()
//		// x = -(g (sqrt((2 h n^2)/g + ((n^2 t)/2 - (n t)/2)^2) - (n^2 t)/2 + (n t)/2))/n^2
//		//   = -(g (sqrt((2 h n^2)/g + k^2) - k))/n^2
//		//   = -(g * (sqrt      ((2 * h * n^2)  / g + k^2)   - k))/n^2
//		//   = -(g * (sqrt      ( 2 * h * n^2   / g + k^2)   - k))/n^2
//		//   = -(g * (sqrt      ( 2 * h * n^2   / g + k^2)   - k))/n^2
//		float root = Mathf.Sqrt( 2 * h * n * n / g + k * k);
//		float accel = g * (k + root) / (n * n);
//		if (accel < 0) {
//			accel = g * (k - root) / (n * n);
//		}
//		UnityEngine.Assertions.Assert.IsTrue (accel > 0);
//		return accel;
//	}

	void TestEquations() {
//		accelerationOnCorrect = 2.0f * targetAnswerTime * gravity;
		Debug.Log("Acceleration = " + accelerationOnCorrect + " gravity = " + gravity);
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
