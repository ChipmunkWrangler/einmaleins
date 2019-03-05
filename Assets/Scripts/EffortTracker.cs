using UnityEngine;
using UnityEngine.Assertions;

internal class EffortTracker : MonoBehaviour, IOnWrongAnswer
{
    private bool allowGivingUp;
    [SerializeField] private EffortTrackerConfig config;
    [SerializeField] private EffortTrackerPersistentData data;
    [SerializeField] private Fuel fuel;
    [SerializeField] private Goal goal;
    private bool isDataLoaded;
    private bool isQuizStarted;

    private int numAnswersLeftInQuiz;
    [SerializeField] private Questions questions;

    public int QuizzesToday
    {
        get => Data.QuizzesToday;
        set => Data.QuizzesToday = value;
    }

    public float TimeToday
    {
        get => Data.TimeToday;
        set => Data.TimeToday = value;
    }

    public int Frustration
    {
        get => Data.Frustration;
        set => Data.Frustration = Mathf.Clamp(value, config.MinFrustration, config.MaxFrustration);
    }

    private int NumAnswersLeftInQuiz
    {
        get => numAnswersLeftInQuiz;
        set
        {
            numAnswersLeftInQuiz = value;
            fuel.UpdateFuelDisplay(numAnswersLeftInQuiz);
        }
    }

    private EffortTrackerPersistentData Data
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

    public void OnWrongAnswer(bool wasNew)
    {
        Data.Frustration += config.FrustrationWrong;
        if (isQuizStarted) --NumAnswersLeftInQuiz;
    }

    public bool IsDoneForToday()
    {
        return Data.QuizzesToday >= config.MinQuizzesPerDay && Data.TimeToday >= config.MinTimePerDay;
    }

    public int GetNumAnswersInQuiz(bool isGauntlet)
    {
        return isGauntlet ? questions.NumQuestions : config.NumAnswersPerQuiz;
    }

    public void OnCorrectAnswer(Question question, bool isNewlyMastered)
    {
        var answerTime = question.GetLastAnswerTime();
        Data.TimeToday += answerTime + Celebrate.Duration;
        Data.Frustration += answerTime <= Question.FastTime ? config.FrustrationFast : config.FrustrationRight;
        if (isQuizStarted) --NumAnswersLeftInQuiz;
    }

    public Question GetQuestion()
    {
        if (!isQuizStarted) StartQuiz();

//        Debug.Log("frustration = " + Data.Frustration + " numAnswersInQuiz " + NumAnswersLeftInQuiz);
        if (NumAnswersLeftInQuiz <= 0) return null;

        var isFrustrated = Data.Frustration > 0;

        if (NumAnswersLeftInQuiz <= config.NumAnswersLeftWhenLaunchCodeAsked && !isFrustrated)
        {
            var q = questions.GetLaunchCodeQuestion();
            if (q != null) return q;
        }

        return questions.GetQuestion(isFrustrated, !allowGivingUp);
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

    private void StartQuiz()
    {
        Data.Load();
        var curGoal = goal.CalcCurGoal();
        Assert.IsTrue(
            curGoal == Goal.CurGoal.FlyToPlanet || curGoal == Goal.CurGoal.Gauntlet || curGoal == Goal.CurGoal.Won,
            "unexpected goal " + curGoal);
        allowGivingUp = Goal.IsGivingUpAllowed(curGoal);
        NumAnswersLeftInQuiz = GetNumAnswersInQuiz(curGoal == Goal.CurGoal.Gauntlet);
        questions.ResetForNewQuiz();
        isQuizStarted = true;
    }
}