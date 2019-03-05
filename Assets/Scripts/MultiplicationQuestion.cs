using I2.Loc;
using UnityEngine;

internal class MultiplicationQuestion : Question
{
    public MultiplicationQuestion(int a, int b, QuestionPersistentData data) : base(a, b, data)
    {
    }

    public override int GetAnswer()
    {
        return A * B;
    }

    public override string GetLocalizedString()
    {
        var x = Random.Range(0, 2);
        var dot = LocalizationManager.GetTermTranslation("multiplicationDot");
        var s = x == 0 ? A + dot + B : B + dot + A;
        s += " = ";
        return s;
    }
}