using System;
using UnityEngine;
using UnityEngine.Events;

internal class QuestionPicker : MonoBehaviour
{
    [SerializeField] private CorrectAnswerEvent correctAnswer = new CorrectAnswerEvent();

    private Question curQuestion;
    [SerializeField] private EffortTracker effortTracker;
    private SubscriberList<IOnQuestionChanged> onQuestionChangeds;
    private SubscriberList<IOnQuizAborted> onQuizAborteds;
    private SubscriberList<IOnWrongAnswer> onWrongAnswers;
    private float questionTime;
    [SerializeField] private GameObject[] subscribers;

    public string CurAnswer
    {
        get => curQuestion?.GetAnswer().ToString() ?? "";

        set
        {
            if (curQuestion == null || value.Length == 0) return;
            var isCorrect = curQuestion.IsAnswerCorrect(value);
            var answerTime = Time.time - questionTime;
            HandleAnswer(isCorrect, answerTime);
            effortTracker.Save();
        }
    }

    public void AbortQuiz()
    {
        onQuizAborteds.Notify(subscriber => subscriber.OnQuizAborted());
        StopAllCoroutines();
    }

    public void NextQuestion()
    {
        ShowQuestion(curQuestion != null && curQuestion.IsLaunchCode
            ? curQuestion
            : effortTracker
                .GetQuestion()); // if NextQuestion is called with curQuestion.isLaunchCode, the player gave up on the launch code, which means we should show it again
        // StartCoroutine (AutoAnswer());
    }

    public void ShowQuestion(Question newQuestion)
    {
        curQuestion = newQuestion;
        onQuestionChangeds.Notify(subscriber => subscriber.OnQuestionChanged(curQuestion));
        questionTime = Time.time;
    }

    private void Start()
    {
        SplitSubscribers();
    }

    // I can't figure out a way to get the editor to display a list of OnQuestionChangeds (since an Interface can't be Serializable)...
    private void SplitSubscribers()
    {
        onQuestionChangeds = new SubscriberList<IOnQuestionChanged>(subscribers);
        onWrongAnswers = new SubscriberList<IOnWrongAnswer>(subscribers);
        onQuizAborteds = new SubscriberList<IOnQuizAborted>(subscribers);
    }

    private void HandleAnswer(bool isCorrect, float answerTime)
    {
        var wasNew = curQuestion.IsNew();
        var isNewlyMastered = curQuestion.Answer(isCorrect, answerTime);
        if (isCorrect)
        {
            correctAnswer.Invoke(curQuestion, isNewlyMastered);
            curQuestion = null;
        }
        else
        {
            onWrongAnswers.Notify(subscriber => subscriber.OnWrongAnswer(wasNew));
        }
    }

/*
    Enumerator AutoAnswer()
    {
        yield return new WaitForSeconds(Question.FAST_TIME);
        if (curQuestion != null)
        {
            OnAnswer((curQuestion.a * curQuestion.b).ToString());
        }
    }
*/
    [Serializable]
    private class CorrectAnswerEvent : UnityEvent<Question, bool /*isNewlyMastered*/>
    {
    }
}