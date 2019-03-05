using I2.Loc;

internal class DivisionQuestion : Question
{
    public DivisionQuestion(int a, int b, QuestionPersistentData data) : base(a, b, data)
    {
    }

    public override int GetAnswer()
    {
        return A / B;
    }

    public override string GetLocalizedString()
    {
        var slash = LocalizationManager.GetTermTranslation("divisionSlash");
        return A + slash + B + " = ";
    }
}