using UnityEngine;

class EffortTracker : MonoBehaviour, IOnWrongAnswer
{
    [SerializeField] Goal goal = null;
    [SerializeField] Questions questions = null;
    [SerializeField] Fuel fuel = null;

    EffortTrackerPersistantData data = new EffortTrackerPersistantData();
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
        return isGauntlet ? EffortTrackerConfig.GauntletAskListLength : EffortTrackerConfig.NumAnswersPerQuiz;
    }

    public bool IsDoneForToday() => data.IsDoneForToday();

    public void OnCorrectAnswer(Question question, bool isNewlyMastered)
    {
        float answerTime = question.GetLastAnswerTime();
        data.TimeToday += answerTime + Celebrate.Duration;
        data.Frustration += (answerTime <= Question.FastTime) ? EffortTrackerConfig.FrustrationFast : EffortTrackerConfig.FrustrationRight;
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

        if (NumAnswersLeftInQuiz <= EffortTrackerConfig.NumAnswersLeftWhenLaunchCodeAsked && !isFrustrated)
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
        data.Frustration += EffortTrackerConfig.FrustrationWrong;
        if (isQuizStarted)
        {
            --NumAnswersLeftInQuiz;
        }
    }

    public void OnGiveUp()
    {
        data.Frustration += EffortTrackerConfig.FrustrationGiveUp;
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
        UnityEngine.Assertions.Assert.IsTrue(curGoal == Goal.CurGoal.FlyToPlanet || curGoal == Goal.CurGoal.Gauntlet || curGoal == Goal.CurGoal.Won, "unexpected goal " + curGoal);
        allowGivingUp = Goal.IsGivingUpAllowed(curGoal);
        NumAnswersLeftInQuiz = GetNumAnswersInQuiz(curGoal == Goal.CurGoal.Gauntlet);
        questions.ResetForNewQuiz();
        isQuizStarted = true;
    }
}
