using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Questions : MonoBehaviour {
	public const int maxNum = 10;

	public Question[] questions { get; private set; }

	const string prefsKey = "questions";
		
	public static bool WereQuestionsCreated ()
	{
		return MDPrefs.HasKey (prefsKey + ":ArrayLen");
	}

	public static Question[] CreateQuestions() {
		Question[] questions = new Question[maxNum * (maxNum+1) /2];
		int idx = 0;
		for (int a = 1; a <= maxNum; ++a) {
			for (int b = a; b <= maxNum; ++b) {
				questions [idx] = new Question (a, b);
				++idx;
			}
		}
		return questions;
	}

	public static string GetQuestionKey (int i)
	{
		return prefsKey + ":" + i;
	}
		
	public static int GetNumQuestions() {
		return maxNum * (maxNum + 1) / 2;
	}
		
	public void ResetForNewQuiz() {
		foreach (Question question in questions) {
			question.ResetForNewQuiz ();
		}
	}

	public Question GetQuestion(bool isFrustrated, bool allowGaveUp) {
		IEnumerable<Question> allowed = questions.Where (question => !question.wasAnsweredInThisQuiz && !question.IsMastered () && (allowGaveUp || !question.gaveUp));

		if (!allowed.Any()) {
			allowed = questions.Where (question => !question.wasAnsweredInThisQuiz && (allowGaveUp || !question.gaveUp));
			if (!allowed.Any ()) {
				allowed = questions.Where (question => !question.wasAnsweredInThisQuiz);
				if (!allowed.Any ()) {
					return null; // should never happen
				}
			}
		}
		IEnumerable<Question> candidates = null;
		if (allowGaveUp) { // then prefer gaveUp
			candidates = allowed.Where (question => question.gaveUp);
		}
		if (candidates == null || !candidates.Any ()) {
			candidates = allowed.Where (question => question.wasWrong);
			if (!candidates.Any ()) {
				candidates = allowed.Where (question => !question.isNew);
				if (!candidates.Any ()) { // then give a new question
					return (isFrustrated) ? allowed.First () : allowed.ElementAt (Random.Range (0, allowed.Count ()));
				}
			}
		}
		var orderedCandidates = candidates.OrderBy (q => q.GetAverageAnswerTime ());
		return (isFrustrated) ? orderedCandidates.First () : orderedCandidates.Last ();
	}

	public Question GetGaveUpQuestion() {
		Question gaveUpQuestion = GetQuestion (false, true);
		return gaveUpQuestion.gaveUp ? gaveUpQuestion : null;
	}

	public void Save() {
		MDPrefs.SetInt (prefsKey + ":ArrayLen", questions.Length);
		for (int i = 0; i < questions.Length; ++i) {
			questions [i].Save ();
		}
	}

	void Load() {
		UnityEngine.Assertions.Assert.AreEqual (MDPrefs.GetInt (prefsKey + ":ArrayLen", questions.Length), questions.Length);
		if (WereQuestionsCreated ()) {
			for (int i = 0; i < questions.Length; ++i) {
				questions [i].Load (GetQuestionKey (i), i);
			}
		} else {
			for (int i = 0; i < questions.Length; ++i) {
				questions [i].Create (GetQuestionKey(i), i);
			}
		}
	}

	void OnEnable() {
		questions = CreateQuestions ();
		Load ();
	}
}
