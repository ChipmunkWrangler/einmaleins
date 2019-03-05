using System;
using CrazyChipmunk;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityScript.Parser;

abstract class QuestionGenerator
{
    public const int MaxMultiplicand = 10;

    public static QuestionGenerator Factory(Prefs prefs)
    {
        string typeName = GetQuestionType(prefs);
        switch (typeName)
        {
            case "multiplication":
                return new MultiplicationQuestionGenerator();
            case "division":
                return new DivisionQuestionGenerator();
            default:
                Debug.LogWarning($"Unknown question type {typeName}");
                return new MultiplicationQuestionGenerator();
        }
    }

    public static string GetQuestionType(Prefs prefs)
    {
        return prefs.GetString("questionType", "multiplication");
    }

    public static void SetQuestionType(Prefs prefs, string questionType)
    {
        prefs.SetString("questionType", questionType);
    }

    public abstract int GetNumQuestions();

    public static int GetNumQuestions(Prefs prefs)
    {
        string typeName = GetQuestionType(prefs);
        switch (typeName)
        {
            case "multiplication":
                return MultiplicationQuestionGenerator.NumQuestions;
            case "division":
                return DivisionQuestionGenerator.NumQuestions;
            default:
                Debug.LogError($"Unknown question type {typeName}");
                return MultiplicationQuestionGenerator.NumQuestions;
        }
    }

    public abstract Question[] Generate(QuestionsPersistentData data);
}