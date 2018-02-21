using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Question
{
    public int A { get; private set; }
    public int B { get; private set; }
    public bool WasAnsweredInThisQuiz { get; private set; }
    public bool IsLaunchCode;

    public const float FastTime = 4.0F;
    const float AnswerTimeMax = 60.0F;
    const float WrongAnswerTimePenalty = 1F;
    readonly QuestionPersistentData Data;

    public Question(int a, int b, QuestionPersistentData data)
    {
        A = a;
        B = b;
        Data = data;
    }

    public int GetAnswer() => A * B;

    public bool WasWrong() => Data.WasWrong;

    public bool IsNew() => Data.IsNew;

    public bool GaveUp() => Data.GaveUp;

    public bool IsMastered() => GetAverageAnswerTime() <= FastTime;

    public float GetLastAnswerTime() => Data.AnswerTimes[Data.AnswerTimes.Count - 1];

    public float GetAverageAnswerTime() => Data.AnswerTimes.Average();

    public void SetNewFromAnswerTime()
    {
        bool isNew = true;
        foreach (float answerTime in Data.AnswerTimes)
        {
            if (answerTime != QuestionPersistentData.AnswerTimeInitial)
            {
                isNew = false;
                break;
            }
        }
        Data.IsNew = isNew;
    }

    public bool IsAnswerCorrect(string answer)
    {
        int result;
        if (int.TryParse(answer, out result))
        {
            return result == GetAnswer();
        }
        return false;
    }

    public void Ask()
    {
        Data.WasWrong = false;
        Data.GaveUp = false;
        IsLaunchCode = false;
    }

    public void ResetForNewQuiz()
    {
        WasAnsweredInThisQuiz = false;
    }

    public bool Answer(bool isCorrect, float timeRequired)
    {
        bool isNewlyMastered = false;
        if (isCorrect)
        {
            RecordAnswerTime(GetAdjustedTime(timeRequired));
            Data.IsNew = false;
            WasAnsweredInThisQuiz = true;
            if (!Data.WasMastered && IsMastered())
            {
                isNewlyMastered = true;
                Data.WasMastered = true;
            }
        }
        else
        {
            Data.WasWrong = true;
        }
        Debug.Log(ToString());
        return isNewlyMastered;
    }

    public void GiveUp()
    {
        Data.IsNew = false;
        Data.GaveUp = true;
        WasAnsweredInThisQuiz = true;
        Data.Save();
    }

    public override string ToString()
    {
        string s = Data.Idx + " is " + A + " * " + B + " : asMastered = " + Data.WasMastered + " wasWrong = " + Data.WasWrong + " isNew = " + Data.IsNew + " gaveUp " + Data.GaveUp + " averageTime " + GetAverageAnswerTime() + " times = ";
        foreach (var time in Data.AnswerTimes)
        {
            s += time + " ";
        }
        return s;
    }

    public void UpdateInitialAnswerTime(float oldAnswerTimeInitial)
    {
        var newAnswerTimes = new List<float>();
        foreach (var time in Data.AnswerTimes)
        {
            newAnswerTimes.Add(time == oldAnswerTimeInitial ? QuestionPersistentData.AnswerTimeInitial : time);
        }
        Data.AnswerTimes = newAnswerTimes;
    }

    void RecordAnswerTime(float timeRequired)
    {
        Data.AnswerTimes.Add(timeRequired);
        if (Data.AnswerTimes.Count > QuestionPersistentData.NumAnswerTimesToRecord)
        {
            Data.AnswerTimes.RemoveRange(0, Data.AnswerTimes.Count - QuestionPersistentData.NumAnswerTimesToRecord);
        }
    }

    float GetAdjustedTime(float timeRequired)
    {
        if (IsLaunchCode)
        {
            timeRequired = AnswerTimeMax;
        }
        else if (Data.WasWrong)
        {
            timeRequired += WrongAnswerTimePenalty;
        }
        if (timeRequired > AnswerTimeMax)
        {
            timeRequired = AnswerTimeMax;
        }
        return timeRequired;
    }

}

[System.Serializable]
public class QuestionPersistentData
{
    public int Idx;
    public bool WasMastered;
    // even if it is no longer mastered. This is for awarding rocket parts
    public bool WasWrong;
    // if a question is answered wrong, then wasWrong is true until it is next asked
    public bool IsNew = true;
    public bool GaveUp;
    public List<float> AnswerTimes;

    public static readonly int NumAnswerTimesToRecord = 3;
    public static readonly float AnswerTimeInitial = Question.FastTime + 0.01F;

    string PrefsKey;

    public void Load(string prefsKey, int idx)
    {
        PrefsKey = prefsKey;
        Idx = idx;
        AnswerTimes = GetAnswerTimes(PrefsKey);
        WasMastered = MDPrefs.GetBool(PrefsKey + ":wasMastered");
        WasWrong = MDPrefs.GetBool(PrefsKey + ":wasWrong");
        IsNew = MDPrefs.GetBool(PrefsKey + ":isNew", defaultValue: true);
        GaveUp = MDPrefs.GetBool(PrefsKey + ":gaveUp");
    }

    public void Create(string prefsKey, int idx)
    {
        PrefsKey = prefsKey;
        Idx = idx;
        AnswerTimes = GetNewAnswerTimes();
    }

    public void Save(string prefsKey = "")
    {
        if (prefsKey == "")
        {
            prefsKey = PrefsKey;
        }
        UnityEngine.Assertions.Assert.AreNotEqual(prefsKey.Length, 0);
        SetAnswerTimes(prefsKey, AnswerTimes);
        MDPrefs.SetBool(prefsKey + ":wasMastered", WasMastered);
        MDPrefs.SetBool(prefsKey + ":wasWrong", WasWrong);
        MDPrefs.SetBool(prefsKey + ":isNew", IsNew);
        MDPrefs.SetBool(prefsKey + ":gaveUp", GaveUp);
    }

    static List<float> GetAnswerTimes(string prefsKey) => MDPrefs.GetFloatArray(prefsKey + ":times").ToList();

    static void SetAnswerTimes(string prefsKey, List<float> answerTimes)
    {
        MDPrefs.SetFloatArray(prefsKey + ":times", answerTimes.ToArray());
    }

    static List<float> GetNewAnswerTimes()
    {
        var answerTimes = new List<float>();
        for (int i = 0; i < NumAnswerTimesToRecord; ++i)
        {
            answerTimes.Add(AnswerTimeInitial);
        }
        return answerTimes;
    }
}
