using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class Questions : MonoBehaviour
{
    public static readonly int MaxNum = 10;

    [SerializeField] QuestionsPersistentData data = null;

    public Question[] QuestionArray { get; private set; }

    public static int GetNumQuestions() => MaxNum * (MaxNum + 1) / 2;
    public Question GetGaveUpQuestion() => QuestionArray.FirstOrDefault(question => question.GaveUp());
    public Question GetLaunchCodeQuestion() => QuestionArray.FirstOrDefault(question => question.IsLaunchCode);

    public void ResetForNewQuiz()
    {
        foreach (Question question in QuestionArray)
        {
            question.ResetForNewQuiz();
        }
    }

    public Question GetQuestion(bool isFrustrated, bool allowGaveUpQuestions)
    {
        IEnumerable<Question> allowed = QuestionArray.Where(question => !question.WasAnsweredInThisQuiz && !question.IsMastered() && (allowGaveUpQuestions || !question.GaveUp()));

        if (!allowed.Any())
        {
            allowed = QuestionArray.Where(question => !question.WasAnsweredInThisQuiz && !question.GaveUp());
            if (!allowed.Any())
            {
                allowed = QuestionArray.Where(question => !question.WasAnsweredInThisQuiz);
                if (!allowed.Any())
                {
                    return null; // should never happen
                }
            }
        }
        IEnumerable<Question> candidates = allowed.Where(question => question.WasWrong());
        if (!candidates.Any())
        {
            candidates = allowed.Where(question => !question.IsNew());
            if (!candidates.Any())
            { // then give a new question
                return isFrustrated ? allowed.First() : allowed.ElementAt(Random.Range(0, allowed.Count()));
            }
        }
        var orderedCandidates = candidates.OrderBy(q => q.GetAverageAnswerTime());
        return isFrustrated ? orderedCandidates.First() : orderedCandidates.Last();
    }

    public void Save()
    {
        data.Save();
    }

    public Question[] CreateQuestions()
    {
        var qs = new Question[GetNumQuestions()];
        int idx = 0;
        for (int a = 1; a <= MaxNum; ++a)
        {
            for (int b = a; b <= MaxNum; ++b)
            {
                qs[idx] = new Question(a, b, data.QuestionData[idx]);
                ++idx;
            }
        }
        return qs;
    }

    void OnEnable()
    {
        data.Load();
        QuestionArray = CreateQuestions();
    }
}
