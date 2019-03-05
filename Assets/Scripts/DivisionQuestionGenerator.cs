﻿using System;

class DivisionQuestionGenerator : QuestionGenerator
{
    public const int NumQuestions = MaxMultiplicand * MaxMultiplicand;
    public override int GetNumQuestions() => NumQuestions;

    public override Question[] Generate(QuestionsPersistentData data) 
    {
        var qs = new DivisionQuestion[GetNumQuestions()];
        int idx = 0;
        for (int divisor = 1; divisor <= MaxMultiplicand; ++divisor)
        {
            for (int quotient = 1; quotient <= MaxMultiplicand; ++quotient)
            {
                qs[idx] = new DivisionQuestion(divisor * quotient, divisor, data.QuestionData[idx]);
                ++idx;
            }
        }
        return qs;
    }
}

