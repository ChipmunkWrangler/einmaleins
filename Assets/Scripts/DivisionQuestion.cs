using System;

class DivisionQuestion : Question
{
    public DivisionQuestion(int a, int b, QuestionPersistentData data) : base(a, b, data) { }

    public override int GetAnswer() => A / B;

    public override string GetLocalizedString()
    {
        string slash = I2.Loc.LocalizationManager.GetTermTranslation("divisionSlash");
        return A + slash + B + " = ";
    }
}
