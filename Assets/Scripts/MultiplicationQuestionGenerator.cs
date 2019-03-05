internal class MultiplicationQuestionGenerator : QuestionGenerator
{
    public const int NumQuestions = MaxMultiplicand * (MaxMultiplicand + 1) / 2;

    public override int GetNumQuestions()
    {
        return NumQuestions;
    }

    public override Question[] Generate(QuestionsPersistentData data)
    {
        var qs = new MultiplicationQuestion[GetNumQuestions()];
        var idx = 0;
        for (var a = 1; a <= MaxMultiplicand; ++a)
        for (var b = a; b <= MaxMultiplicand; ++b)
        {
            qs[idx] = new MultiplicationQuestion(a, b, data.QuestionData[idx]);
            ++idx;
        }

        return qs;
    }
}