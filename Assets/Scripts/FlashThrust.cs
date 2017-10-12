using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashThrust : MonoBehaviour, OnCorrectAnswer, OnQuestionChanged {
	[SerializeField] GameObject heightWidget = null;
	[SerializeField] Text heightText = null;
	[SerializeField] Text recordHeightText = null;
	[SerializeField] Text apogeeText = null;
	[SerializeField] GameObject orbitsWidget = null;
	[SerializeField] Text orbitsText = null;
	[SerializeField] Text recordOrbitsText = null;
	[SerializeField] Text apogeeOrbitsText = null;
	public const float targetAnswerTime = 6.0f; // If a player answers all questions correctly, each in targetAnswerTime, she reaches maxAttainableHeight -- remember to include celebration time!
	[SerializeField] float minSpeedFactor = 0.25f;
	[SerializeField] KickoffLaunch launch = null;
	[SerializeField] QuestionPicker questionPicker = null;
	[SerializeField] Celebrate celebrate = null;
	[SerializeField] GameObject oldRecord = null;
	[SerializeField] Params paramObj = null;
	[SerializeField] Text achievementText = null;
	[SerializeField] string recordBrokenMsg = "Neuer Rekord!";
	[SerializeField] ShowSolarSystem zoomToPlanet = null;
	[SerializeField] float achievementTextTransitionTime = 3.0f;
	[SerializeField] float planetAchievementTextDelay = 2.0f;
	[SerializeField] Questions questions = null;
	[SerializeField] float circumferanceFactor = 1000.0f;
	float minSpeed;
	float maxSpeed = float.MaxValue;
	public Renderer orbitingPlanet { get; private set; }
	public int orbitingPlanetIdx { get; private set; }
	public float speed { get; private set; } // km per second
	public float accelerationOnCorrect { get; private set; } // total speed increase per correct answer.
	public float height { get; private set; } // km
	public float orbitalDistance { get; private set; }

	readonly string[] planetReachedTexts = {
		"Mars erreicht!",
		"Jupiter erreicht!",
		"Saturn erreicht!",
		"Uranus erreicht!",
		"Neptun erreicht!",
		"Pluto erreicht!",
		"Ende erreicht!" // should never happen
	};

	static readonly float[] planetCircumferances = {
		21344.0f,
		439264.0f,
		365882.4f,
		159354.1f,
		154704.6f,
		7231.9f
	};

	float gravity = 50f;
	float recordHeight;
	float apogee;
	public float planetCircumferance { get; private set; }
	float recordOrbitalDistance;
	float orbitalApogee;
	bool isRunning;
	System.IFormatProvider formatProvider;
	int numAnswersGiven;
	bool noMoreQuestions;
	bool checkForRecord;

	const string recordPrefsKey = "recordHeight";
	const string recordOrbitalDistancePrefsKey = "recordOrbitalDistance";
	const string heightFormat = "N0";
	const string orbitFormat = "N2";
	const string unit = " km";

	void Start() {
		heightWidget.SetActive (true);
		orbitsWidget.SetActive (false);
		heightText.text = "0";
		apogeeText.text = "0";
		recordHeight = MDPrefs.GetFloat (recordPrefsKey, 0);
		recordHeightText.text = recordHeight.ToString (heightFormat) + unit;
		var recordPos = oldRecord.transform.position;
		recordPos.y += recordHeight * paramObj.heightScale * oldRecord.transform.parent.localScale.y;
		oldRecord.transform.position = recordPos;
		bool hasReachedTargetPlanet = TargetPlanet.GetLastReachedIdx() == TargetPlanet.GetTargetPlanetIdx();
		checkForRecord = recordHeight > 0 && !hasReachedTargetPlanet;
		if (!hasReachedTargetPlanet) { // new planet counts new orbits
			MDPrefs.SetFloat (recordOrbitalDistancePrefsKey, 0);
		}
		oldRecord.SetActive (checkForRecord);
		UnityEngine.Assertions.Assert.AreEqual(RocketParts.instance.numUpgrades + 1, TargetPlanet.heights.Length);
		int upgradeLevel = RocketParts.instance.upgradeLevel;
		float newTargetHeight = TargetPlanet.heights [upgradeLevel];
		float maxHeight = (upgradeLevel < TargetPlanet.heights.Length - 1) ? TargetPlanet.heights [upgradeLevel + 1] : TargetPlanet.heights [upgradeLevel] * 2.0f;
		CalcParams(maxHeight, newTargetHeight, questions.GetAskListLength() + 1); // +1 because of the initial launch acceleration
//		accelerationOnCorrect = CalcAcceleration(TargetPlanet.heights[RocketParts.instance.UpgradeLevel], FlashQuestions.ASK_LIST_LENGTH);
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
			if (speed < minSpeed) {
				speed = minSpeed;
			}
			if (orbitingPlanet == null) { 
				Ascend ();
			} else {
				Orbit ();
			}
			if (speed <= 0 && noMoreQuestions) {
				OnDone ();
//				Debug.Log("Coasting time = " + (Time.time - prevTime));
			}
		}
	}

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		Accelerate ();
		++numAnswersGiven;
	}

	public void OnQuestionChanged(Question question) {
		if (question == null) {
			noMoreQuestions = true;
		} else { // test
//			StartCoroutine (AutoAnswerQuestion ());
		}

	}

	public void Accelerate() {
//		Debug.Log ("t = " + Time.time + " oldSpeed = " + speed + " newSpeed = " + (Mathf.Max(0, speed) + accelerationOnCorrect) + " height = " + height);
		if (speed < 0) {
			speed = 0; // anything else is too discouraging.
		}
		speed += accelerationOnCorrect;
	}

	IEnumerator AutoAnswerQuestion() {
		float tgtTime = targetAnswerTime;// - 3.0f;
		yield return new WaitForSeconds (tgtTime - 3.0f); // 3.0f is the celebration time HACK
		while(numAnswersGiven < questions.GetAskListLength() + 1) { // +1 because the first question has been shown.
			OnCorrectAnswer(null, false);
			Debug.Log ("NumAnswersGiven " + numAnswersGiven + " of " + (questions.GetAskListLength()+1));
			yield return new WaitForSeconds (tgtTime);
		}
		noMoreQuestions = true;
	}

	void Ascend ()
	{
		height += Mathf.Clamp (speed, minSpeed, maxSpeed) * Time.deltaTime;
		if (height < 0) {
			height = 0;
			speed = 0;
		}
		recordHeight = UpdateRecord (height, recordHeight, recordPrefsKey);
		if (TargetPlanet.IsTargetPlanetReached (height)) {
			int planetReachedIdx = TargetPlanet.GetTargetPlanetIdx ();
			if (TargetPlanet.IsAlreadyReached (planetReachedIdx)) {
				if (planetReachedIdx <= TargetPlanet.GetNumPlanets ()) {
					StartOrbiting (planetReachedIdx);
				}
			}
			else {
				TargetPlanet.SetLastReachedIdx (planetReachedIdx);
				if (planetReachedIdx == TargetPlanet.GetNumPlanets () - 2) {
					RocketParts.instance.FinalUpgrade ();
				}
				else
					if (planetReachedIdx == TargetPlanet.GetNumPlanets () - 1) {
						TargetPlanet.TargetNextPlanet ();
					}
				StartCoroutine (CelebrateReachingPlanet (planetReachedIdx));
			}
		}
		if (height > apogee) {
			apogee = height;
		}
		heightText.text = height.ToString (heightFormat, formatProvider) + unit;
		recordHeightText.text = recordHeight.ToString (heightFormat, formatProvider) + unit;
		apogeeText.text = apogee.ToString (heightFormat, formatProvider) + unit;
	}

	void Orbit ()
	{
		orbitalDistance += Mathf.Clamp (speed, minSpeed, maxSpeed) * Time.deltaTime;
		recordOrbitalDistance = UpdateRecord (orbitalDistance, recordOrbitalDistance, recordOrbitalDistancePrefsKey);
		if (orbitalDistance > orbitalApogee) {
			orbitalApogee = orbitalDistance;
		}
		orbitsText.text = GetNumOrbits (orbitalDistance).ToString (orbitFormat, formatProvider);
		recordOrbitsText.text = GetNumOrbits (recordOrbitalDistance).ToString (orbitFormat, formatProvider);
		apogeeOrbitsText.text = GetNumOrbits (orbitalApogee).ToString (orbitFormat, formatProvider);
	}

	void StartOrbiting(int planetIdx) {
		orbitalDistance = 0;
		planetCircumferance = planetCircumferances [planetIdx] * circumferanceFactor;
		orbitsText.text = "0";
		apogeeOrbitsText.text = "0";
		recordOrbitalDistance = MDPrefs.GetFloat (recordOrbitalDistancePrefsKey, 0);
		recordOrbitsText.text = GetNumOrbits(recordOrbitalDistance).ToString (heightFormat);
		checkForRecord = recordOrbitalDistance > 0;
		heightWidget.SetActive (false);
		orbitsWidget.SetActive (true);
		zoomToPlanet.ZoomToPlanet (planetIdx, false);
		orbitingPlanet = zoomToPlanet.GetPlanet(planetIdx);
		orbitingPlanetIdx = planetIdx;
	}

	float GetNumOrbits(float linearDistance) {
		return linearDistance / planetCircumferance;
	}

	void OnDone() {
		StopRunning ();
		PrepareNewStart ();
	}

	void PrepareNewStart ()
	{
		noMoreQuestions = false;
		questionPicker.Reset ();
		launch.ShowLaunchButton ();
	}

	void StopRunning ()
	{
		isRunning = false;
		questionPicker.Abort ();
	}

	public void OnCountdown() {
		isRunning = true;
		height = 0;
		apogee = 0;
		speed = 0;
		numAnswersGiven = 0;
		noMoreQuestions = false;
		celebrate.curParticleIdx = RocketParts.instance.upgradeLevel;
	}

	IEnumerator CelebrateReachingPlanet(int planetIdx) {
		StopRunning ();
		oldRecord.SetActive (false);
		float zoomTime = zoomToPlanet.ZoomToPlanet (planetIdx, true);
		UnityEngine.Assertions.Assert.IsTrue (zoomTime - planetAchievementTextDelay >= 0);
		ClearAchievementText (planetAchievementTextDelay * 0.9f);
		yield return new WaitForSeconds (planetAchievementTextDelay);
		ActivateAchievementText (planetReachedTexts [planetIdx]);
		yield return new WaitForSeconds (zoomTime - planetAchievementTextDelay);
		PrepareNewStart ();
		yield return null;
	}

	void ClearAchievementText(float transitionTime) {
		if (achievementText.text.Length > 0) {
			iTween.ScaleTo (achievementText.gameObject, iTween.Hash ("scale", Vector3.zero, "time", transitionTime));
		}
	}

	void ActivateAchievementText (string text)
	{
		achievementText.text = text;
		achievementText.CrossFadeAlpha (1.0f, 0, false);
		achievementText.CrossFadeAlpha (0, achievementTextTransitionTime, false);
		achievementText.transform.localScale = Vector3.zero;
		iTween.ScaleTo (achievementText.gameObject, iTween.Hash ("scale", Vector3.one, "time", achievementTextTransitionTime, "easeType", iTween.EaseType.easeOutQuad));
	}

	void CelebrateBreakingRecord() {
		ActivateAchievementText( recordBrokenMsg );
	}

	float UpdateRecord(float dist, float record, string key) {
		if (dist > record) {
			record = dist;
			MDPrefs.SetFloat (key, record);
			if (checkForRecord) {
				checkForRecord = false;
				CelebrateBreakingRecord ();
			}
		} 
		return record;
	}

	// The rocket reaches speed zero at targetAnswerTime. Calculate acceleration and gravity to guarantee given maxHeight
	void CalcParams(float maxHeight, float targetHeight, int numChancesToAccelerate) {
		// we want the player to reach v = 0 at targetAnswerTime (=:t) seconds after getting accelerated (1)
		// Let v = accelerationOnCorrect
		// Let h = the max height from one acceleration, starting at height = 0 and v = 0
		// Then h = v * v / 2g (2)
		// and time to reach h = v / g (3)
		// But (1) => time to reach h = t, so (3) => t = v / g => g = v / t (4)
		// (2) & (4) => h = v * v / 2(v/t) = v * t / 2 (5)
		// Also, if we accelerate each time from a speed of zero, then total height (=: H, which we want to equal targetHeight) is just
		// H = n * h => h = H / n (6)
		// (5) & (6) => H / n = vt / 2 => 2H / nt = v
		//
		// In addition, we want to cap the max movement rate so that even a God-player can't reach the next planet.
		// While the movement rate is capped, we keep track of the uncapped rate for deceleration purposes.
		// Since a God-player would answer all questions instantly, the initial (uncapped) velocity v0 = vn (7).
		// Since deceleration is based on uncapped velocity, the time to reach speed 0 (and hence max height) will be T = an/g = ant/a (by (4)) = nt (8)
		// The height is more complex, because we have a segment in which we are travelling at velocity cap m (deceleration tracked internally but doesn't change movement rate)
		//   and one in which our uncapped speed has diminished to equal m (and movement rate == internal speed)
		// That is, H' = h1 + h2 = (m * t1) + (m/2 * t2).
		// t1 = (v0 - m) / g = (vn - m) / g
		// t2 = m / g
		// So H' = (m * (vn - m) / g) + (m^2 / 2g)    			 // m^2 / 2g is the usual height formula (2), as expected
		//       = (2mvn - m^2) / 2g = (2mvn - m^2)t / 2v by (4) => m^2(t) - m (2tvn) + 2vH' = 0 => m = nv +/- sqrt((tnv)^2 - 2tvH) / t  
		// For the root to be real, we need n > H'/H, which is no problem.
		// for m >= a, we need H'/H >= 2 - 1/n.
		// Basically, we need 2H <= H' <= nH.
		// Unfortunately, Neptune is less than twice as far as Uranus and Pluto less than twice as far as Neptune. 
		// Even with realistic god (answers instantly after question is displayed), you still go too far if m is set >= a.
		accelerationOnCorrect = GetAccelerationNewStyle (targetHeight, numChancesToAccelerate);
		gravity = accelerationOnCorrect / targetAnswerTime;
		minSpeed = -accelerationOnCorrect * minSpeedFactor;
		maxSpeed = Mathf.Max(accelerationOnCorrect, numChancesToAccelerate * accelerationOnCorrect - 
			Mathf.Sqrt (targetAnswerTime * targetAnswerTime * numChancesToAccelerate * numChancesToAccelerate * accelerationOnCorrect * accelerationOnCorrect 
				- 2 * targetAnswerTime * accelerationOnCorrect * maxHeight) / targetAnswerTime);
//		UnityEngine.Assertions.Assert.IsTrue (maxSpeed >= accelerationOnCorrect);
	}

	float GetAccelerationNewStyle(float targetHeight, int numChancesToAccelerate)
	{
		return 2 * targetHeight / (numChancesToAccelerate * targetAnswerTime);
	}

	// Fixed gravity, acceleration calculated to reach maxHeight
	// Todo: I guess what we want is to calculate gravity and acceleration based on reaching a velocity of e.g. (initialVelocity + accelerationOnCorrect/2), since slowing significantly in the time it takes to answer a question makes things look more exciting
	float CalcAcceleration(float h, int n) {
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
		minSpeed = 0;
		return accel;
	}
				
//	void TestEquations() {
//		Debug.Log("Acceleration = " + accelerationOnCorrect + " gravity = " + gravity);
//		ShowResultsForT (0); // this simplifies to numQuestions * maxHeight
//		ShowResultsForT (0.1f);
//		ShowResultsForT (3.0f); // this should be the practical minimum, because the celebration lasts this long
//		ShowResultsForT (targetAnswerTime);
//		ShowResultsForT (targetAnswerTime * 5);
////		ShowResultsForT (targetAnswerTime * 10);
//	
//	}
//
//	void ShowResultsForT (float t)
//	{
//		float g = gravity;
//		const int n = FlashQuestions.ASK_LIST_LENGTH;
//		float decelarationByNextQuestion = Mathf.Min(accelerationOnCorrect, g * t);
//		for (int i = 0; i < n; ++i) {
//			// right after you answer the question:
//			if (speed < 0) {
//				speed = 0;
//			}
//			speed += accelerationOnCorrect;
//			// by the time you answer the next question in targetAnswerTime:
//			height += (speed - 0.5f * decelarationByNextQuestion) * t;
//			speed -= decelarationByNextQuestion;
//			if (speed < minSpeed) {
//				speed = minSpeed;
//			}
//			Debug.Log ("speed = " + speed + " height = " + height);
//		}
//		// height and speed at t = (targetAnswerTime after you answer the last question) = FlashQuestions.ASK_LIST_LENGTH * targetAnswerTime
//		float totalAnswerTime = n * t;
//		const int n1 = n + 1;
//		const int gauss	 = n1 * (n1 + 1) / 2;
//		height = decelarationByNextQuestion * totalAnswerTime * 0.5f + t * (accelerationOnCorrect - decelarationByNextQuestion) * (n1 * n1 - gauss);
//		speed = n * accelerationOnCorrect - g * totalAnswerTime; 
//		Debug.Log ("Closed form speed = " + speed + " height = " + height);
//		height += speed * speed / (2.0f * g);
//		float x = accelerationOnCorrect;
//		float altHeight = (n * x * (g * (t - n * t) + n * x)) / (2 * g);
//		float timeToStop = speed / g + totalAnswerTime;
//		Debug.Log ("final height = " + height + " or " + altHeight + " time = " + timeToStop + " of which freefall = " + speed / g);
//		height = 0;
//		speed = 0;
//	}
}
