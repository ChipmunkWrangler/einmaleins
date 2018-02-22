using UnityEngine;

public class QuestionDisplay : TextDisplay, IOnQuestionChanged, IOnQuizAborted
{
    public void OnQuestionChanged(Question question)
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

    public void OnCorrectAnswer()
    {
        SetText("");
    }

    public void OnQuizAborted()
    {
        SetText("");
    }
}
