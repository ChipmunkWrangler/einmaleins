﻿using System;
using CrazyChipmunk;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "TimesTables/QuestionsPersistentData")]
class QuestionsPersistentData : ScriptableObject
{
    const string PrefsKey = "questions";

    [SerializeField] Prefs prefs = null;

    public QuestionPersistentData[] QuestionData { get; private set; }

    public static string GetQuestionKey(int i) => PrefsKey + ":" + i;
    public bool WereQuestionsCreated() => prefs.HasKey(PrefsKey + ":ArrayLen");

    public void Save()
    {
        prefs.SetInt(PrefsKey + ":ArrayLen", QuestionData.Length);
        for (int i = 0; i < QuestionData.Length; ++i)
        {
            QuestionData[i].Save(GetQuestionKey(i)); // GetQuestionKey is needed for loading data from XML
        }
    }

    public void Load(int numQuestions)
    {
        if (QuestionData == null || QuestionData.Length == 0)
        {
            QuestionData = new QuestionPersistentData[numQuestions];
        }
        else
        {
            UnityEngine.Assertions.Assert.AreEqual(numQuestions, QuestionData.Length);
        }
        UnityEngine.Assertions.Assert.AreEqual(prefs.GetInt(PrefsKey + ":ArrayLen", QuestionData.Length), QuestionData.Length);
        bool shouldCreate = !WereQuestionsCreated();
        for (int i = 0; i < QuestionData.Length; ++i)
        {
            QuestionData[i] = ScriptableObject.CreateInstance<QuestionPersistentData>();
            if (shouldCreate)
            {
                QuestionData[i].Create(prefs, GetQuestionKey(i), i);
            }
            else
            {
                QuestionData[i].Load(prefs, GetQuestionKey(i), i);
            }
        }
    }
}