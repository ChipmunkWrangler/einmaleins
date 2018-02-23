using UnityEngine;

class QuestionDisplay : TextDisplay, IOnQuestionChanged, IOnQuizAborted
{
    void IOnQuestionChanged.OnQuestionChanged(Question question)
    {
        string s = "";
        if (question != null)
        {
            int x = Random.Range(0, 2);
            string dot = I2.Loc.LocalizationManager.GetTermTranslation("multiplicationDot");
            s = (x == 0) ? question.A + dot + question.B : question.B + dot + question.A;
            s += " = ";
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
