using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashThrust : MonoBehaviour, IOnQuestionChanged
{
    [SerializeField] GameObject HeightWidget = null;
    [SerializeField] Text HeightText = null;
    [SerializeField] Text RecordHeightText = null;
    [SerializeField] KickoffLaunch Launch = null;
    [SerializeField] GameObject OldRecord = null;
    [SerializeField] Params ParamObj = null;
    [SerializeField] Text AchievementText = null;
    [SerializeField] string RecordBrokenMsg = "Neuer Rekord!";
    [SerializeField] ShowSolarSystem ZoomToPlanet = null;
    [SerializeField] float AchievementTextTransitionTime = 3.0F;
    [SerializeField] float PlanetAchievementTextDelay = 2.0F;
    [SerializeField] EffortTracker EffortTracker = null;
    [SerializeField] QuestionPicker QPicker = null;
    [SerializeField] Goal GoalStatus = null;
    public float Speed { get; private set; } // km per second
    public float Height { get; private set; } // km

    readonly string[] PlanetReachedTexts = {
        "Mars erreicht!",
        "Jupiter erreicht!",
        "Saturn erreicht!",
        "Uranus erreicht!",
        "Neptun erreicht!",
        "Pluto erreicht!",
        "Ende erreicht!" // should never happen
	};

    const float AllottedTime = Question.FastTime; // If a player answers all questions correctly, each in targetAnswerTime, she reaches maxAttainableHeight
    const float MinThrustFactor = 0.1F;
    const int V = 4;
    static float Q;
    static float MaxThrustFactor;
    float BaseThrust;
    float Gravity;
    float RecordHeight;
    float EarnedHeight;
    bool IsRunning;
    System.IFormatProvider FormatProvider;
    bool NoMoreQuestions;
    bool CheckForRecord;
    Goal.CurGoal CurGoal;

    const string RecordPrefsKey = "recordHeight";
    const string HeightFormat = "N0";
    const string Unit = " km";

    public static float GetThrustFactor(float timeRequired) => MinThrustFactor + (MaxThrustFactor - MinThrustFactor) / Mathf.Pow(1.0F + Q * Mathf.Exp(timeRequired - AllottedTime), 1.0F / V);

    public float GetMaxSingleQuestionSpeed() => GetHeightIncrease(AllottedTime) / Celebrate.Duration;

    public void OnCorrectAnswer(Question question, bool isNewlyMastered)
    {
        if (question == null || !question.IsLaunchCode)
        {
            Accelerate(question?.GetLastAnswerTime() ?? AllottedTime);
        }
    }

    public void OnQuestionChanged(Question question)
    {
        if (question == null)
        {
            NoMoreQuestions = true;
        }
        else if (IsRunning)
        { // test
            question.Ask();
        }
    }

    public void Accelerate(float answerTime = AllottedTime)
    {
        EarnedHeight += GetHeightIncrease(answerTime);
        float deltaHeight = EarnedHeight - Height;
        UnityEngine.Assertions.Assert.IsTrue(deltaHeight > 0);
        float oldSpeed = Speed;
        Speed = Mathf.Sqrt(2F * deltaHeight * Gravity);
        UnityEngine.Assertions.Assert.IsTrue(Speed > oldSpeed);
        Debug.Log("old speed = " + oldSpeed + " speed = " + Speed);
    }

    static float GetTargetHeight() => TargetPlanet.GetPlanetHeight(RocketParts.Instance.UpgradeLevel);
    static float CalcQ(float m, float M) => Mathf.Pow((M - m) / (1F - m), V) - 1F;
    static float CalcBaseThrust(bool isGauntlet) => GetTargetHeight() / (EffortTracker.GetNumAnswersInQuiz(isGauntlet) + 1); // +1 because there is an initial launch thrust
    static float CalcMaxThrustFactor()
    {
        //      float minHeightRatio = float.MaxValue;
        //      for (int i = 0; i < TargetPlanet.GetNumPlanets() - 1; ++i) {
        //          float heightRatio = TargetPlanet.GetPlanetHeight (i + 1) / TargetPlanet.GetPlanetHeight (i);
        //          if (heightRatio < minHeightRatio) {
        //              minHeightRatio = heightRatio;
        //          }
        //      }
        //      return minHeightRatio;
        int u = RocketParts.Instance.UpgradeLevel;
        return TargetPlanet.GetPlanetHeight(u + 1) / TargetPlanet.GetPlanetHeight(u);
    }

    bool IsTargetPlanetReached() => CurGoal != Goal.CurGoal.WON && Height > TargetPlanet.GetPlanetHeight(TargetPlanet.GetTargetPlanetIdx());
    float GetHeightIncrease(float timeRequired) => BaseThrust * GetThrustFactor(timeRequired);

    void Start()
    {
        InitScoreWidget();
        OldRecord.SetActive(CheckForRecord);
    }

    void InitScoreWidget()
    {
        FormatProvider = MDCulture.GetCulture();
        HeightWidget.SetActive(true);
        HeightText.text = "0";
        InitRecordHeight();
        RecordHeightText.text = RecordHeight.ToString(HeightFormat) + Unit;
    }

    void InitRecordHeight()
    {
        RecordHeight = MDPrefs.GetFloat(RecordPrefsKey, 0);
    }

    void InitOldRecordLine(bool enable)
    {
        var recordPos = OldRecord.transform.position;
        recordPos.y += RecordHeight * ParamObj.HeightScale * OldRecord.transform.parent.localScale.y;
        OldRecord.transform.position = recordPos;
        OldRecord.SetActive(enable);
    }

    void InitRecord()
    {
        InitRecordHeight();
        CheckForRecord = RecordHeight > 0;
        InitOldRecordLine(CheckForRecord);
    }

    void InitPhysics(bool isGauntlet)
    {
        MaxThrustFactor = CalcMaxThrustFactor();
        Q = CalcQ(MinThrustFactor, MaxThrustFactor);
        BaseThrust = CalcBaseThrust(isGauntlet);
        Gravity = CalcGravity();
    }

    void Update()
    {
        if (IsRunning)
        {
            Speed -= Gravity * Time.deltaTime;
            if (Speed > 0)
            {
                Ascend();
            }
            else if (NoMoreQuestions)
            {
                OnDone();
            }
        }
    }

    void Ascend()
    {
        Height += Speed * Time.deltaTime;
        RecordHeight = UpdateRecord(Height, RecordHeight, RecordPrefsKey);
        HeightText.text = Height.ToString(HeightFormat, FormatProvider) + Unit;
        RecordHeightText.text = RecordHeight.ToString(HeightFormat, FormatProvider) + Unit;
        if (IsTargetPlanetReached())
        {
            ReachPlanet();
        }
    }

    void ReachPlanet()
    {
        int planetReachedIdx = TargetPlanet.GetTargetPlanetIdx();
        TargetPlanet.SetLastReachedIdx(planetReachedIdx);
        TargetPlanet.TargetNextPlanet();
        if (planetReachedIdx == TargetPlanet.GetMaxPlanetIdx())
        {
            RocketParts.Instance.UnlockFinalUpgrade();
        }
        StopRunning();
        QPicker.AbortQuiz();
        OldRecord.SetActive(false);
        StartCoroutine(CelebrateReachingPlanet(planetReachedIdx));
    }

    void OnDone()
    {
        Debug.Log("OnDone");
        StopRunning();
        PrepareNewStart();
    }

    void PrepareNewStart()
    {
        NoMoreQuestions = false;
        Launch.ShowLaunchButton();
    }

    void StopRunning()
    {
        Debug.Log("StopRunning");
        Speed = 0;
        IsRunning = false;
        EffortTracker.EndQuiz();
    }

    public void OnCountdown()
    {
        CurGoal = GoalStatus.CalcCurGoal();
        UnityEngine.Assertions.Assert.IsTrue(CurGoal == Goal.CurGoal.FLY_TO_PLANET || CurGoal == Goal.CurGoal.GAUNTLET || CurGoal == Goal.CurGoal.WON, "unexpected goal " + CurGoal);
        InitRecord();
        InitPhysics(CurGoal == Goal.CurGoal.GAUNTLET);
        IsRunning = true;
        Height = 0;
        Speed = 0;
        NoMoreQuestions = false;
    }

    IEnumerator CelebrateReachingPlanet(int planetIdx)
    {
        float zoomTime = ZoomToPlanet.ZoomToPlanet(planetIdx);
        UnityEngine.Assertions.Assert.IsTrue(zoomTime - PlanetAchievementTextDelay >= 0);
        ClearAchievementText(PlanetAchievementTextDelay * 0.9F);
        yield return new WaitForSeconds(PlanetAchievementTextDelay);
        ActivateAchievementText(PlanetReachedTexts[planetIdx]);
        yield return new WaitForSeconds(zoomTime - PlanetAchievementTextDelay);
        PrepareNewStart();
        yield return null;
    }

    void ClearAchievementText(float transitionTime)
    {
        if (AchievementText.text.Length > 0)
        {
            iTween.ScaleTo(AchievementText.gameObject, iTween.Hash("scale", Vector3.zero, "time", transitionTime));
        }
    }

    void ActivateAchievementText(string term)
    {
        AchievementText.text = I2.Loc.LocalizationManager.GetTermTranslation(term);
        AchievementText.CrossFadeAlpha(1.0F, 0, false);
        AchievementText.CrossFadeAlpha(0, AchievementTextTransitionTime, false);
        AchievementText.transform.localScale = Vector3.zero;
        iTween.ScaleTo(AchievementText.gameObject, iTween.Hash("scale", Vector3.one, "time", AchievementTextTransitionTime, "easeType", iTween.EaseType.easeOutQuad));
    }

    void CelebrateBreakingRecord()
    {
        ActivateAchievementText(RecordBrokenMsg);
    }

    float UpdateRecord(float dist, float record, string key)
    {
        if (dist > record)
        {
            record = dist;
            MDPrefs.SetFloat(key, record);
            if (CheckForRecord)
            {
                CheckForRecord = false;
                CelebrateBreakingRecord();
            }
        }
        return record;
    }

    float CalcGravity()
    {
        float allottedDeltaHeight = GetHeightIncrease(AllottedTime);
        UnityEngine.Assertions.Assert.IsTrue(allottedDeltaHeight > 0);
        float time = Celebrate.Duration + AllottedTime;
        float avgSpeed = allottedDeltaHeight / time;
        float s = 2F * avgSpeed;
        return s * s / (2F * allottedDeltaHeight);
    }
}
