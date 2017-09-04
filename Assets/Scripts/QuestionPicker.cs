using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPicker : MonoBehaviour {
	[SerializeField] int minPlayMinutes = 5;
	[SerializeField] int maxRecommendedPlayMinutes = 10;
	[SerializeField] int minNumWrong = 3;
	[SerializeField] GameObject[] subscribers;

	Questions questions;
	Question curQuestion;
	List<OnQuestionChanged> onQuestionChangedSubscribers;
	List<OnCorrectAnswer> onCorrectAnswerSubscribers;
	List<OnWrongAnswer> onWrongAnswerSubscribers;
	int numWrong;

	void Start () {
		questions = new Questions();
		SplitSubscribers ();
		NextQuestion ();
	}

	public void NextQuestion() {
		curQuestion = questions.GetNextQuestion (AllowNewQuestion());
		foreach (OnQuestionChanged subscriber in onQuestionChangedSubscribers) {
			subscriber.OnQuestionChanged (curQuestion);
		}
	}

	public void OnAnswer(string answer) {
		if (curQuestion == null || answer.Length == 0) {
			return;
		}
		bool isCorrect = curQuestion.IsAnswerCorrect (answer);
		curQuestion.UpdateInterval (isCorrect);
		if (isCorrect) {
			foreach (OnCorrectAnswer subscriber in onCorrectAnswerSubscribers) {
				subscriber.OnCorrectAnswer (curQuestion);
			}

		} else {
			++numWrong;
			foreach (OnWrongAnswer subscriber in onWrongAnswerSubscribers) {
				subscriber.OnWrongAnswer ();
			}
		}
	}

	/* goal: No more than maxPerDay in a day.
	 * Store a persistant list of questions answered with timestamps.
	 * On start, erase all the logged questions from yesterday (local time) and count the rest.
	 * Calculate the number due by the end of the day.
	 * Sum these. If it is less than maxPerDay, you can add a new question.
	 * This doesn't work perfectly, but should be close enough.
	 * Say maxPerDay is 20. Initially: t = 0, 0 + 0 => Q1, t = 30 (review 55s), 1 + 1 => Q2, 1m (r1m25s), 2 + 2 => Review Q1, 1m30 (r3m30), 3 + 2 => RQ2, 2m (r4m), 4 + 2 => Q3, 2m30 (r2m55), 5 + 3 => Q4, 3m (r3m25), 6 + 4 => Q3, 3m30 (r5m30), 7 + 4 => RQ4, 4m (6m), 8 + 4 => RQ1, 4m30 (r14m30), 9 + 4 => RQ2, 5m (15m), 10 + 4 => Q5, 5m (5m25), 10 + 5 => Q6, 5m30 (5m55), 11 + 6 => RQ5, 6m (8m), 12 + 6 => RQ3, 6m30 (16m30), 13 + 6 => RQ6, 7m (9m), 14 + 6 => RQ4, 7m30 (17m30), 15 + 6 => DONE (15 questions). But just a little slower and you get RQ5, 8m30 (18m30), 16 + 6, and RQ6, 9m30 (19m30), 17 + 6. (17 questions)
//Q1 14m30, RQ2 15m, RQ3 16m30, RQ4 17m30, RQ5 18m30, RQ6 19m30
	 * Next day, you get: t = 0, 0 + 6 => RQ1, 30s (r1h30s), 1 + 6 => RQ2, 1m (r1h1m), 2 + 6, RQ3, 1m30 (r1h1m30), 3 + 6 => RQ4, 2m (r1h2m), 4 + 6 => RQ5, 2m30 (r1h2m30), 5 + 6, RQ6, 3m (r1h3m), 6 + 6 => Q7, 3m30 (r3m55), 7 + 7 => Q8, 4m (r4m25), 8 + 8 => RQ7, 4m30 (6m30), 9 + 8 => RQ8, 5m (7m), 10 + 8 => Q9, 5m30 (5m55), 11 + 9 => Q10, 6m (6m25), 12 + 10 => RQ9, 6m30 (8m30), 13 + 10 => RQ10, 7m (r9m), 14 + 10 => RQ7, 7m30 (17m30), 15 + 10 => RQ8, 8m (18m), 16 + 10 => fudge RQ9, 9m (19m), 17 + 10 => RQ10, 9m30 (19m30), 18 + 10 => DONE. 18 questions in session.
	 * If on the first day you had maxed out your answering with later sessions, you would have (get notification when there are at least three questions to answer):
	 * t = 16m30, 17 + 6 => RQ1, 17m (1h17m), 18 + 6 => RQ2, 17m30 (1h17m30), 19 + 6 => RQ3, 18m (1h18m), 20 + 6 => RQ4, 18m30 (1h18m30), 21 + 6 => RQ5, 19m (1h19m), 22 + 6 => DONE. 5 questions in session.
	 * t = 1h17m30, 22 + 6 => RQ6, 1h18m (2h18m), 23 + 6 => RQ1, 1h18m30 (6h18m30), 24 + 6 => RQ2, 1h19m (6h19m), 25 + 6 => RQ3, 1h19m30 (r6h19m30), 26 + 6 => RQ4, 1h20m(r6h20m), 27 + 6 => RQ5, 1h20m30(r6h20m30), 28 + 6 => DONE. 6 questions in session.
	 * t = 6h19m, 28 + 6 => RQ6, 6h19m30s (11h19m30), 29 + 6 => RQ1, 6h20m (30h20m), 30 + 5 => RQ2, 6h20m30 (30h20m30), 31 + 4 => RQ3, 6h21m (30h21m), 32 + 3 => RQ4, 6h21m30 (30h21m30), 33 + 2 => RQ5, 6h22m (30h22m), 34 + 1 => DONE (6 questions in session)
	 * Total questions on day 1: 34. Then the next day would be:
	 * t = 0, 0 + 6 => RQ6, 30s (24h30s), 1 + 5 => RQ1 through 5, 5m (r 5 days later), 6 + 0 => Q7, 5m30 (5m55), 7 + 1 => Q8, 6m (6m25), 8 + 2 => RQ7, 6m30 (8m30), 9 + 2 => RQ8, 7m (9m), 10 + 2 => Q9, 7m30 (7m55), 10 + 3 => Q10, 8m (8m25), 11 + 4 => RQ9, 8m30 (10m30), 12 + 4 => RQ10, 9m (11m), 13 + 4 => RQ7, 9m30 (19m30), 14 + 4 => RQ8, 10m (20m), 15 + 4 => RQ9, 11m (21m), 16 + 4 => RQ10, 11m30 (21m30), 17 + 4 => DONE. 17 questions in this session.
	*/
	bool AllowNewQuestion() {

		return (numWrong < minNumWrong && Time.time <= maxRecommendedPlayMinutes) || Time.time <= minPlayMinutes * 60;
	}

	void TestAllowNewQuestion() {
		TestAnswerWhenDue ();
//		TestAnswerWhenNotified ();
//		TestAnswerTwicePerDay ();
//		TestAnswerOncePerDay ();
	}

	void TestAnswerWhenDue() {
		
	}

	// I can't figure out a way to get the editor to display a list of OnQuestionChangeds (since an Interface can't be Serializable)...
	private void SplitSubscribers() {
		onQuestionChangedSubscribers = new List<OnQuestionChanged> ();
		onCorrectAnswerSubscribers = new List<OnCorrectAnswer> ();
		onWrongAnswerSubscribers = new List<OnWrongAnswer> ();
		foreach(GameObject subscriber in subscribers) {
			OnQuestionChanged[] onQuestionChangeds = subscriber.GetComponents<OnQuestionChanged>();
			foreach(OnQuestionChanged s in onQuestionChangeds) {
				onQuestionChangedSubscribers.Add (s);
			}
			OnCorrectAnswer[] onCorrectAnswers = subscriber.GetComponents<OnCorrectAnswer>();
			foreach(OnCorrectAnswer s in onCorrectAnswers) {
				onCorrectAnswerSubscribers.Add (s);
			}
			OnWrongAnswer[] onWrongAnswers = subscriber.GetComponents<OnWrongAnswer>();
			foreach(OnWrongAnswer s in onWrongAnswers) {
				onWrongAnswerSubscribers.Add (s);
			}
		}
	}
}
