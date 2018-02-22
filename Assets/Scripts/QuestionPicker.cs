using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CorrectAnswerEvent : UnityEvent<Question, bool /*isNewlyMastered*/> { }

public class QuestionPicker : MonoBehaviour
{
    [SerializeField] GameObject[] subscribers = null;
    [SerializeField] EffortTracker effortTracker = null;
    [SerializeField] CorrectAnswerEvent correctAnswer = new CorrectAnswerEvent();

    Question curQuestion;
    SubscriberList<IOnQuestionChanged> onQuestionChangeds;
    SubscriberList<IOnWrongAnswer> onWrongAnswers;
    SubscriberList<IOnQuizAborted> onQuizAborteds;
    SubscriberList<IOnGiveUp> onGiveUps;
    float questionTime;

    void Start()
    {
        SplitSubscribers();
    }

    public void AbortQuiz()
    {
        onQuizAborteds.Notify(subscriber => subscriber.OnQuizAborted());
        StopAllCoroutines();
    }

    public void NextQuestion()
    {
        ShowQuestion((curQuestion != null && curQuestion.IsLaunchCode) ? curQuestion : effortTracker.GetQuestion()); // if NextQuestion is called with curQuestion.isLaunchCode, the player gave up on the launch code, which means we should show it again
                                                                                                                     //			StartCoroutine (AutoAnswer());
    }

    //	IEnumerator AutoAnswer() {
    //		yield return new WaitForSeconds (Question.FAST_TIME);
    //		if (curQuestion != null) {
    //			OnAnswer ((curQuestion.a * curQuestion.b).ToString ());
    //		}
    //	}

    public void OnAnswer(string answer)
    {
        if (curQuestion == null || answer.Length == 0)
        {
            return;
        }
        bool isCorrect = curQuestion.IsAnswerCorrect(answer);
        float answerTime = Time.time - questionTime;
        HandleAnswer(isCorrect, answerTime);
        effortTracker.Save();
    }

    public void OnGiveUp()
    {
        onGiveUps.Notify(subscriber => subscriber.OnGiveUp(curQuestion));
    }

    public void ShowQuestion(Question newQuestion)
    {
        curQuestion = newQuestion;
        onQuestionChangeds.Notify(subscriber => subscriber.OnQuestionChanged(curQuestion));
        questionTime = Time.time;
    }

    // I can't figure out a way to get the editor to display a list of OnQuestionChangeds (since an Interface can't be Serializable)...
    private void SplitSubscribers()
    {
        onQuestionChangeds = new SubscriberList<IOnQuestionChanged>(subscribers);
        onWrongAnswers = new SubscriberList<IOnWrongAnswer>(subscribers);
        onQuizAborteds = new SubscriberList<IOnQuizAborted>(subscribers);
        onGiveUps = new SubscriberList<IOnGiveUp>(subscribers);
    }

    void HandleAnswer(bool isCorrect, float answerTime)
    {
        bool wasNew = curQuestion.IsNew();
        bool isNewlyMastered = curQuestion.Answer(isCorrect, answerTime);
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
}
