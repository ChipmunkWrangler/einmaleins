using System;
using CrazyChipmunk;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
[CreateAssetMenu(menuName = "TimesTables/QuestionsPersistentData")]
internal class QuestionsPersistentData : ScriptableObject
{
    private const string PrefsKey = "questions";

    [SerializeField] private Prefs prefs;

    public QuestionPersistentData[] QuestionData { get; private set; }

    public static string GetQuestionKey(int i)
    {
        return PrefsKey + ":" + i;
    }

    public bool WereQuestionsCreated()
    {
        return prefs.HasKey(PrefsKey + ":ArrayLen");
    }

    public void Save()
    {
        prefs.SetInt(PrefsKey + ":ArrayLen", QuestionData.Length);
        for (var i = 0; i < QuestionData.Length; ++i)
            QuestionData[i].Save(GetQuestionKey(i)); // GetQuestionKey is needed for loading data from XML
    }

    public void Load(int numQuestions)
    {
        if (QuestionData == null || QuestionData.Length == 0)
            QuestionData = new QuestionPersistentData[numQuestions];
        else
            Assert.AreEqual(numQuestions, QuestionData.Length);
        Assert.AreEqual(prefs.GetInt(PrefsKey + ":ArrayLen", QuestionData.Length), QuestionData.Length);
        var shouldCreate = !WereQuestionsCreated();
        for (var i = 0; i < QuestionData.Length; ++i)
        {
            QuestionData[i] = CreateInstance<QuestionPersistentData>();
            if (shouldCreate)
                QuestionData[i].Create(prefs, GetQuestionKey(i), i);
            else
                QuestionData[i].Load(prefs, GetQuestionKey(i), i);
        }
    }
}