using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TimesTables.Test
{
    class TestQuestion
    {
        const int NumAnswerTimesToRecord = 3;
        const float AnswerTimeMax = 60.0F;
        const float InitialAnswerTime = TestAlgorithm.TargetTime + 0.01F;

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
        public bool IsMastered() => GetAverageAnswerTime() < TestAlgorithm.TargetTime;
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
            if (time <= TestAlgorithm.TargetTime)
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
