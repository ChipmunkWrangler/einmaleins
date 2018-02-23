﻿[System.Serializable]
public class QuestionsPersistentData
{
    const string PrefsKey = "questions";

    public QuestionPersistentData[] QuestionData { get; set; } = new QuestionPersistentData[Questions.GetNumQuestions()];

    public static string GetQuestionKey(int i) => PrefsKey + ":" + i;
    public static bool WereQuestionsCreated() => MDPrefs.HasKey(PrefsKey + ":ArrayLen");

    public void Save()
    {
        MDPrefs.SetInt(PrefsKey + ":ArrayLen", QuestionData.Length);
        for (int i = 0; i < QuestionData.Length; ++i)
        {
            QuestionData[i].Save(GetQuestionKey(i)); // GetQuestionKey is needed for loading data from XML
        }
    }

    public void Load()
    {
        UnityEngine.Assertions.Assert.AreEqual(MDPrefs.GetInt(PrefsKey + ":ArrayLen", QuestionData.Length), QuestionData.Length);
        bool shouldCreate = !WereQuestionsCreated();
        for (int i = 0; i < QuestionData.Length; ++i)
        {
            QuestionData[i] = new QuestionPersistentData();
            if (shouldCreate)
            {
                QuestionData[i].Create(GetQuestionKey(i), i);
            }
            else
            {
                QuestionData[i].Load(GetQuestionKey(i), i);
            }
        }
    }
}