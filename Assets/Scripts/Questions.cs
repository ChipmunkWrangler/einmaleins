using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Questions : MonoBehaviour {
	public const int maxNum = 10;

	public Question[] questions { get; private set; }

	QuestionsPersistentData data = new QuestionsPersistentData();

	public static int GetNumQuestions () {
		return maxNum * (maxNum + 1) / 2;
	}

	public void ResetForNewQuiz () {
		foreach (Question question in questions) {
			question.ResetForNewQuiz();
		}
	}

	public Question GetQuestion (bool isFrustrated, bool allowGaveUpQuestions) {
		IEnumerable<Question> allowed = questions.Where( question => !question.wasAnsweredInThisQuiz && !question.IsMastered() && (allowGaveUpQuestions || !question.GaveUp()) );

		if (!allowed.Any()) {
			allowed = questions.Where( question => !question.wasAnsweredInThisQuiz && !question.GaveUp() );
			if (!allowed.Any()) {
				allowed = questions.Where( question => !question.wasAnsweredInThisQuiz );
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

	public Question GetGaveUpQuestion () {
		return questions.FirstOrDefault( question => question.GaveUp() );
	}

	public Question GetLaunchCodeQuestion () {
		return questions.FirstOrDefault( question => question.isLaunchCode );
	}

	public void Save () {
		data.Save();
	}

	void OnEnable () {
		data.Load();
		questions = CreateQuestions();
	}

	public Question[] CreateQuestions () {
		Question[] questions = new Question[GetNumQuestions()];
		int idx = 0;
		for (int a = 1; a <= maxNum; ++a) {
			for (int b = a; b <= maxNum; ++b) {
				questions[ idx ] = new Question( a, b, data.questionData[ idx ] );
				++idx;
			}
		}
		return questions;
	}
}

[System.Serializable]
public class QuestionsPersistentData {
	public QuestionPersistentData[] questionData = new QuestionPersistentData[Questions.GetNumQuestions()];

	const string prefsKey = "questions";

	public static string GetQuestionKey (int i) {
		return prefsKey + ":" + i;
	}

	public void Save () {
		MDPrefs.SetInt( prefsKey + ":ArrayLen", questionData.Length );
		for (int i = 0; i < questionData.Length; ++i) {
			questionData[ i ].Save( GetQuestionKey( i ) ); // GetQuestionKey is needed for loading data from XML
		}
	}

	public void Load () {
		UnityEngine.Assertions.Assert.AreEqual( MDPrefs.GetInt( prefsKey + ":ArrayLen", questionData.Length ), questionData.Length );
		bool shouldCreate = !WereQuestionsCreated();
		for (int i = 0; i < questionData.Length; ++i) {
			questionData[ i ] = new QuestionPersistentData();
			if (shouldCreate) {
				questionData[ i ].Create( GetQuestionKey( i ), i );
			} else {
				questionData[ i ].Load( GetQuestionKey( i ), i );
			}
		}
	}

	public static bool WereQuestionsCreated () {
		return MDPrefs.HasKey( prefsKey + ":ArrayLen" );
	}
}