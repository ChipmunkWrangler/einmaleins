﻿using System.Collections.Generic;
using System.Linq;

[System.Serializable]
class QuestionPersistentData
{
    public static readonly int NumAnswerTimesToRecord = 3;
    public static readonly float AnswerTimeInitial = Question.FastTime + 0.01F;

    string prefsKey;

    public int Idx { get; set; }
    public bool WasMastered { get; set; } // even if it is no longer mastered. This is for awarding rocket parts
    public bool WasWrong { get; set; } // if a question is answered wrong, then wasWrong is true until it is next asked
    public bool IsNew { get; set; } = true;
    public bool GaveUp { get; set; }
    public List<float> AnswerTimes { get; set; }

    public void Load(string prefsKey, int idx)
    {
        this.prefsKey = prefsKey;
        Idx = idx;
        AnswerTimes = GetAnswerTimes(prefsKey);
        WasMastered = MDPrefs.GetBool(prefsKey + ":wasMastered");
        WasWrong = MDPrefs.GetBool(prefsKey + ":wasWrong");
        IsNew = MDPrefs.GetBool(prefsKey + ":isNew", defaultValue: true);
        GaveUp = MDPrefs.GetBool(prefsKey + ":gaveUp");
    }

    public void Create(string prefsKey, int idx)
    {
        this.prefsKey = prefsKey;
        Idx = idx;
        AnswerTimes = GetNewAnswerTimes();
    }

    public void Save(string prefsKey = "")
    {
        if (prefsKey == "")
        {
            prefsKey = this.prefsKey;
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
