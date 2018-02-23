using UnityEngine;

namespace TimesTablesTest.Helper
{
    class KidModel
    {
        readonly float initialAnswerTimeMax; // increased by difficulty
        readonly float answerTimeImprovementRate;
        int initialChanceOfCorrect; // lowered by difficulty
        int improvementRate;

        public KidModel(int initialChanceOfCorrect, int improvementRate, float initialAnswerTimeMax, float answerTimeImprovementRate)
        {
            this.initialChanceOfCorrect = initialChanceOfCorrect;
            this.improvementRate = improvementRate;
            this.initialAnswerTimeMax = initialAnswerTimeMax;
            this.answerTimeImprovementRate = answerTimeImprovementRate;
        }

        public bool AnswersCorrectly(TestQuestion question)
        {
            int chance = initialChanceOfCorrect + (improvementRate * (question.TimesAnsweredCorrectly + question.TimesAnsweredWrong)) - question.BaseDifficulty;
            return Random.Range(0, 100) < chance;
        }

        public float AnswerTime(TestQuestion question)
        {
            float maxTime = Mathf.Max(TestAlgorithm.TargetTime, initialAnswerTimeMax - (question.TimesAnsweredCorrectly * answerTimeImprovementRate) + question.BaseDifficulty);
            return Random.Range(TestAlgorithm.MinAnswerTime, maxTime);
        }
    }
}