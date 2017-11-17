using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashThrust : MonoBehaviour, OnCorrectAnswer, OnQuestionChanged {
	[SerializeField] GameObject heightWidget = null;
	[SerializeField] Text heightText = null;
	[SerializeField] Text recordHeightText = null;
	[SerializeField] KickoffLaunch launch = null;
	[SerializeField] GameObject oldRecord = null;
	[SerializeField] Params paramObj = null;
	[SerializeField] Text achievementText = null;
	[SerializeField] string recordBrokenMsg = "Neuer Rekord!";
	[SerializeField] ShowSolarSystem zoomToPlanet = null;
	[SerializeField] float achievementTextTransitionTime = 3.0f;
	[SerializeField] float planetAchievementTextDelay = 2.0f;
	[SerializeField] EffortTracker effortTracker = null;
	[SerializeField] QuestionPicker questionPicker = null;
	[SerializeField] Goal goal = null;
	public float speed { get; private set; } // km per second
	public float height { get; private set; } // km

	readonly string[] planetReachedTexts = {
		"Mars erreicht!",
		"Jupiter erreicht!",
		"Saturn erreicht!",
		"Uranus erreicht!",
		"Neptun erreicht!",
		"Pluto erreicht!",
		"Ende erreicht!" // should never happen
	};

	const float ALLOTTED_TIME = Question.FAST_TIME; // If a player answers all questions correctly, each in targetAnswerTime, she reaches maxAttainableHeight
	const float MIN_THRUST_FACTOR = 0.1f;
	const int V = 4;
	static float Q;
	static float maxThrustFactor;
	float baseThrust;
	float gravity;
	float recordHeight;
	float earnedHeight;
	bool isRunning;
	System.IFormatProvider formatProvider;
	bool noMoreQuestions;
	bool checkForRecord;
	Goal.CurGoal curGoal;

	const string recordPrefsKey = "recordHeight";
	const string heightFormat = "N0";
	const string unit = " km";

	void Start() {
		InitScoreWidget ();
		oldRecord.SetActive (checkForRecord);
	}

	void InitScoreWidget() {
		formatProvider = MDCulture.GetCulture();
		heightWidget.SetActive (true);
		heightText.text = "0";
		InitRecordHeight ();
		recordHeightText.text = recordHeight.ToString (heightFormat) + unit;
	}

	void InitRecordHeight() {
		recordHeight = MDPrefs.GetFloat (recordPrefsKey, 0);
	}

	void InitOldRecordLine(bool enable) {
		var recordPos = oldRecord.transform.position;
		recordPos.y += recordHeight * paramObj.heightScale * oldRecord.transform.parent.localScale.y;
		oldRecord.transform.position = recordPos;
		oldRecord.SetActive (enable);
	}

	void InitRecord() {
		InitRecordHeight ();
		checkForRecord = recordHeight > 0;
		InitOldRecordLine (checkForRecord);
	}

	void InitPhysics(bool isGauntlet) {
		maxThrustFactor = CalcMaxThrustFactor ();
		Q = CalcQ (MIN_THRUST_FACTOR, maxThrustFactor);
		baseThrust = CalcBaseThrust (isGauntlet);
		gravity = CalcGravity ();
	}

	void Update() {
		if (isRunning) {
			speed -= gravity * Time.deltaTime;
			if (speed > 0) {
				Ascend ();
			} else if (noMoreQuestions) {
				OnDone ();
			}
		}
	}

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		Accelerate ((question == null) ? ALLOTTED_TIME : question.GetLastAnswerTime());
	}

	public void OnQuestionChanged(Question question) {
		if (question == null) {
			noMoreQuestions = true;
		} else if (isRunning) { // test
			question.Ask();
		}
	}

	public void Accelerate(float answerTime = ALLOTTED_TIME) {
		earnedHeight += GetHeightIncrease(answerTime);
		float deltaHeight = earnedHeight - height;
		UnityEngine.Assertions.Assert.IsTrue (deltaHeight > 0);
		float oldSpeed = speed;
		speed = Mathf.Sqrt (2f * deltaHeight * gravity);
		UnityEngine.Assertions.Assert.IsTrue (speed > oldSpeed);
		Debug.Log ("old speed = " + oldSpeed + " speed = " + speed);
	}

	public float GetMaxSingleQuestionSpeed() {
		return GetHeightIncrease (ALLOTTED_TIME) / Celebrate.duration;
	}

	void Ascend () {
		height += speed * Time.deltaTime;
		recordHeight = UpdateRecord (height, recordHeight, recordPrefsKey);
		heightText.text = height.ToString (heightFormat, formatProvider) + unit;
		recordHeightText.text = recordHeight.ToString (heightFormat, formatProvider) + unit;
		if (IsTargetPlanetReached()) {
			ReachPlanet ();
		}
	}

	bool IsTargetPlanetReached() {
		return curGoal != Goal.CurGoal.WON && height > TargetPlanet.GetPlanetHeight (TargetPlanet.GetTargetPlanetIdx ());
	}

	void ReachPlanet ()
	{
		int planetReachedIdx = TargetPlanet.GetTargetPlanetIdx ();
		TargetPlanet.SetLastReachedIdx (planetReachedIdx);
		TargetPlanet.TargetNextPlanet ();
		if (planetReachedIdx == TargetPlanet.GetMaxPlanetIdx ()) {
			RocketParts.instance.UnlockFinalUpgrade ();
		}
		StopRunning ();
		questionPicker.AbortQuiz ();
		oldRecord.SetActive (false);
		StartCoroutine (CelebrateReachingPlanet (planetReachedIdx));
	}

	void OnDone() {
		Debug.Log ("OnDone");
		StopRunning ();
		PrepareNewStart ();
	}

	void PrepareNewStart ()
	{
		noMoreQuestions = false;
		launch.ShowLaunchButton ();
	}

	void StopRunning ()
	{
		Debug.Log ("StopRunning");
		speed = 0;
		isRunning = false;
		effortTracker.EndQuiz ();
	}

	public void OnCountdown() {
		curGoal = goal.calcCurGoal ();
		UnityEngine.Assertions.Assert.IsTrue (curGoal == Goal.CurGoal.FLY_TO_PLANET || curGoal == Goal.CurGoal.GAUNTLET || curGoal == Goal.CurGoal.WON, "unexpected goal " + curGoal);
		InitRecord ();
		InitPhysics (curGoal == Goal.CurGoal.GAUNTLET);
		isRunning = true;
		height = 0;
		speed = 0;
		noMoreQuestions = false;
	}

	IEnumerator CelebrateReachingPlanet(int planetIdx) {
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

	void ActivateAchievementText (string term)
	{
		achievementText.text = I2.Loc.LocalizationManager.GetTermTranslation( term );
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
			
	static float GetTargetHeight() { 
		return TargetPlanet.GetPlanetHeight(RocketParts.instance.upgradeLevel);
	}

	float GetHeightIncrease(float timeRequired) {
		return baseThrust * GetThrustFactor(timeRequired);
	}

	// Each correct answer increases rocket height by a generalized logistic function H(t)
	public static float GetThrustFactor(float timeRequired) {
		return MIN_THRUST_FACTOR + (maxThrustFactor - MIN_THRUST_FACTOR) / Mathf.Pow(1.0f + Q * Mathf.Exp(timeRequired-ALLOTTED_TIME), 1.0f/V);
	}

	static float CalcMaxThrustFactor() {
//		float minHeightRatio = float.MaxValue;
		//		for (int i = 0; i < TargetPlanet.GetNumPlanets() - 1; ++i) {
		//			float heightRatio = TargetPlanet.GetPlanetHeight (i + 1) / TargetPlanet.GetPlanetHeight (i);
//			if (heightRatio < minHeightRatio) {
//				minHeightRatio = heightRatio;
//			}
//		}
//		return minHeightRatio;
		int u = RocketParts.instance.upgradeLevel;
		return TargetPlanet.GetPlanetHeight(u + 1) / TargetPlanet.GetPlanetHeight (u);
	}

	static float CalcQ(float m, float M) {
		return Mathf.Pow((M - m) / (1f - m), V) - 1f;
	}

	static float CalcBaseThrust(bool isGauntlet) {
		return GetTargetHeight () / (EffortTracker.GetNumAnswersInQuiz(isGauntlet)+1); // +1 because there is an initial launch thrust
	}

	float CalcGravity() {
		float allottedDeltaHeight = GetHeightIncrease (ALLOTTED_TIME);
		UnityEngine.Assertions.Assert.IsTrue (allottedDeltaHeight > 0);
		float time = Celebrate.duration + ALLOTTED_TIME;
		float avgSpeed = allottedDeltaHeight / time;
		float s = 2f * avgSpeed;
		return s * s / (2f * allottedDeltaHeight);
	}
}
