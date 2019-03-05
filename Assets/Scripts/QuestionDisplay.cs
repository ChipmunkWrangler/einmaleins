internal class QuestionDisplay : TextDisplay, IOnQuestionChanged, IOnQuizAborted
{
    void IOnQuestionChanged.OnQuestionChanged(Question question)
    {
        var s = "";
        if (question != null) s = question.GetLocalizedString();
        SetText(s);
    }

    void IOnQuizAborted.OnQuizAborted()
    {
        SetText("");
    }

    private void OnCorrectAnswer()
    {
        SetText("");
    }
}