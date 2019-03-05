internal class DivisionQuestionGenerator : QuestionGenerator
{
    public const int NumQuestions = MaxMultiplicand * MaxMultiplicand;

    public override int GetNumQuestions()
    {
        return NumQuestions;
    }

    public override Question[] Generate(QuestionsPersistentData data)
    {
        var qs = new DivisionQuestion[GetNumQuestions()];
        var idx = 0;
        for (var divisor = 1; divisor <= MaxMultiplicand; ++divisor)
        for (var quotient = 1; quotient <= MaxMultiplicand; ++quotient)
        {
            qs[idx] = new DivisionQuestion(divisor * quotient, divisor, data.QuestionData[idx]);
            ++idx;
        }

        return qs;
    }
}