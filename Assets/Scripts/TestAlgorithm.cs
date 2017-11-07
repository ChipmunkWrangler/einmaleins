using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestAlgorithm : MonoBehaviour {
	const float TARGET_TIME = 3.0f;
	const float CELEBRATION_TIME = 3.0f;
	const int QUIZZES_PER_DAY = 3;
	const int MAX_QUESTIONS_PER_QUIZ = 33;
	const float MAX_TIME_PER_QUIZ = (TARGET_TIME + CELEBRATION_TIME) * MAX_QUESTIONS_PER_QUIZ;
	public static readonly float[] PLANET_HEIGHTS = TargetPlanet.heights;
	const float MIN_ANSWER_TIME = 1.0f;
	const int FRUSTRATION_WRONG = 2; // N.B. Since the question is repeated until it is correct, the net effect will be FRUSTRATION_WRONG * n - FRUSTRATION_RIGHT (or _FAST)
	const int FRUSTRATION_RIGHT = -1;
	const int FRUSTRATION_FAST = -2;
	const int MIN_FRUSTRATION = -2;
	const int MAX_FRUSTRATION = 3;
	const float HEIGHT_FRACTION_PER_CORRECT = 1.0f / (MAX_QUESTIONS_PER_QUIZ - 1);
	const float HEIGHT_FRACTION_PER_MASTERY_BOOST = (float)(TestQuestion.NUM_ANSWER_TIMES_TO_RECORD) / MAX_QUESTIONS_PER_QUIZ;

	class KidModel {
		int initialChanceOfCorrect; // lowered by difficulty
		int improvementRate;
		float initialAnswerTimeMax; // increased by difficulty
		float answerTimeImprovementRate;

		public KidModel(int _initialChanceOfCorrect, int _improvementRate, float _initialAnswerTimeMax, float _answerTimeImprovementRate) {
			initialChanceOfCorrect = _initialChanceOfCorrect;
			improvementRate = _improvementRate;
			initialAnswerTimeMax  = _initialAnswerTimeMax;
			answerTimeImprovementRate = _answerTimeImprovementRate;
		}

		public bool AnswersCorrectly(TestQuestion question) {
			int chance = initialChanceOfCorrect + improvementRate * question.timesAnsweredCorrectly - question.baseDifficulty;
			Debug.Log ("Chance of correctness = " + chance);
			return Random.Range(0, 100) < chance;
		}

		public float AnswerTime(TestQuestion question) {
			float maxTime = Mathf.Max (TARGET_TIME, initialAnswerTimeMax - answerTimeImprovementRate * question.timesAnsweredCorrectly + question.baseDifficulty);
			Debug.Log ("maxTime of correctness = " + maxTime);
			return Random.Range (MIN_ANSWER_TIME, maxTime);
		}
	}

	class TestQuestion {
		public int baseDifficulty  { get; private set; }
		public int timesAnsweredCorrectly { get; private set; }
		public bool wasWrong { get; private set; }
		public bool wasAsked { get; private set; }
		public bool isNew { get; private set; }
		public int masteredForPlanet { get; private set; }

		List<float> answerTimes;
		int timesAnsweredWrong = 0;
		int timesAnsweredFast = 0;

		public const int NUM_ANSWER_TIMES_TO_RECORD = 3;
		const float ANSWER_TIME_MAX = 60.0f;
		const float INITIAL_ANSWER_TIME = TARGET_TIME + 0.01f;

		public TestQuestion(int _baseDifficulty) {
			baseDifficulty = _baseDifficulty;
			timesAnsweredCorrectly = 0;
			wasWrong = false;
			wasAsked = false;
			isNew = true;
			masteredForPlanet = -1;
			InitAnswerTimes ();
		}

		public bool IsMastered() {
			return masteredForPlanet >= 0;
		}

		public void ResetForNewQuiz() {
			wasAsked = false;
		}
			
		public void Ask() {
			wasWrong = false;
			wasAsked = true;
			isNew = false;
		}

		public void Answer(bool isCorrect, float time, int targetPlanet) {
			if (isCorrect) {
				++timesAnsweredCorrectly;
				RecordAnswerTime (time);
				if (time <= TARGET_TIME) {
					++timesAnsweredFast;
					if (!IsMastered() && GetAverageAnswerTime() < TARGET_TIME) {
						masteredForPlanet = targetPlanet;
					}
				} 
			} else {
				++timesAnsweredWrong;
				wasWrong = true;
			}
		}

		public float GetAverageAnswerTime() {
			return answerTimes.Average ();
		}

		override public string ToString() {
			return "Answered wrong " + timesAnsweredWrong + " correct " + timesAnsweredCorrectly + " fast " + timesAnsweredFast + " averageTime = " + GetAverageAnswerTime() + " times " + answerTimes ;
		}

		void InitAnswerTimes ()
		{
			answerTimes = new List<float> (NUM_ANSWER_TIMES_TO_RECORD);
			for (int i = 0; i < NUM_ANSWER_TIMES_TO_RECORD; ++i) {
				answerTimes.Add(INITIAL_ANSWER_TIME);
			}
		}

		void RecordAnswerTime (float timeRequired)
		{
			answerTimes.Add (Mathf.Min(timeRequired, ANSWER_TIME_MAX));
			if (answerTimes.Count > NUM_ANSWER_TIMES_TO_RECORD) {
				answerTimes.RemoveRange (0, answerTimes.Count - NUM_ANSWER_TIMES_TO_RECORD);
			}
		}

	}

	TestQuestion[] questions;

	const int NUM_QUESTIONS = 55;

	// Use this for initialization
	void Start () {
		Test (new KidModel( 100, 5, 5.0f, 10.0f) );
	}

	void InitQuestions() {
		questions = new TestQuestion[NUM_QUESTIONS];
		for (int i = 0; i < NUM_QUESTIONS; ++i) {
			questions[i] = new TestQuestion(i);
		}
	}	

	void Test(KidModel kid) {
		InitQuestions ();
		int targetPlanet = 0;
		int frustration = 0;
		float recordHeight = 0;
		for (int day = 0; targetPlanet < PLANET_HEIGHTS.Length && !AreAllQuestionsMastered(); ++day) {
			Debug.Log ("Day = " + day);
			TestDay(kid, ref targetPlanet, ref frustration, ref recordHeight);
		}
		foreach (var question in questions) {
			Debug.Log (question);
		}
	}

	void TestDay (KidModel kid, ref int targetPlanet, ref int frustration, ref float recordHeight) {
		for (int i = 0; i < QUIZZES_PER_DAY && targetPlanet < PLANET_HEIGHTS.Length; ++i ) {
			Debug.Log ("Quiz " + i + " targetPlanet = " + targetPlanet + " frustration = " + frustration);
			float height = TestQuiz (kid, targetPlanet, ref frustration);
			if (height > recordHeight) {
				recordHeight = height;
				Debug.Log ("New record height = " + recordHeight);
				if (recordHeight >= PLANET_HEIGHTS [targetPlanet]) {
					Debug.Log ("Reached planet " + targetPlanet);
					++targetPlanet;
				}
			}
		}
	}

	float TestQuiz(KidModel kid, int targetPlanet, ref int frustration) {
		int masteredForBoost = questions.Count(question => question.masteredForPlanet == targetPlanet);
		float height = masteredForBoost * HEIGHT_FRACTION_PER_MASTERY_BOOST * PLANET_HEIGHTS[targetPlanet];
		Debug.Log("Initial boost = " + masteredForBoost + " height = " + height);
		float time = 0;
		float questionsAsked = 0;
		ResetQuestionsForNewQuiz ();
		for (questionsAsked = 0; questionsAsked < MAX_QUESTIONS_PER_QUIZ && time <= MAX_TIME_PER_QUIZ; ++questionsAsked) {
			float questionTime = 0;
			TestQuestion nextQuestion = GetNextQuestion (frustration);
			nextQuestion.Ask ();
			Debug.Log("Frustration = " + frustration + " " + nextQuestion);
			while (!kid.AnswersCorrectly (nextQuestion)) {
				Debug.Log ("Wrong");
				questionTime += kid.AnswerTime (nextQuestion);
				nextQuestion.Answer (false, questionTime, targetPlanet);
				frustration += FRUSTRATION_WRONG;
			}
			questionTime += kid.AnswerTime (nextQuestion);
			frustration += (questionTime <= TARGET_TIME) ? FRUSTRATION_FAST : FRUSTRATION_RIGHT;
			frustration = Mathf.Clamp (frustration, MIN_FRUSTRATION, MAX_FRUSTRATION);
			nextQuestion.Answer (true, questionTime, targetPlanet);

			time += questionTime;
			height += HEIGHT_FRACTION_PER_CORRECT * PLANET_HEIGHTS[targetPlanet];
		}
		Debug.Log ("Asked " + questionsAsked + " questions in " + time + " seconds. Reached " + height);
		return height;
	}

	TestQuestion GetNextQuestion(int frustration) {
		var candidates = questions.Where (IsAllowed);
		if (candidates.Any (question => question.wasWrong)) {
			var orderedWrongCandidates = candidates.Where (question => question.wasWrong).OrderBy (q => q.GetAverageAnswerTime ());
			return (frustration > 0) ? orderedWrongCandidates.First () : orderedWrongCandidates.Last ();
		} else if (candidates.Any (question => !question.isNew)) {
			var orderedNonNewCandidates = candidates.Where (question => !question.isNew).OrderBy (q => q.GetAverageAnswerTime ());
			return (frustration > 0) ? orderedNonNewCandidates.First () : orderedNonNewCandidates.Last ();
		} else {
			return candidates.First ();
		}
	}

	bool AreAllQuestionsMastered() {
		return questions.All (question => question.IsMastered ());
	}

	void ResetQuestionsForNewQuiz() {
		foreach (TestQuestion question in questions) {
			question.ResetForNewQuiz ();
		}
	}

	bool IsAllowed(TestQuestion question) {
		return !question.wasAsked && !question.IsMastered ();
	}

}
