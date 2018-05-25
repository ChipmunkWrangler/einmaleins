using System;

abstract class QuestionGenerator
{
    public static readonly int MaxMultiplicand = 10;

    public abstract int GetNumQuestions();
    public abstract Question[] Generate(QuestionsPersistentData data);
}

