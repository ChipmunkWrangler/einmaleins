class QuestionDisplay : TextDisplay, IOnQuestionChanged, IOnQuizAborted
{
    void IOnQuestionChanged.OnQuestionChanged(Question question)
    {
        string s = "";
        if (question != null)
        {
            s = question.GetLocalizedString();
        }
        SetText(s);
    }

    void IOnQuizAborted.OnQuizAborted()
    {
        SetText("");
    }

    void OnCorrectAnswer()
    {
        SetText("");
    }
}
