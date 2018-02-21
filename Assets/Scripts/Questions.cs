using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Questions : MonoBehaviour {
    readonly QuestionsPersistentData Data = new QuestionsPersistentData();

    public static readonly int MaxNum = 10;
    public Question[] QuestionArray { get; private set; }
	public static int GetNumQuestions () => MaxNum * (MaxNum + 1) / 2;
    public Question GetGaveUpQuestion() => QuestionArray.FirstOrDefault(question => question.GaveUp());
    public Question GetLaunchCodeQuestion() => QuestionArray.FirstOrDefault(question => question.IsLaunchCode);

	public void ResetForNewQuiz () {
		foreach (Question question in QuestionArray) {
			question.ResetForNewQuiz();
		}
	}

	public Question GetQuestion (bool isFrustrated, bool allowGaveUpQuestions) {
		IEnumerable<Question> allowed = QuestionArray.Where( question => !question.WasAnsweredInThisQuiz && !question.IsMastered() && (allowGaveUpQuestions || !question.GaveUp()) );

		if (!allowed.Any()) {
			allowed = QuestionArray.Where( question => !question.WasAnsweredInThisQuiz && !question.GaveUp() );
			if (!allowed.Any()) {
				allowed = QuestionArray.Where( question => !question.WasAnsweredInThisQuiz );
				if (!allowed.Any()) {
					return null; // should never happen
				}
			}
		}
		IEnumerable<Question> candidates = allowed.Where( question => question.WasWrong() );
		if (!candidates.Any()) {
			candidates = allowed.Where( question => !question.IsNew() );
			if (!candidates.Any()) { // then give a new question
				return (isFrustrated) ? allowed.First() : allowed.ElementAt( Random.Range( 0, allowed.Count() ) );
			}
		}
		var orderedCandidates = candidates.OrderBy( q => q.GetAverageAnswerTime() );
		return (isFrustrated) ? orderedCandidates.First() : orderedCandidates.Last();
	}

	public void Save () {
		Data.Save();
	}

	void OnEnable () {
		Data.Load();
		QuestionArray = CreateQuestions();
	}

	public Question[] CreateQuestions () {
        var qs = new Question[GetNumQuestions()];
		int idx = 0;
		for (int a = 1; a <= MaxNum; ++a) {
			for (int b = a; b <= MaxNum; ++b) {
				qs[ idx ] = new Question( a, b, Data.QuestionData[ idx ] );
				++idx;
			}
		}
		return qs;
	}
}

[System.Serializable]
public class QuestionsPersistentData {
    public QuestionPersistentData[] QuestionData = new QuestionPersistentData[Questions.GetNumQuestions()];

	const string prefsKey = "questions";

	public static string GetQuestionKey (int i) => prefsKey + ":" + i;
    public static bool WereQuestionsCreated() => MDPrefs.HasKey(prefsKey + ":ArrayLen");

	public void Save () {
		MDPrefs.SetInt( prefsKey + ":ArrayLen", QuestionData.Length );
		for (int i = 0; i < QuestionData.Length; ++i) {
			QuestionData[ i ].Save( GetQuestionKey( i ) ); // GetQuestionKey is needed for loading data from XML
		}
	}

	public void Load () {
		UnityEngine.Assertions.Assert.AreEqual( MDPrefs.GetInt( prefsKey + ":ArrayLen", QuestionData.Length ), QuestionData.Length );
		bool shouldCreate = !WereQuestionsCreated();
		for (int i = 0; i < QuestionData.Length; ++i) {
			QuestionData[ i ] = new QuestionPersistentData();
			if (shouldCreate) {
				QuestionData[ i ].Create( GetQuestionKey( i ), i );
			} else {
				QuestionData[ i ].Load( GetQuestionKey( i ), i );
			}
		}
	}
}