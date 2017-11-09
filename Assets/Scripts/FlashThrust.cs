using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashThrust : MonoBehaviour, OnCorrectAnswer, OnQuestionChanged {
	[SerializeField] GameObject heightWidget = null;
	[SerializeField] Text heightText = null;
	[SerializeField] Text recordHeightText = null;
	[SerializeField] KickoffLaunch launch = null;
	[SerializeField] Celebrate celebrate = null;
	[SerializeField] GameObject oldRecord = null;
	[SerializeField] Params paramObj = null;
	[SerializeField] Text achievementText = null;
	[SerializeField] string recordBrokenMsg = "Neuer Rekord!";
	[SerializeField] ShowSolarSystem zoomToPlanet = null;
	[SerializeField] float achievementTextTransitionTime = 3.0f;
	[SerializeField] float planetAchievementTextDelay = 2.0f;
	[SerializeField] EffortTracker effortTracker = null;
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
	float Q;
	float maxThrustFactor;
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

	void InitGoalDependent() {
		InitRecordHeight ();
		curGoal = goal.calcCurGoal ();
		UnityEngine.Assertions.Assert.IsTrue (curGoal == Goal.CurGoal.FLY_TO_PLANET || curGoal == Goal.CurGoal.GAUNTLET || curGoal == Goal.CurGoal.WON, "unexpected goal " + curGoal);
		checkForRecord = recordHeight > 0;
		InitOldRecordLine (checkForRecord);
	}

	void InitParams() {
		maxThrustFactor = CalcMaxThrustFactor ();
		Q = CalcQ (MIN_THRUST_FACTOR, maxThrustFactor);
		baseThrust = CalcBaseThrust ();
	}

	void Update() {
		if (isRunning) {
			speed -= gravity * Time.deltaTime;
			if (speed < 0) {
				speed = 0;
			}
			Ascend ();
			if (speed <= 0 && noMoreQuestions) {
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
		} else { // test
			question.Ask();
		}
	}

	public void Accelerate(float answerTime = ALLOTTED_TIME) {
//		answerTime = ALLOTTED_TIME;
		earnedHeight += GetHeightIncrease(answerTime);
		float deltaHeight = earnedHeight - height;
		UnityEngine.Assertions.Assert.IsTrue (deltaHeight > 0);
		float time = celebrate.duration + ALLOTTED_TIME;
		float avgSpeed = deltaHeight / time;
		speed = 2 * avgSpeed;
		gravity = speed * speed / (2f * deltaHeight);
		Debug.Log ("speed = " + speed + " gravity = " + gravity);
	}

	public float GetMaxSingleQuestionSpeed() {
		return GetHeightIncrease (ALLOTTED_TIME) / celebrate.duration;
	}

	void Ascend () {
		height += speed * Time.deltaTime;
		recordHeight = UpdateRecord (height, recordHeight, recordPrefsKey);
		if (curGoal != Goal.CurGoal.WON && height > TargetPlanet.GetPlanetHeight (TargetPlanet.GetTargetPlanetIdx())) {
			ReachPlanet ();
		}
		heightText.text = height.ToString (heightFormat, formatProvider) + unit;
		recordHeightText.text = recordHeight.ToString (heightFormat, formatProvider) + unit;
	}

	void ReachPlanet ()
	{
		int planetReachedIdx = TargetPlanet.GetTargetPlanetIdx ();
		TargetPlanet.SetLastReachedIdx (planetReachedIdx);
		if (curGoal == Goal.CurGoal.GAUNTLET) {
			TargetPlanet.TargetNextPlanet ();
		}
		else if (planetReachedIdx == TargetPlanet.GetNumPlanets () - 2) {
			RocketParts.instance.FinalUpgrade ();
		}
		StartCoroutine (CelebrateReachingPlanet (planetReachedIdx));
	}

	void OnDone() {
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
		isRunning = false;
		effortTracker.EndQuiz ();
	}

	public void OnCountdown() {
		InitGoalDependent ();
		InitParams ();
		isRunning = true;
		height = 0;
		speed = 0;
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
			
	float GetTargetHeight(int upgradeLevel) { 
		return TargetPlanet.GetPlanetHeight(RocketParts.instance.upgradeLevel);
	}

	// Each correct answer increases rocket height by a generalized logistic function H(t)
	float GetHeightIncrease(float timeRequired) {
		return baseThrust * (MIN_THRUST_FACTOR + (maxThrustFactor - MIN_THRUST_FACTOR) / Mathf.Pow(1.0f + Q * Mathf.Exp(timeRequired-ALLOTTED_TIME), 1.0f/V));
	}

	float CalcMaxThrustFactor() {
		float minHeightRatio = float.MaxValue;
		for (int i = 0; i < TargetPlanet.heights.Length - 1; ++i) {
			float heightRatio = TargetPlanet.heights [i + 1] / TargetPlanet.heights [i];
			if (heightRatio < minHeightRatio) {
				minHeightRatio = heightRatio;
			}
		}
		return minHeightRatio;
	}

	float CalcQ(float minThrustFactor, float maxThrustFactor) {
		return Mathf.Pow((maxThrustFactor - minThrustFactor) / (1f - minThrustFactor), V) - 1f;
	}

	float CalcBaseThrust() {
		return GetTargetHeight (RocketParts.instance.upgradeLevel) / (EffortTracker.NUM_ANSWERS_PER_QUIZ+1); // +1 because there is an initial launch thrust
	}
}
