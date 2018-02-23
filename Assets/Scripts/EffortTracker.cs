using UnityEngine;

class EffortTracker : MonoBehaviour, IOnWrongAnswer, IOnGiveUp
{
    const int FrustrationWrong = 2; // N.B. Since the question is repeated until it is correct, the net effect will be FRUSTRATION_WRONG * n - FRUSTRATION_RIGHT (or _FAST)
    const int FrustrationGiveUp = 2;
    const int FrustrationRight = -1;
    const int FrustrationFast = -2;
    const int NumAnswersPerQuiz = 7; // the bigger this is, the more new questions the kid will be confronted with at once
    const int GauntletAskListLength = 55;
    const int NumAnswersLeftWhenLaunchCodeAsked = 3;

    readonly EffortTrackerPersistantData data = new EffortTrackerPersistantData();

    [SerializeField] Goal goal = null;
    [SerializeField] Questions questions = null;
    [SerializeField] Fuel fuel = null;

    int numAnswersLeftInQuiz;
    bool isQuizStarted;
    bool allowGivingUp;

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

    public static int GetNumAnswersInQuiz(bool isGauntlet)
    {
        return isGauntlet ? GauntletAskListLength : NumAnswersPerQuiz;
    }

    public bool IsDoneForToday() => data.IsDoneForToday();

    public void OnCorrectAnswer(Question question, bool isNewlyMastered)
    {
        float answerTime = question.GetLastAnswerTime();
        data.TimeToday += answerTime + Celebrate.Duration;
        data.Frustration += (answerTime <= Question.FastTime) ? FrustrationFast : FrustrationRight;
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
        Debug.Log("frustration = " + data.Frustration + " numAnswersInQuiz " + NumAnswersLeftInQuiz);
        if (NumAnswersLeftInQuiz <= 0)
        {
            return null;
        }
        bool isFrustrated = data.Frustration > 0;

        if (NumAnswersLeftInQuiz <= NumAnswersLeftWhenLaunchCodeAsked && !isFrustrated)
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
        data.Frustration += FrustrationWrong;
        if (isQuizStarted)
        {
            --NumAnswersLeftInQuiz;
        }
    }

    public void OnGiveUp(Question question)
    {
        data.Frustration += FrustrationGiveUp;
    }

    public void EndQuiz()
    {
        isQuizStarted = false;
        RocketParts.Instance.JustUpgraded = false;
        ++data.QuizzesToday;
        Save();
    }

    public void Save()
    {
        data.Save();
        questions.Save();
    }

    void StartQuiz()
    {
        data.Load();
        Goal.CurGoal curGoal = goal.CalcCurGoal();
        UnityEngine.Assertions.Assert.IsTrue(curGoal == Goal.CurGoal.FLY_TO_PLANET || curGoal == Goal.CurGoal.GAUNTLET || curGoal == Goal.CurGoal.WON, "unexpected goal " + curGoal);
        allowGivingUp = Goal.IsGivingUpAllowed(curGoal);
        NumAnswersLeftInQuiz = GetNumAnswersInQuiz(curGoal == Goal.CurGoal.GAUNTLET);
        questions.ResetForNewQuiz();
        isQuizStarted = true;
    }
}
