using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class Questions : MonoBehaviour
{
    readonly QuestionGenerator questionGenerator = new DivisionQuestionGenerator();
    [SerializeField] private QuestionsPersistentData data;

    public Question[] QuestionArray { get; private set; }

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
        IEnumerable<Question> allowed = QuestionArray.Where(question =>
            !question.WasAnsweredInThisQuiz && !question.IsMastered() && (allowGaveUpQuestions || !question.GaveUp()));

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
            {
                // then give a new question
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


    Question[] CreateQuestions()
    {
        return questionGenerator.Generate(data);
    }

    void OnEnable()
    {
        data.Load(questionGenerator.GetNumQuestions());
        QuestionArray = CreateQuestions();
    }
}
