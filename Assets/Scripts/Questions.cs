using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Questions : MonoBehaviour {
	public const int maxNum = 10;

	public Question[] questions { get; private set; }

	const string prefsKey = "questions";

	void Awake() {
		CreateQuestions ();
		Load ();
	}

	public static int GetNumQuestions() {
		return maxNum * (maxNum + 1) / 2;
	}
		
	public void ResetForNewQuiz() {
		foreach (Question question in questions) {
			question.ResetForNewQuiz ();
		}
	}

	public float GetQuizTime() {
		return questions.Where (q => q.wasAnsweredInThisQuiz).Sum (q => q.GetLastAnswerTime ());
	}

	public Question GetQuestion(bool isFrustrated) {
		IEnumerable<Question> allowed = questions.Where (question => !question.wasAnsweredInThisQuiz && !question.IsMastered ());

		if (!allowed.Any()) {
			allowed = questions.Where (question => !question.wasAnsweredInThisQuiz);
			if (!allowed.Any ()) {
				return null;
			}
		}
		IEnumerable<Question> candidates = allowed.Where (question => question.wasWrong);
		if (!candidates.Any ()) {
			candidates = allowed.Where (question => !question.isNew);
			if (!candidates.Any ()) { // then give a new question
				return (isFrustrated) ? allowed.First () : allowed.ElementAt(Random.Range(0, allowed.Count()));
			}
		}
		var orderedCandidates = candidates.OrderBy (q => q.GetAverageAnswerTime ());
		return (isFrustrated) ? orderedCandidates.First () : orderedCandidates.Last ();
	}

	public void Save() {
		MDPrefs.SetInt (prefsKey + ":ArrayLen", questions.Length);
		for (int i = 0; i < questions.Length; ++i) {
			questions [i].Save ();
		}
	}

	void Load() {
		UnityEngine.Assertions.Assert.AreEqual (MDPrefs.GetInt (prefsKey + ":ArrayLen", questions.Length), questions.Length);
		if (MDPrefs.HasKey (prefsKey + ":ArrayLen")) {
			for (int i = 0; i < questions.Length; ++i) {
				questions [i].Load (prefsKey + ":" + i, i);
			}
		} else {
			for (int i = 0; i < questions.Length; ++i) {
				questions [i].Create (prefsKey + ":" + i, i);
			}
		}
	}

	void CreateQuestions() {
		questions = new Question[maxNum * (maxNum+1) /2];
		int idx = 0;
		for (int a = 1; a <= maxNum; ++a) {
			for (int b = a; b <= maxNum; ++b) {
				questions [idx] = new Question (a, b);
				++idx;
			}
		}
	}
}
