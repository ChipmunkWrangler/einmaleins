using System;

class MultiplicationQuestionGenerator : QuestionGenerator
{
    public override int GetNumQuestions() => MaxMultiplicand * (MaxMultiplicand + 1) / 2;

    public override Question[] Generate(QuestionsPersistentData data)
    {
        var qs = new MultiplicationQuestion[GetNumQuestions()];
        int idx = 0;
        for (int a = 1; a <= MaxMultiplicand; ++a)
        {
            for (int b = a; b <= MaxMultiplicand; ++b)
            {
                qs[idx] = new MultiplicationQuestion(a, b, data.QuestionData[idx]);
                ++idx;
            }
        }
        return qs;
    }
}

