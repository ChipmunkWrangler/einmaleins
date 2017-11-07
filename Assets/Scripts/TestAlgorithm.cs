using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestAlgorithm : MonoBehaviour {
	const float TARGET_TIME = 3.0f;
	const int QUIZZES_PER_DAY = 3;
	const int MAX_QUESTIONS_PER_QUIZ = 11;
	const float MAX_TIME_PER_QUIZ = TARGET_TIME * MAX_QUESTIONS_PER_QUIZ;
	public static readonly float[] PLANET_HEIGHTS = TargetPlanet.heights;
	const float MIN_ANSWER_TIME = 1.0f;
	const int FRUSTRATION_WRONG = 2; // N.B. Since the question is repeated until it is correct, the net effect will be FRUSTRATION_WRONG * n - FRUSTRATION_RIGHT (or _FAST)
	const int FRUSTRATION_RIGHT = -1;
	const int FRUSTRATION_FAST = -2;
	const int MIN_FRUSTRATION = -2;
	const int MAX_FRUSTRATION = 3;
	const float HEIGHT_FRACTION_PER_CORRECT = 1.0f / (MAX_QUESTIONS_PER_QUIZ - 1);
	const float HEIGHT_FRACTION_PER_MASTERY_BOOST = (float)(TestQuestion.NUM_ANSWER_TIMES_TO_RECORD) / MAX_QUESTIONS_PER_QUIZ;
	const float HEIGHT_FRACTION_AFTER_WRONG = HEIGHT_FRACTION_PER_CORRECT * 0.25f;
	const int PARTS_PER_UPGRADE = 11;
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
//			Debug.Log ("Chance of correctness = " + chance);
			return Random.Range(0, 100) < chance;
		}

		public float AnswerTime(TestQuestion question) {
			float maxTime = Mathf.Max (TARGET_TIME, initialAnswerTimeMax - answerTimeImprovementRate * question.timesAnsweredCorrectly + question.baseDifficulty);
//			Debug.Log ("maxTime of correctness = " + maxTime);
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
			return GetAverageAnswerTime () < TARGET_TIME;
		}

		public void ResetForNewQuiz() {
			wasAsked = false;
		}
			
		public void Ask() {
			wasWrong = false;
			wasAsked = true;
			isNew = false;
		}

		public bool AnswerRight(float time) {
			++timesAnsweredCorrectly;
			bool wasMastered = IsMastered ();
			RecordAnswerTime (time);
			if (time <= TARGET_TIME) {
				++timesAnsweredFast;
			} 
			return !wasMastered && IsMastered ();
		}

		public void AnswerWrong() {
			++timesAnsweredWrong;
			wasWrong = true;
		}

		public float GetAverageAnswerTime() {
			return answerTimes.Average ();
		}

		override public string ToString() {
			string s = "Q" + baseDifficulty + " Answered wrong " + timesAnsweredWrong + " correct " + timesAnsweredCorrectly + " fast " + timesAnsweredFast + " averageTime = " + GetAverageAnswerTime() + " times ";
			foreach (var time in answerTimes) {
				s += time + " ";
			}
			return s;
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
//		Test (new KidModel( 1000, 5, -999.0f, 10.0f) );
		Test (new KidModel( 110, 5, -7.0f, 10.0f) );
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
		int upgradeLevel = 0;
		int rocketParts = 0;
		int frustration = 0;
		float recordHeight = 0;
		for (int day = 0; targetPlanet < PLANET_HEIGHTS.Length; ++day) {//!AreAllQuestionsMastered()
			Debug.Log ("Day = " + day);
			TestDay(kid, ref targetPlanet, ref upgradeLevel, ref rocketParts, ref frustration, ref recordHeight);
		}
		foreach (var question in questions) {
			Debug.Log (question);
		}
	}

	void TestDay (KidModel kid, ref int targetPlanet, ref int upgradeLevel, ref int rocketParts, ref int frustration, ref float recordHeight) {
		for (int i = 0; i < QUIZZES_PER_DAY && targetPlanet < PLANET_HEIGHTS.Length; ++i ) {
			Debug.Log ("Quiz " + i + " upgradeLevel = " + upgradeLevel + " targetPlanet " + targetPlanet + " rocketparts = " + rocketParts + " frustration = " + frustration);
			TestQuiz (kid, ref targetPlanet, ref upgradeLevel, ref rocketParts, ref frustration, ref recordHeight);
		}
	}

	void TestQuiz(KidModel kid, ref int targetPlanet, ref int upgradeLevel, ref int rocketParts, ref int frustration, ref float recordHeight) {
		float height = 0;
		float time = 0;
		float questionsAnswered = 0;
		bool isNewRecord = false;
		bool breakEarly = false;
		int numNew = 0;
		int numMastered = 0;
		int numWrong = 0;
		bool reachedNewPlanet = false;
		bool gotUpgrade = false;

		ResetQuestionsForNewQuiz ();
		while (time <= MAX_TIME_PER_QUIZ && !breakEarly) {
			TestQuestion nextQuestion = GetNextQuestion (frustration);
			if (nextQuestion == null) {
				break;
			}
			if (nextQuestion.isNew) {
				++numNew;
			}
			nextQuestion.Ask ();
//			Debug.Log("Frustration = " + frustration + " " + nextQuestion);
			float questionTime = kid.AnswerTime (nextQuestion);
			while( questionTime + time <= MAX_TIME_PER_QUIZ) {
				if (kid.AnswersCorrectly (nextQuestion)) {
					break;
				}
				frustration += FRUSTRATION_WRONG;
				nextQuestion.AnswerWrong ();
				questionTime += kid.AnswerTime (nextQuestion);
			}
			if (questionTime + time > MAX_TIME_PER_QUIZ) {
				break; // we don't get to answer the question
			}
			++questionsAnswered;
			frustration += (questionTime <= TARGET_TIME) ? FRUSTRATION_FAST : FRUSTRATION_RIGHT;
			frustration = Mathf.Clamp (frustration, MIN_FRUSTRATION, MAX_FRUSTRATION);
			bool isNewlyMastered = nextQuestion.AnswerRight (questionTime);
			if (nextQuestion.wasWrong) {
				++numWrong;
			}
//			Debug.Log("Answered " + nextQuestion);
			time += questionTime;
			height += (nextQuestion.wasWrong ? HEIGHT_FRACTION_AFTER_WRONG : HEIGHT_FRACTION_PER_CORRECT) * GetTargetHeight(upgradeLevel);
			if (isNewlyMastered) {
				++rocketParts;
				++numMastered;
				if (rocketParts >= PARTS_PER_UPGRADE) {
					rocketParts -= PARTS_PER_UPGRADE;
					++upgradeLevel;
					gotUpgrade = true;
					breakEarly = true;
				}
			}
			if (height > recordHeight) {
				recordHeight = height;
				isNewRecord = true;
				if (targetPlanet < PLANET_HEIGHTS.Length && recordHeight >= PLANET_HEIGHTS [targetPlanet]) {
					++targetPlanet;
					reachedNewPlanet = true;
					breakEarly = true;
				}
			}

		}
		Debug.Log ("Answered " + questionsAnswered + " questions (" + numNew + " new, " + numWrong + " wrong, " + numMastered + " mastered) in " + time + " seconds. Reached " + height + (isNewRecord ? " new record" : "") + (reachedNewPlanet ? (" reached planet " + (targetPlanet-1)) : "") + (gotUpgrade ? (" gotUpgrade " + upgradeLevel) : ""));
	}

	TestQuestion GetNextQuestion(int frustration) {
		var allowed = questions.Where (IsAllowed);
//		var candidates = allowed.OrderBy (question => question.wasWrong).ThenBy(question => !question.isNew).ThenBy(q => q.GetAverageAnswerTime);

		if (!allowed.Any()) {
			allowed = questions.Where (question => !question.wasAsked);
			if (!allowed.Any ()) {
				return null;
			}
		}
		var candidates = allowed.Where (question => question.wasWrong);
		if (!candidates.Any ()) {
			candidates = allowed.Where (question => !question.isNew);
			if (!candidates.Any ()) {
//				return (frustration > 0) ? allowed.First () : allowed.ElementAt(Random.Range(0, allowed.Count()));
				return allowed.First ();
			}
		}
		var orderedCandidates = candidates.OrderBy (q => q.GetAverageAnswerTime ());
		return (frustration > 0) ? orderedCandidates.First () : orderedCandidates.Last ();
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

	float GetTargetHeight(int upgradeLevel) {
		return (upgradeLevel < PLANET_HEIGHTS.Length) ? PLANET_HEIGHTS [upgradeLevel] : PLANET_HEIGHTS [PLANET_HEIGHTS.Length - 1] * 2.0f;
	}
}
