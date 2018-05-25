using UnityEngine;

class MultiplicationQuestion : Question
{
    public MultiplicationQuestion(int a, int b, QuestionPersistentData data) : base(a, b, data) { }

    public override int GetAnswer() => A * B;

    public override string GetLocalizedString()
    {
        int x = Random.Range(0, 2);
        string dot = I2.Loc.LocalizationManager.GetTermTranslation("multiplicationDot");
        string s = (x == 0) ? A + dot + B : B + dot + A;
        s += " = ";
        return s;
    }
}
