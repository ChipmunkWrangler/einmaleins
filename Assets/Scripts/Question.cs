using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Question
{
    public const float FastTime = 4.0F;

    const float AnswerTimeMax = 60.0F;
    const float WrongAnswerTimePenalty = 1F;
    readonly QuestionPersistentData data;

    public Question(int a, int b, QuestionPersistentData data)
    {
        A = a;
        B = b;
        this.data = data;
    }

    public bool IsLaunchCode { get; set; }
    public int A { get; private set; }
    public int B { get; private set; }
    public bool WasAnsweredInThisQuiz { get; private set; }

    public int GetAnswer() => A * B;
    public bool WasWrong() => data.WasWrong;
    public bool IsNew() => data.IsNew;
    public bool GaveUp() => data.GaveUp;
    public bool IsMastered() => GetAverageAnswerTime() <= FastTime;
    public float GetLastAnswerTime() => data.AnswerTimes[data.AnswerTimes.Count - 1];
    public float GetAverageAnswerTime() => data.AnswerTimes.Average();

    public void SetNewFromAnswerTime()
    {
        bool isNew = true;
        foreach (float answerTime in data.AnswerTimes)
        {
            if (answerTime != QuestionPersistentData.AnswerTimeInitial)
            {
                isNew = false;
                break;
            }
        }
        data.IsNew = isNew;
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
        data.WasWrong = false;
        data.GaveUp = false;
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
            data.IsNew = false;
            WasAnsweredInThisQuiz = true;
            if (!data.WasMastered && IsMastered())
            {
                isNewlyMastered = true;
                data.WasMastered = true;
            }
        }
        else
        {
            data.WasWrong = true;
        }
        Debug.Log(ToString());
        return isNewlyMastered;
    }

    public void GiveUp()
    {
        data.IsNew = false;
        data.GaveUp = true;
        WasAnsweredInThisQuiz = true;
        data.Save();
    }

    public override string ToString()
    {
        string s = data.Idx + " is " + A + " * " + B + " : asMastered = " + data.WasMastered + " wasWrong = " + data.WasWrong + " isNew = " + data.IsNew + " gaveUp " + data.GaveUp + " averageTime " + GetAverageAnswerTime() + " times = ";
        foreach (var time in data.AnswerTimes)
        {
            s += time + " ";
        }
        return s;
    }

    public void UpdateInitialAnswerTime(float oldAnswerTimeInitial)
    {
        var newAnswerTimes = new List<float>();
        foreach (var time in data.AnswerTimes)
        {
            newAnswerTimes.Add(time == oldAnswerTimeInitial ? QuestionPersistentData.AnswerTimeInitial : time);
        }
        data.AnswerTimes = newAnswerTimes;
    }

    void RecordAnswerTime(float timeRequired)
    {
        data.AnswerTimes.Add(timeRequired);
        if (data.AnswerTimes.Count > QuestionPersistentData.NumAnswerTimesToRecord)
        {
            data.AnswerTimes.RemoveRange(0, data.AnswerTimes.Count - QuestionPersistentData.NumAnswerTimesToRecord);
        }
    }

    float GetAdjustedTime(float timeRequired)
    {
        if (IsLaunchCode)
        {
            timeRequired = AnswerTimeMax;
        }
        else if (data.WasWrong)
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
