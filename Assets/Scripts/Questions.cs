using System.Linq;
using CrazyChipmunk;
using UnityEngine;

internal class Questions : MonoBehaviour
{
    [SerializeField] private QuestionsPersistentData data;
    [SerializeField] private Prefs prefs;

    private QuestionGenerator questionGenerator;

    public Question[] QuestionArray { get; private set; }
    public int NumQuestions => questionGenerator.GetNumQuestions();

    public Question GetGaveUpQuestion()
    {
        return QuestionArray.FirstOrDefault(question => question.GaveUp());
    }

    public Question GetLaunchCodeQuestion()
    {
        return QuestionArray.FirstOrDefault(question => question.IsLaunchCode);
    }

    public void ResetForNewQuiz()
    {
        foreach (var question in QuestionArray) question.ResetForNewQuiz();
    }

    public Question GetQuestion(bool isFrustrated, bool allowGaveUpQuestions)
    {
        var allowed = QuestionArray.Where(question =>
            !question.WasAnsweredInThisQuiz && !question.IsMastered() && (allowGaveUpQuestions || !question.GaveUp()));

        if (!allowed.Any())
        {
            allowed = QuestionArray.Where(question => !question.WasAnsweredInThisQuiz && !question.GaveUp());
            if (!allowed.Any())
            {
                allowed = QuestionArray.Where(question => !question.WasAnsweredInThisQuiz);
                if (!allowed.Any()) return null; // should never happen
            }
        }

        var candidates = allowed.Where(question => question.WasWrong());
        if (!candidates.Any())
        {
            candidates = allowed.Where(question => !question.IsNew());
            if (!candidates.Any())
                return isFrustrated ? allowed.First() : allowed.ElementAt(Random.Range(0, allowed.Count()));
        }

        var orderedCandidates = candidates.OrderBy(q => q.GetAverageAnswerTime());
        return isFrustrated ? orderedCandidates.First() : orderedCandidates.Last();
    }

    public void Save()
    {
        data.Save();
    }


    private Question[] CreateQuestions()
    {
        return questionGenerator.Generate(data);
    }

    private void Awake()
    {
        if (questionGenerator == null) questionGenerator = QuestionGenerator.Factory(prefs);
    }

    private void OnEnable()
    {
        data.Load(NumQuestions);
        QuestionArray = CreateQuestions();
    }
}