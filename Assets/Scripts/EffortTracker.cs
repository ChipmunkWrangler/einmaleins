using UnityEngine;

class EffortTracker : MonoBehaviour, IOnWrongAnswer
{
    [SerializeField] Goal goal = null;
    [SerializeField] Questions questions = null;
    [SerializeField] Fuel fuel = null;
    [SerializeField] EffortTrackerConfig config = null;
    [SerializeField] EffortTrackerPersistentData data = null;

    int numAnswersLeftInQuiz;
    bool isQuizStarted;
    bool allowGivingUp;
    bool isDataLoaded;

    public int QuizzesToday { get { return Data.QuizzesToday; } set { Data.QuizzesToday = value; } }
    public float TimeToday { get { return Data.TimeToday; } set { Data.TimeToday = value; } }
    public int Frustration
    {
        get { return Data.Frustration; }
        set { Data.Frustration = Mathf.Clamp(value, config.MinFrustration, config.MaxFrustration); }
    }

    int NumAnswersLeftInQuiz
    {
        get
        {
            return numAnswersLeftInQuiz;
        }
        set
        {
            numAnswersLeftInQuiz = value;
            fuel.UpdateFuelDisplay(numAnswersLeftInQuiz);
        }
    }

    EffortTrackerPersistentData Data
    {
        get
        {
            if (!isDataLoaded)
            {
                data.Load();
                isDataLoaded = true;
            }
            return data;
        }
    }

    public bool IsDoneForToday() => Data.QuizzesToday >= config.MinQuizzesPerDay && Data.TimeToday >= config.MinTimePerDay;
    public int GetNumAnswersInQuiz(bool isGauntlet) => isGauntlet ? config.GauntletAskListLength : config.NumAnswersPerQuiz;

    public void OnCorrectAnswer(Question question, bool isNewlyMastered)
    {
        float answerTime = question.GetLastAnswerTime();
        Data.TimeToday += answerTime + Celebrate.Duration;
        Data.Frustration += (answerTime <= Question.FastTime) ? config.FrustrationFast : config.FrustrationRight;
        if (isQuizStarted)
        {
            --NumAnswersLeftInQuiz;
        }
    }

    public Question GetQuestion()
    {
        if (!isQuizStarted)
        {
            StartQuiz();
        }
        Debug.Log("frustration = " + Data.Frustration + " numAnswersInQuiz " + NumAnswersLeftInQuiz);
        if (NumAnswersLeftInQuiz <= 0)
        {
            return null;
        }
        bool isFrustrated = Data.Frustration > 0;

        if (NumAnswersLeftInQuiz <= config.NumAnswersLeftWhenLaunchCodeAsked && !isFrustrated)
        {
            Question q = questions.GetLaunchCodeQuestion();
            if (q != null)
            {
                return q;
            }
        }
        return questions.GetQuestion(isFrustrated, !allowGivingUp);
    }

    public void OnWrongAnswer(bool wasNew)
    {
        Data.Frustration += config.FrustrationWrong;
        if (isQuizStarted)
        {
            --NumAnswersLeftInQuiz;
        }
    }

    public void OnGiveUp()
    {
        Data.Frustration += config.FrustrationGiveUp;
    }

    public void EndQuiz()
    {
        isQuizStarted = false;
        RocketParts.Instance.JustUpgraded = false;
        ++Data.QuizzesToday;
        Save();
    }

    public void Save()
    {
        Data.Save();
        questions.Save();
    }

    void StartQuiz()
    {
        Data.Load();
        Goal.CurGoal curGoal = goal.CalcCurGoal();
        UnityEngine.Assertions.Assert.IsTrue(curGoal == Goal.CurGoal.FlyToPlanet || curGoal == Goal.CurGoal.Gauntlet || curGoal == Goal.CurGoal.Won, "unexpected goal " + curGoal);
        allowGivingUp = Goal.IsGivingUpAllowed(curGoal);
        NumAnswersLeftInQuiz = GetNumAnswersInQuiz(curGoal == Goal.CurGoal.Gauntlet);
        questions.ResetForNewQuiz();
        isQuizStarted = true;
    }
}
