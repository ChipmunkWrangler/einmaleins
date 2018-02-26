using System.Threading.Tasks;
using UnityEngine;

class AnswerDisplay : TextDisplay, IOnQuestionChanged, IOnWrongAnswer, IOnQuizAborted
{
    const float FadeSeconds = EnterAnswerButtonController.TransitionTime;

    [SerializeField] QuestionPicker answerHandler = null;
    [SerializeField] int maxDigits = 0;
    [SerializeField] BoolEvent answerChanged = new BoolEvent();
    string queuedTxt;
    bool isFading;
    Color oldColor;

    public void OnGiveUp()
    {
        SetText(answerHandler.CurAnswer);
    }

    void IOnQuizAborted.OnQuizAborted()
    {
        SetText("");
    }

    void IOnQuestionChanged.OnQuestionChanged(Question question)
    {
        SetText("");
    }

    void IOnWrongAnswer.OnWrongAnswer(bool wasNew)
    {
        GetTextField().color = oldColor;
        Fade().WrapErrors();
    }

    void OnCorrectAnswer()
    {
        SetText("");
    }

    void OnAddDigit(string nextDigit)
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

    void OnBackspace()
    {
        string answerTxt = GetText();
        if (answerTxt.Length > 0)
        {
            SetText(answerTxt.Substring(0, answerTxt.Length - 1));
            NotifySubscribers();
        }
    }

    void OnSubmitAnswer()
    {
        answerHandler.CurAnswer = GetText();
    }

    void Start()
    {
        oldColor = GetTextField().color;
        SetText("");
    }

    async Task Fade()
    {
        if (isFading)
        {
            return;
        }
        isFading = true;
        queuedTxt = "";
        GetTextField().CrossFadeColor(Color.clear, FadeSeconds, false, true);
        await new WaitForSeconds(FadeSeconds);
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
