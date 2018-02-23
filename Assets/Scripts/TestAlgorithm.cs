using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class TestAlgorithm : MonoBehaviour
{
    public static readonly float[] PlanetHeights = TargetPlanet.Heights;

    const float TargetTime = 3.0F;
    const int MinQuizzesPerDay = 3;
    const int MinTimePerDay = 5 * 60;
    const int MaxQuestionsPerQuiz = 7; // because that's enough to get previous questions out of short term memory, I guess
    const float MinAnswerTime = 1.0F;
    const int FrustrationWrong = 2; // N.B. Since the question is repeated until it is correct, the net effect will be FRUSTRATION_WRONG * n - FRUSTRATION_RIGHT (or _FAST)
    const int FrustrationRight = -1;
    const int FrustrationFast = -2;
    const int MinFrustration = -2;
    const int MaxFrustration = 3;
    const int PartsPerUpgrade = 11;
    const float MinThrustFactor = 0.1F;
    const int V = 4;
    const int NumQuestions = 55;

    TestQuestion[] testQuestions;

    static bool IsReadyForGauntlet(int targetPlanet) => targetPlanet >= PlanetHeights.Length - 1;
    static float GetThrustFactor(float minThrustFactor, float maxThrustFactor, float q, float timeRequired, float allottedTime, int v) => (minThrustFactor + ((maxThrustFactor - minThrustFactor) / GetThrustFraction(q, timeRequired, allottedTime, v)));
    static float GetThrustFraction(float q, float timeRequired, float allottedTime, int v) => Mathf.Pow(1.0F + (q * Mathf.Exp(timeRequired - allottedTime)), 1.0F / v);

    void Start()
    {
        Test(new KidModel(1000, 5, -999.0F, 10.0F));
        Test(new KidModel(110, 5, -7.0F, 10.0F));
        Test(new KidModel(60, 3, 15.0F, 6.0F));
        Test(new KidModel(30, 1, 30.0F, 3.0F));
    }

    void InitQuestions()
    {
        testQuestions = new TestQuestion[NumQuestions];
        for (int i = 0; i < NumQuestions; ++i)
        {
            testQuestions[i] = new TestQuestion(i);
        }
    }

    void Test(KidModel kid)
    {
        InitQuestions();
        int targetPlanet = 0;
        int upgradeLevel = 0;
        int rocketParts = 0;
        int frustration = 0;
        float recordHeight = 0;
        float maxThrustFactor = CalcMaxThrustFactor();
        float q = CalcQ(MinThrustFactor, maxThrustFactor, V);
        for (int day = 0; !IsReadyForGauntlet(targetPlanet); ++day)
        {
            Debug.Log("Day = " + day);
            TestDay(kid, maxThrustFactor, q, ref targetPlanet, ref upgradeLevel, ref rocketParts, ref frustration, ref recordHeight);
        }
        Debug.Log("Ready for gauntlet. Num mastered = " + testQuestions.Count(question => question.WasMastered) + " total right = " + testQuestions.Sum(question => question.TimesAnsweredCorrectly) + " total wrong = " + testQuestions.Sum(question => question.TimesAnsweredWrong));
        foreach (var question in testQuestions)
        {
            Debug.Log(question);
        }
    }

    void TestDay(KidModel kid, float maxThrustFactor, float q, ref int targetPlanet, ref int upgradeLevel, ref int rocketParts, ref int frustration, ref float recordHeight)
    {
        float timeToday = 0;
        for (int i = 0; (i < MinQuizzesPerDay || timeToday < MinTimePerDay) && !IsReadyForGauntlet(targetPlanet); ++i)
        {
            Debug.Log("Quiz " + i + " upgradeLevel = " + upgradeLevel + " targetPlanet " + targetPlanet + " rocketparts = " + rocketParts + " frustration = " + frustration);
            timeToday += TestQuiz(kid, maxThrustFactor, q, ref targetPlanet, ref upgradeLevel, ref rocketParts, ref frustration, ref recordHeight);
        }
    }

    float TestQuiz(KidModel kid, float maxThrustFactor, float q, ref int targetPlanet, ref int upgradeLevel, ref int rocketParts, ref int frustration, ref float recordHeight)
    {
        float height = 0;
        float time = 0;
        float questionsAnswered = 0;
        bool isNewRecord = false;
        int numNew = 0;
        int numMastered = 0;
        int numWrong = 0;
        bool reachedNewPlanet = false;
        bool gotUpgrade = false;
        float baseThrust = GetTargetHeight(upgradeLevel) / MaxQuestionsPerQuiz;

        ResetQuestionsForNewQuiz();
        for (int i = 0; i < MaxQuestionsPerQuiz - numWrong && !reachedNewPlanet; ++i)
        {
            TestQuestion nextQuestion = GetNextQuestion(frustration);
            if (nextQuestion.IsNew)
            {
                ++numNew;
            }
            nextQuestion.Ask();
            Debug.Log("Frustration = " + frustration + " " + nextQuestion);
            float questionTime = kid.AnswerTime(nextQuestion);
            while (!kid.AnswersCorrectly(nextQuestion))
            {
                frustration += FrustrationWrong;
                nextQuestion.AnswerWrong();
                questionTime += kid.AnswerTime(nextQuestion);
            }
            ++questionsAnswered;
            frustration += (questionTime <= TargetTime) ? FrustrationFast : FrustrationRight;
            frustration = Mathf.Clamp(frustration, MinFrustration, MaxFrustration);
            bool isNewlyMastered = nextQuestion.AnswerRight(questionTime);
            if (nextQuestion.WasWrong)
            {
                ++numWrong;
            }
            Debug.Log("Answered " + nextQuestion);
            time += questionTime;
            height += GetHeightIncrease(baseThrust, MinThrustFactor, maxThrustFactor, q, questionTime, TargetTime, V);
            if (isNewlyMastered)
            {
                ++rocketParts;
                ++numMastered;
            }
            if (height > recordHeight)
            {
                if (targetPlanet < PlanetHeights.Length && recordHeight >= PlanetHeights[targetPlanet])
                {
                    height = PlanetHeights[targetPlanet];
                    ++targetPlanet;
                    reachedNewPlanet = true;
                }
                recordHeight = height;
                isNewRecord = true;
            }
        }
        while (rocketParts >= PartsPerUpgrade)
        {
            rocketParts -= PartsPerUpgrade;
            ++upgradeLevel;
            gotUpgrade = true;
        }
        if (gotUpgrade || isNewRecord || reachedNewPlanet)
        {
            Debug.Log("Answered " + questionsAnswered + " questions (" + numNew + " new, " + numWrong + " wrong, " + numMastered + " mastered) in " + time + " seconds. Reached " + height + (isNewRecord ? " new record" : "") + (reachedNewPlanet ? (" reached planet " + (targetPlanet - 1)) : "") + (gotUpgrade ? (" gotUpgrade " + upgradeLevel) : ""));
        }
        return time;
    }

    TestQuestion GetNextQuestion(int frustration)
    {
        var allowed = testQuestions.Where(IsAllowed);

        if (!allowed.Any())
        {
            allowed = testQuestions.Where(question => !question.WasAsked);
            if (!allowed.Any())
            {
                return null;
            }
        }
        var candidates = allowed.Where(question => question.WasWrong);
        if (!candidates.Any())
        {
            candidates = allowed.Where(question => !question.IsNew);
            if (!candidates.Any())
            {
                return (frustration > 0) ? allowed.First() : allowed.ElementAt(Random.Range(0, allowed.Count()));
            }
        }
        var orderedCandidates = candidates.OrderBy(q => q.GetAverageAnswerTime());
        return (frustration > 0) ? orderedCandidates.First() : orderedCandidates.Last();
    }

    void ResetQuestionsForNewQuiz()
    {
        foreach (TestQuestion question in testQuestions)
        {
            question.ResetForNewQuiz();
        }
    }

    bool IsAllowed(TestQuestion question) => !question.WasAsked && !question.IsMastered();
    float GetTargetHeight(int upgradeLevel) => (upgradeLevel < PlanetHeights.Length) ? PlanetHeights[upgradeLevel] : PlanetHeights[PlanetHeights.Length - 1] * 2.0F;
    float GetHeightIncrease(float baseThrust, float minThrustFactor, float maxThrustFactor, float q, float timeRequired, float allottedTime, int v) => baseThrust * GetThrustFactor(minThrustFactor, maxThrustFactor, q, timeRequired, allottedTime, v);
    float CalcMaxThrustFactor()
    {
        float minHeightRatio = float.MaxValue;
        for (int i = 0; i < PlanetHeights.Length - 1; ++i)
        {
            float heightRatio = PlanetHeights[i + 1] / PlanetHeights[i];
            if (heightRatio < minHeightRatio)
            {
                minHeightRatio = heightRatio;
            }
        }
        return minHeightRatio;
    }

    float CalcQ(float minThrustFactor, float maxThrustFactor, int v)
    {
        float min = minThrustFactor;
        float max = maxThrustFactor;
        return Mathf.Pow((max - min) / (1.0F - min), v) - 1.0F;
    }

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
            float maxTime = Mathf.Max(TargetTime, initialAnswerTimeMax - (question.TimesAnsweredCorrectly * answerTimeImprovementRate) + question.BaseDifficulty);
            return Random.Range(MinAnswerTime, maxTime);
        }
    }

    class TestQuestion
    {
        const int NumAnswerTimesToRecord = 3;
        const float AnswerTimeMax = 60.0F;
        const float InitialAnswerTime = TargetTime + 0.01F;

        List<float> answerTimes;
        int timesAnsweredFast;

        public TestQuestion(int baseDifficulty)
        {
            BaseDifficulty = baseDifficulty;
            TimesAnsweredCorrectly = 0;
            TimesAnsweredWrong = 0;
            WasWrong = false;
            WasAsked = false;
            IsNew = true;
            InitAnswerTimes();
        }

        public int BaseDifficulty { get; private set; }
        public int TimesAnsweredCorrectly { get; private set; }
        public int TimesAnsweredWrong { get; private set; }
        public bool WasWrong { get; private set; }
        public bool WasAsked { get; private set; }
        public bool IsNew { get; private set; }
        public bool WasMastered { get; private set; }
        public bool IsMastered() => GetAverageAnswerTime() < TargetTime;
        public float GetAverageAnswerTime() => answerTimes.Average();

        public void ResetForNewQuiz()
        {
            WasAsked = false;
        }

        public void Ask()
        {
            WasWrong = false;
            WasAsked = true;
            IsNew = false;
        }

        public bool AnswerRight(float time)
        {
            ++TimesAnsweredCorrectly;
            RecordAnswerTime(time);
            if (time <= TargetTime)
            {
                ++timesAnsweredFast;
            }
            bool newlyMastered = false;
            if (!WasMastered)
            {
                if (IsMastered())
                {
                    newlyMastered = true;
                    WasMastered = true;
                }
            }
            return newlyMastered;
        }

        public void AnswerWrong()
        {
            ++TimesAnsweredWrong;
            WasWrong = true;
        }

        public override string ToString()
        {
            string s = "Q" + BaseDifficulty + " Answered wrong " + TimesAnsweredWrong + " correct " + TimesAnsweredCorrectly + " fast " + timesAnsweredFast + " averageTime = " + GetAverageAnswerTime() + " times ";
            foreach (var time in answerTimes)
            {
                s += time + " ";
            }
            return s;
        }

        void InitAnswerTimes()
        {
            answerTimes = new List<float>(NumAnswerTimesToRecord);
            for (int i = 0; i < NumAnswerTimesToRecord; ++i)
            {
                answerTimes.Add(InitialAnswerTime);
            }
        }

        void RecordAnswerTime(float timeRequired)
        {
            answerTimes.Add(Mathf.Min(timeRequired, AnswerTimeMax));
            if (answerTimes.Count > NumAnswerTimesToRecord)
            {
                answerTimes.RemoveRange(0, answerTimes.Count - NumAnswerTimesToRecord);
            }
        }
    }
}
