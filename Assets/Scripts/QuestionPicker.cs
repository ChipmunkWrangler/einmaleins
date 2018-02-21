using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CorrectAnswerEvent : UnityEvent<Question, bool /*isNewlyMastered*/> { }

public class QuestionPicker : MonoBehaviour
{
    [SerializeField] GameObject[] Subscribers = null;
    [SerializeField] EffortTracker EffortTracker = null;
    [SerializeField] CorrectAnswerEvent CorrectAnswer = new CorrectAnswerEvent();

    Question CurQuestion;
    SubscriberList<IOnQuestionChanged> OnQuestionChangeds;
    SubscriberList<IOnWrongAnswer> OnWrongAnswers;
    SubscriberList<IOnQuizAborted> OnQuizAborteds;
    SubscriberList<IOnGiveUp> OnGiveUps;
    float QuestionTime;

    void Start()
    {
        SplitSubscribers();
    }

    public void AbortQuiz()
    {
        OnQuizAborteds.Notify(subscriber => subscriber.OnQuizAborted());
        StopAllCoroutines();
    }

    public void NextQuestion()
    {
        ShowQuestion((CurQuestion != null && CurQuestion.IsLaunchCode) ? CurQuestion : EffortTracker.GetQuestion()); // if NextQuestion is called with curQuestion.isLaunchCode, the player gave up on the launch code, which means we should show it again
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
        if (CurQuestion == null || answer.Length == 0)
        {
            return;
        }
        bool isCorrect = CurQuestion.IsAnswerCorrect(answer);
        float answerTime = Time.time - QuestionTime;
        HandleAnswer(isCorrect, answerTime);
        EffortTracker.Save();
    }

    public void OnGiveUp()
    {
        OnGiveUps.Notify(subscriber => subscriber.OnGiveUp(CurQuestion));
    }

    public void ShowQuestion(Question newQuestion)
    {
        CurQuestion = newQuestion;
        OnQuestionChangeds.Notify(subscriber => subscriber.OnQuestionChanged(CurQuestion));
        QuestionTime = Time.time;
    }

    // I can't figure out a way to get the editor to display a list of OnQuestionChangeds (since an Interface can't be Serializable)...
    private void SplitSubscribers()
    {
        OnQuestionChangeds = new SubscriberList<IOnQuestionChanged>(Subscribers);
        OnWrongAnswers = new SubscriberList<IOnWrongAnswer>(Subscribers);
        OnQuizAborteds = new SubscriberList<IOnQuizAborted>(Subscribers);
        OnGiveUps = new SubscriberList<IOnGiveUp>(Subscribers);
    }

    void HandleAnswer(bool isCorrect, float answerTime)
    {
        bool wasNew = CurQuestion.IsNew();
        bool isNewlyMastered = CurQuestion.Answer(isCorrect, answerTime);
        if (isCorrect)
        {
            CorrectAnswer.Invoke(CurQuestion, isNewlyMastered);
            CurQuestion = null;
        }
        else
        {
            OnWrongAnswers.Notify(subscriber => subscriber.OnWrongAnswer(wasNew));
        }
    }
}
