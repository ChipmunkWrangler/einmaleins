using System.Collections;
using CrazyChipmunk;
using UnityEngine;
using UnityEngine.UI;

class FlashThrust : MonoBehaviour, IOnQuestionChanged
{
    const float AllottedTime = Question.FastTime; // If a player answers all questions correctly, each in targetAnswerTime, she reaches maxAttainableHeight
    const float MinThrustFactor = 0.1F;
    const int V = 4;
    const string RecordPrefsKey = "recordHeight";
    const string HeightFormat = "N0";
    const string Unit = " km";

    static float q;
    static float maxThrustFactor;

    readonly string[] planetReachedTexts =
    {
        "Mars erreicht!",
        "Jupiter erreicht!",
        "Saturn erreicht!",
        "Uranus erreicht!",
        "Neptun erreicht!",
        "Pluto erreicht!",
        "Ende erreicht!" // should never happen
    };

    [SerializeField] GameObject heightWidget = null;
    [SerializeField] Text heightText = null;
    [SerializeField] Text recordHeightText = null;
    [SerializeField] KickoffLaunch launch = null;
    [SerializeField] GameObject oldRecord = null;
    [SerializeField] Params paramObj = null;
    [SerializeField] Text achievementText = null;
    [SerializeField] string recordBrokenMsg = "Neuer Rekord!";
    [SerializeField] ShowSolarSystem zoomToPlanet = null;
    [SerializeField] float achievementTextTransitionTime = 3.0F;
    [SerializeField] float planetAchievementTextDelay = 2.0F;
    [SerializeField] EffortTracker effortTracker = null;
    [SerializeField] QuestionPicker questionPicker = null;
    [SerializeField] Goal goal = null;

    float baseThrust;
    float gravity;
    float recordHeight;
    float earnedHeight;
    bool isRunning;
    System.IFormatProvider formatProvider;
    bool noMoreQuestions;
    bool checkForRecord;
    Goal.CurGoal curGoal;

    public float Speed { get; private set; } // km per second
    public float Height { get; private set; } // km

    public static float GetThrustFactor(float timeRequired) => MinThrustFactor + ((maxThrustFactor - MinThrustFactor) / GetThrustFraction(timeRequired));
    public float GetMaxSingleQuestionSpeed() => GetHeightIncrease(AllottedTime) / Celebrate.Duration;

    public void Accelerate(float answerTime = AllottedTime)
    {
        earnedHeight += GetHeightIncrease(answerTime);
        float deltaHeight = earnedHeight - Height;
        UnityEngine.Assertions.Assert.IsTrue(deltaHeight > 0);
        float oldSpeed = Speed;
        Speed = Mathf.Sqrt(2F * deltaHeight * gravity);
        UnityEngine.Assertions.Assert.IsTrue(Speed > oldSpeed);
        Debug.Log("old speed = " + oldSpeed + " speed = " + Speed);
    }

    public void OnCountdown()
    {
        curGoal = goal.CalcCurGoal();
        UnityEngine.Assertions.Assert.IsTrue(curGoal == Goal.CurGoal.FlyToPlanet || curGoal == Goal.CurGoal.Gauntlet || curGoal == Goal.CurGoal.Won, "unexpected goal " + curGoal);
        InitRecord();
        InitPhysics(curGoal == Goal.CurGoal.Gauntlet);
        isRunning = true;
        Height = 0;
        Speed = 0;
        noMoreQuestions = false;
    }

    void IOnQuestionChanged.OnQuestionChanged(Question question)
    {
        if (question == null)
        {
            noMoreQuestions = true;
        }
        else if (isRunning)
        { // test
            question.Ask();
        }
    }

    static float GetTargetHeight() => TargetPlanet.GetPlanetHeight(RocketParts.Instance.UpgradeLevel);
    static float CalcQ(float min, float max) => Mathf.Pow((max - min) / (1F - min), V) - 1F;
    static float CalcBaseThrust(bool isGauntlet) => GetTargetHeight() / (EffortTracker.GetNumAnswersInQuiz(isGauntlet) + 1); // +1 because there is an initial launch thrust
    static float GetThrustFraction(float timeRequired) => Mathf.Pow(1.0F + (q * Mathf.Exp(timeRequired - AllottedTime)), 1.0F / V);
    static float CalcMaxThrustFactor()
    {
        int u = RocketParts.Instance.UpgradeLevel;
        return TargetPlanet.GetPlanetHeight(u + 1) / TargetPlanet.GetPlanetHeight(u);
    }

    bool IsTargetPlanetReached() => curGoal != Goal.CurGoal.Won && Height > TargetPlanet.GetPlanetHeight(TargetPlanet.GetTargetPlanetIdx());
    float GetHeightIncrease(float timeRequired) => baseThrust * GetThrustFactor(timeRequired);

    void OnCorrectAnswer(Question question, bool isNewlyMastered)
    {
        if (question == null || !question.IsLaunchCode)
        {
            Accelerate(question?.GetLastAnswerTime() ?? AllottedTime);
        }
    }

    void Start()
    {
        InitScoreWidget();
        oldRecord.SetActive(checkForRecord);
    }

    void InitScoreWidget()
    {
        formatProvider = MDCulture.GetCulture();
        heightWidget.SetActive(true);
        heightText.text = "0";
        InitRecordHeight();
        recordHeightText.text = recordHeight.ToString(HeightFormat) + Unit;
    }

    void InitRecordHeight()
    {
        recordHeight = Prefs.GetFloat(RecordPrefsKey, 0);
    }

    void InitOldRecordLine(bool enable)
    {
        var recordPos = oldRecord.transform.position;
        recordPos.y += recordHeight * paramObj.HeightScale * oldRecord.transform.parent.localScale.y;
        oldRecord.transform.position = recordPos;
        oldRecord.SetActive(enable);
    }

    void InitRecord()
    {
        InitRecordHeight();
        checkForRecord = recordHeight > 0;
        InitOldRecordLine(checkForRecord);
    }

    void InitPhysics(bool isGauntlet)
    {
        maxThrustFactor = CalcMaxThrustFactor();
        q = CalcQ(MinThrustFactor, maxThrustFactor);
        baseThrust = CalcBaseThrust(isGauntlet);
        gravity = CalcGravity();
    }

    void Update()
    {
        if (isRunning)
        {
            Speed -= gravity * Time.deltaTime;
            if (Speed > 0)
            {
                Ascend();
            }
            else if (noMoreQuestions)
            {
                OnDone();
            }
        }
    }

    void Ascend()
    {
        Height += Speed * Time.deltaTime;
        recordHeight = UpdateRecord(Height, recordHeight, RecordPrefsKey);
        heightText.text = Height.ToString(HeightFormat, formatProvider) + Unit;
        recordHeightText.text = recordHeight.ToString(HeightFormat, formatProvider) + Unit;
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
        questionPicker.AbortQuiz();
        oldRecord.SetActive(false);
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
        noMoreQuestions = false;
        launch.ShowLaunchButton();
    }

    void StopRunning()
    {
        Debug.Log("StopRunning");
        Speed = 0;
        isRunning = false;
        effortTracker.EndQuiz();
    }

    IEnumerator CelebrateReachingPlanet(int planetIdx)
    {
        float zoomTime = zoomToPlanet.ZoomToPlanet(planetIdx);
        UnityEngine.Assertions.Assert.IsTrue(zoomTime - planetAchievementTextDelay >= 0);
        ClearAchievementText(planetAchievementTextDelay * 0.9F);
        yield return new WaitForSeconds(planetAchievementTextDelay);
        ActivateAchievementText(planetReachedTexts[planetIdx]);
        yield return new WaitForSeconds(zoomTime - planetAchievementTextDelay);
        PrepareNewStart();
        yield return null;
    }

    void ClearAchievementText(float transitionTime)
    {
        if (achievementText.text.Length > 0)
        {
            iTween.ScaleTo(achievementText.gameObject, iTween.Hash("scale", Vector3.zero, "time", transitionTime));
        }
    }

    void ActivateAchievementText(string term)
    {
        achievementText.text = I2.Loc.LocalizationManager.GetTermTranslation(term);
        achievementText.CrossFadeAlpha(1.0F, 0, false);
        achievementText.CrossFadeAlpha(0, achievementTextTransitionTime, false);
        achievementText.transform.localScale = Vector3.zero;
        iTween.ScaleTo(achievementText.gameObject, iTween.Hash("scale", Vector3.one, "time", achievementTextTransitionTime, "easeType", iTween.EaseType.easeOutQuad));
    }

    void CelebrateBreakingRecord()
    {
        ActivateAchievementText(recordBrokenMsg);
    }

    float UpdateRecord(float dist, float record, string key)
    {
        if (dist > record)
        {
            record = dist;
            Prefs.SetFloat(key, record);
            if (checkForRecord)
            {
                checkForRecord = false;
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
