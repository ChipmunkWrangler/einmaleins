using System;
using System.Collections.Generic;
using CrazyChipmunk;
using I2.Loc;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

internal class FlashThrust : MonoBehaviour, IOnQuestionChanged
{
    private const float
        AllottedTime =
            Question.FastTime; // If a player answers all questions correctly, each in targetAnswerTime, she reaches maxAttainableHeight

    private const float MinThrustFactor = 0.1F;
    private const int V = 4;
    private const string RecordPrefsKey = "recordHeight";
    private const string HeightFormat = "N0";
    private const string Unit = " km";

    private static float q;
    private static float maxThrustFactor;

    private readonly string[] planetReachedTexts =
    {
        "Mars erreicht",
        "Jupiter erreicht",
        "Saturn erreicht",
        "Uranus erreicht",
        "Neptun erreicht",
        "Pluto erreicht",
        "Ende erreicht" // should never happen
    };

    [SerializeField] private Text achievementText;
    [SerializeField] private float achievementTextTransitionTime = 3.0F;

    private float baseThrust;
    private bool checkForRecord;
    private Goal.CurGoal curGoal;
    private float earnedHeight;
    [SerializeField] private EffortTracker effortTracker;
    private IFormatProvider formatProvider;
    [SerializeField] private Goal goal;
    private float gravity;
    [SerializeField] private Text heightText;

    [SerializeField] private GameObject heightWidget;
    private bool isRunning;
    [SerializeField] private KickoffLaunch launch;
    private bool noMoreQuestions;
    [SerializeField] private GameObject oldRecord;
    [SerializeField] private Params paramObj;
    [SerializeField] private float planetAchievementTextDelay = 2.0F;
    [SerializeField] private Prefs prefs;
    [SerializeField] private QuestionPicker questionPicker;
    [SerializeField] private string recordBrokenMsg = "Neuer Rekord";
    private float recordHeight;
    [SerializeField] private Text recordHeightText;
    [SerializeField] private TargetPlanet targetPlanet;
    [SerializeField] private ShowSolarSystem zoomToPlanet;

    public float Speed { get; private set; } // km per second
    public float Height { get; private set; } // km

    void IOnQuestionChanged.OnQuestionChanged(Question question)
    {
        if (question == null)
            noMoreQuestions = true;
        else if (isRunning) question.Ask();
    }

    public static float GetThrustFactor(float timeRequired)
    {
        return MinThrustFactor + (maxThrustFactor - MinThrustFactor) / GetThrustFraction(timeRequired);
    }

    public float GetMaxSingleQuestionSpeed()
    {
        return GetHeightIncrease(AllottedTime) / Celebrate.Duration;
    }

    public void Accelerate(float answerTime = AllottedTime)
    {
        earnedHeight += GetHeightIncrease(answerTime);
        var deltaHeight = earnedHeight - Height;
        Assert.IsTrue(deltaHeight > 0);
        var oldSpeed = Speed;
        Speed = Mathf.Sqrt(2F * deltaHeight * gravity);
        Assert.IsTrue(Speed > oldSpeed);
//        Debug.Log("old speed = " + oldSpeed + " speed = " + Speed);
    }

    public void OnCountdown()
    {
        curGoal = goal.CalcCurGoal();
        Assert.IsTrue(
            curGoal == Goal.CurGoal.FlyToPlanet || curGoal == Goal.CurGoal.Gauntlet || curGoal == Goal.CurGoal.Won,
            "unexpected goal " + curGoal);
        InitRecord();
        InitPhysics(curGoal == Goal.CurGoal.Gauntlet);
        isRunning = true;
        Height = 0;
        Speed = 0;
        noMoreQuestions = false;
    }

    private static float GetTargetHeight()
    {
        return TargetPlanet.GetPlanetHeight(RocketParts.Instance.UpgradeLevel);
    }

    private static float CalcQ(float min, float max)
    {
        return Mathf.Pow((max - min) / (1F - min), V) - 1F;
    }

    private static float GetThrustFraction(float timeRequired)
    {
        return Mathf.Pow(1.0F + q * Mathf.Exp(timeRequired - AllottedTime), 1.0F / V);
    }

    private static float CalcMaxThrustFactor()
    {
        var u = RocketParts.Instance.UpgradeLevel;
        return TargetPlanet.GetPlanetHeight(u + 1) / TargetPlanet.GetPlanetHeight(u);
    }

    private float CalcBaseThrust(bool isGauntlet)
    {
        return GetTargetHeight() / (effortTracker.GetNumAnswersInQuiz(isGauntlet) + 1);
    }

    private bool IsTargetPlanetReached()
    {
        return curGoal != Goal.CurGoal.Won && Height > TargetPlanet.GetPlanetHeight(targetPlanet.GetTargetPlanetIdx());
    }

    private float GetHeightIncrease(float timeRequired)
    {
        return baseThrust * GetThrustFactor(timeRequired);
    }

    private void OnCorrectAnswer(Question question, bool isNewlyMastered)
    {
        if (question == null || !question.IsLaunchCode) Accelerate(question?.GetLastAnswerTime() ?? AllottedTime);
    }

    private void Start()
    {
        InitScoreWidget();
        oldRecord.SetActive(checkForRecord);
    }

    private void InitScoreWidget()
    {
        formatProvider = MDCulture.GetCulture();
        heightWidget.SetActive(true);
        heightText.text = "0";
        InitRecordHeight();
        recordHeightText.text = recordHeight.ToString(HeightFormat) + Unit;
    }

    private void InitRecordHeight()
    {
        recordHeight = prefs.GetFloat(RecordPrefsKey, 0);
    }

    private void InitOldRecordLine(bool enable)
    {
        var recordPos = oldRecord.transform.position;
        recordPos.y += recordHeight * paramObj.HeightScale * oldRecord.transform.parent.localScale.y;
        oldRecord.transform.position = recordPos;
        oldRecord.SetActive(enable);
    }

    private void InitRecord()
    {
        InitRecordHeight();
        checkForRecord = recordHeight > 0;
        InitOldRecordLine(checkForRecord);
    }

    private void InitPhysics(bool isGauntlet)
    {
        maxThrustFactor = CalcMaxThrustFactor();
        q = CalcQ(MinThrustFactor, maxThrustFactor);
        baseThrust = CalcBaseThrust(isGauntlet);
        gravity = CalcGravity();
    }

    private void Update()
    {
        if (isRunning)
        {
            Speed -= gravity * Time.deltaTime;
            if (Speed > 0)
                Ascend();
            else if (noMoreQuestions) OnDone();
        }
    }

    private void Ascend()
    {
        Height += Speed * Time.deltaTime;
        recordHeight = UpdateRecord(Height, recordHeight, RecordPrefsKey);
        heightText.text = Height.ToString(HeightFormat, formatProvider) + Unit;
        recordHeightText.text = recordHeight.ToString(HeightFormat, formatProvider) + Unit;
        if (IsTargetPlanetReached()) ReachPlanet();
    }

    private void ReachPlanet()
    {
        var planetReachedIdx = targetPlanet.GetTargetPlanetIdx();
        targetPlanet.SetLastReachedIdx(planetReachedIdx);
        targetPlanet.TargetNextPlanet();
        if (planetReachedIdx == TargetPlanet.GetMaxPlanetIdx()) RocketParts.Instance.UnlockFinalUpgrade();
        StopRunning();
        questionPicker.AbortQuiz();
        oldRecord.SetActive(false);
        StartCoroutine(CelebrateReachingPlanet(planetReachedIdx));
    }

    private void OnDone()
    {
        Debug.Log("OnDone");
        StopRunning();
        PrepareNewStart();
    }

    private void PrepareNewStart()
    {
        noMoreQuestions = false;
        launch.ShowLaunchButton();
    }

    private void StopRunning()
    {
        Debug.Log("StopRunning");
        Speed = 0;
        isRunning = false;
        effortTracker.EndQuiz();
    }

    private IEnumerator<WaitForSeconds> CelebrateReachingPlanet(int planetIdx)
    {
        var zoomTime = zoomToPlanet.ZoomToPlanet(planetIdx);
        Assert.IsTrue(zoomTime - planetAchievementTextDelay >= 0);
        ClearAchievementText(planetAchievementTextDelay * 0.9F);
        yield return new WaitForSeconds(planetAchievementTextDelay);
        ActivateAchievementText(planetReachedTexts[planetIdx]);
        yield return new WaitForSeconds(zoomTime - planetAchievementTextDelay);
        PrepareNewStart();
        yield return null;
    }

    private void ClearAchievementText(float transitionTime)
    {
        if (achievementText.text.Length > 0)
            iTween.ScaleTo(achievementText.gameObject, iTween.Hash("scale", Vector3.zero, "time", transitionTime));
    }

    private void ActivateAchievementText(string term)
    {
        achievementText.text = LocalizationManager.GetTermTranslation(term);
        achievementText.CrossFadeAlpha(1.0F, 0, false);
        achievementText.CrossFadeAlpha(0, achievementTextTransitionTime, false);
        achievementText.transform.localScale = Vector3.zero;
        iTween.ScaleTo(achievementText.gameObject,
            iTween.Hash("scale", Vector3.one, "time", achievementTextTransitionTime, "easeType",
                iTween.EaseType.easeOutQuad));
    }

    private void CelebrateBreakingRecord()
    {
        ActivateAchievementText(recordBrokenMsg);
    }

    private float UpdateRecord(float dist, float record, string key)
    {
        if (dist > record)
        {
            record = dist;
            prefs.SetFloat(key, record);
            if (checkForRecord)
            {
                checkForRecord = false;
                CelebrateBreakingRecord();
            }
        }

        return record;
    }

    private float CalcGravity()
    {
        var allottedDeltaHeight = GetHeightIncrease(AllottedTime);
        Assert.IsTrue(allottedDeltaHeight > 0);
        var time = Celebrate.Duration + AllottedTime;
        var avgSpeed = allottedDeltaHeight / time;
        var s = 2F * avgSpeed;
        return s * s / (2F * allottedDeltaHeight);
    }
}