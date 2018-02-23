using System.Collections;
using UnityEngine;

class AnswerDisplay : TextDisplay, IOnQuestionChanged, IOnWrongAnswer, IOnQuizAborted, IOnGiveUp
{
    const float FadeTime = EnterAnswerButtonController.TransitionTime;

    [SerializeField] QuestionPicker answerHandler = null;
    [SerializeField] int maxDigits = 0;
    [SerializeField] BoolEvent answerChanged = new BoolEvent();
    string queuedTxt;
    bool isFading;
    Color oldColor;

    public void OnQuizAborted()
    {
        SetText("");
    }

    public void OnQuestionChanged(Question question)
    {
        SetText("");
    }

    public void OnWrongAnswer(bool wasNew)
    {
        GetTextField().color = oldColor;
        StopAllCoroutines();
        StartCoroutine(Fade());
    }

    public void OnCorrectAnswer()
    {
        SetText("");
    }

    public void OnGiveUp(Question question)
    {
        SetText(question.GetAnswer().ToString());
    }

    public void OnAddDigit(string nextDigit)
    {
        string s = isFading ? queuedTxt : GetText();
        s += nextDigit;
        if (s.Length > maxDigits)
        {
            s = s.Substring(1, s.Length - 1);
        }
        if (isFading)
        {
            queuedTxt = s;
        }
        else
        {
            SetText(s);
            NotifySubscribers();
        }
    }

    public void OnBackspace()
    {
        string answerTxt = GetText();
        if (answerTxt.Length > 0)
        {
            SetText(answerTxt.Substring(0, answerTxt.Length - 1));
            NotifySubscribers();
        }
    }

    public void OnSubmitAnswer()
    {
        answerHandler.OnAnswer(GetText());
    }

    void Start()
    {
        oldColor = GetTextField().color;
        SetText("");
    }

    IEnumerator Fade()
    {
        isFading = true;
        queuedTxt = "";
        GetTextField().CrossFadeColor(Color.clear, FadeTime, false, true);
        yield return new WaitForSeconds(FadeTime);
        SetText(queuedTxt);
        queuedTxt = "";
        GetTextField().CrossFadeColor(oldColor, 0, false, true);
        isFading = false;
    }

    void NotifySubscribers()
    {
        answerChanged.Invoke(GetText().Length == 0);
    }
}
