﻿using System.Collections.Generic;
using System.Linq;

internal abstract class Question
{
    public const float FastTime = 4.0F;

    private const float AnswerTimeMax = 60.0F;
    private const float WrongAnswerTimePenalty = 1.5F;
    private readonly QuestionPersistentData data;

    public Question(int a, int b, QuestionPersistentData data)
    {
        A = a;
        B = b;
        this.data = data;
    }

    public bool IsLaunchCode { get; set; }
    public int A { get; }
    public int B { get; }
    public bool WasAnsweredInThisQuiz { get; private set; }

    public abstract int GetAnswer();
    public abstract string GetLocalizedString(); // the question to be posed to the user

    public bool WasWrong()
    {
        return data.WasWrong;
    }

    public bool IsNew()
    {
        return data.IsNew;
    }

    public bool GaveUp()
    {
        return data.GaveUp;
    }

    public bool IsMastered()
    {
        return !WasWrong() && GetAverageAnswerTime() <= FastTime;
    }

    public float GetLastAnswerTime()
    {
        return data.AnswerTimes[data.AnswerTimes.Count - 1];
    }

    public float GetAverageAnswerTime()
    {
        return data.AnswerTimes.Average();
    }

    public void SetNewFromAnswerTime()
    {
        var isNew = true;
        foreach (var answerTime in data.AnswerTimes)
            if (answerTime != QuestionPersistentData.AnswerTimeInitial)
            {
                isNew = false;
                break;
            }

        data.IsNew = isNew;
    }

    public bool IsAnswerCorrect(string answer)
    {
        int result;
        if (int.TryParse(answer, out result)) return result == GetAnswer();
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
        var isNewlyMastered = false;
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

//        Debug.Log(ToString());
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
        var s = data.Idx + " is '" + GetLocalizedString() + "' : asMastered = " + data.WasMastered + " wasWrong = " +
                data.WasWrong + " isNew = " + data.IsNew + " gaveUp " + data.GaveUp + " averageTime " +
                GetAverageAnswerTime() + " times = ";
        foreach (var time in data.AnswerTimes) s += time + " ";
        return s;
    }

    public void UpdateInitialAnswerTime(float oldAnswerTimeInitial)
    {
        var newAnswerTimes = new List<float>();
        foreach (var time in data.AnswerTimes)
            newAnswerTimes.Add(time == oldAnswerTimeInitial ? QuestionPersistentData.AnswerTimeInitial : time);
        data.AnswerTimes = newAnswerTimes;
    }

    private void RecordAnswerTime(float timeRequired)
    {
        data.AnswerTimes.Add(timeRequired);
        if (data.AnswerTimes.Count > QuestionPersistentData.NumAnswerTimesToRecord)
            data.AnswerTimes.RemoveRange(0, data.AnswerTimes.Count - QuestionPersistentData.NumAnswerTimesToRecord);
    }

    private float GetAdjustedTime(float timeRequired)
    {
        if (IsLaunchCode)
            timeRequired = AnswerTimeMax;
        else if (data.WasWrong) timeRequired += WrongAnswerTimePenalty;
        if (timeRequired > AnswerTimeMax) timeRequired = AnswerTimeMax;
        return timeRequired;
    }
}