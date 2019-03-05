using System.Threading.Tasks;
using UnityEngine;

internal class AnswerDisplay : TextDisplay, IOnQuestionChanged, IOnWrongAnswer, IOnQuizAborted
{
    private const float FadeSeconds = EnterAnswerButtonController.TransitionTime;
    [SerializeField] private BoolEvent answerChanged = new BoolEvent();

    [SerializeField] private QuestionPicker answerHandler;
    private bool isFading;
    [SerializeField] private int maxDigits;
    private Color oldColor;
    private string queuedTxt;

    void IOnQuestionChanged.OnQuestionChanged(Question question)
    {
        SetText("");
    }

    void IOnQuizAborted.OnQuizAborted()
    {
        SetText("");
    }

    void IOnWrongAnswer.OnWrongAnswer(bool wasNew)
    {
        GetTextField().color = oldColor;
        Fade().WrapErrors();
    }

    public void OnGiveUp()
    {
        SetText(answerHandler.CurAnswer);
    }

    private void OnCorrectAnswer()
    {
        SetText("");
    }

    private void OnAddDigit(string nextDigit)
    {
        var s = isFading ? queuedTxt : GetText();
        s += nextDigit;
        if (s.Length > maxDigits) s = s.Substring(1, s.Length - 1);
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

    private void OnBackspace()
    {
        var answerTxt = GetText();
        if (answerTxt.Length > 0)
        {
            SetText(answerTxt.Substring(0, answerTxt.Length - 1));
            NotifySubscribers();
        }
    }

    private void OnSubmitAnswer()
    {
        answerHandler.CurAnswer = GetText();
    }

    private void Start()
    {
        oldColor = GetTextField().color;
        SetText("");
    }

    private async Task Fade()
    {
        if (isFading) return;
        isFading = true;
        queuedTxt = "";
        GetTextField().CrossFadeColor(Color.clear, FadeSeconds, false, true);
        await new WaitForSeconds(FadeSeconds);
        SetText(queuedTxt);
        queuedTxt = "";
        GetTextField().CrossFadeColor(oldColor, 0, false, true);
        isFading = false;
    }

    private void NotifySubscribers()
    {
        answerChanged.Invoke(GetText().Length == 0);
    }
}