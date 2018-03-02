using System.Collections.Generic;
using System.Linq;
using System;
using CrazyChipmunk;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "TimesTables/QuestionPersistentData")]
class QuestionPersistentData : ScriptableObject
{
    public static readonly int NumAnswerTimesToRecord = 3;
    public static readonly float AnswerTimeInitial = Question.FastTime + 0.01F;

    Prefs prefs;
    string prefsKey;

    public int Idx { get; set; }
    public bool WasMastered { get; set; } // even if it is no longer mastered. This is for awarding rocket parts
    public bool WasWrong { get; set; } // if a question is answered wrong, then wasWrong is true until it is next asked
    public bool IsNew { get; set; } = true;
    public bool GaveUp { get; set; }
    public List<float> AnswerTimes { get; set; }

    public void Load(Prefs prefs, string prefsKey, int idx)
    {
        this.prefs = prefs;
        this.prefsKey = prefsKey;
        Idx = idx;
        AnswerTimes = GetAnswerTimes(prefsKey);
        WasMastered = prefs.GetBool(prefsKey + ":wasMastered");
        WasWrong = prefs.GetBool(prefsKey + ":wasWrong");
        IsNew = prefs.GetBool(prefsKey + ":isNew", defaultValue: true);
        GaveUp = prefs.GetBool(prefsKey + ":gaveUp");
    }

    public void Create(Prefs prefs, string prefsKey, int idx)
    {
        this.prefs = prefs;
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
        prefs.SetBool(prefsKey + ":wasMastered", WasMastered);
        prefs.SetBool(prefsKey + ":wasWrong", WasWrong);
        prefs.SetBool(prefsKey + ":isNew", IsNew);
        prefs.SetBool(prefsKey + ":gaveUp", GaveUp);
    }

    List<float> GetAnswerTimes(string prefsKey) => prefs.GetFloatArray(prefsKey + ":times").ToList();

    void SetAnswerTimes(string prefsKey, List<float> answerTimes)
    {
        prefs.SetFloatArray(prefsKey + ":times", answerTimes.ToArray());
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
