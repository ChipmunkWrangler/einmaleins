using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestAlgorithm : MonoBehaviour {
	const float TARGET_TIME = 3.0f;
	const int MIN_QUIZZES_PER_DAY = 3;
	const int MIN_TIME_PER_DAY = 5 * 60;
	const int MAX_QUESTIONS_PER_QUIZ = 7; // because that's enough to get previous questions out of short term memory, I guess
	public static readonly float[] PLANET_HEIGHTS = TargetPlanet.heights;
	const float MIN_ANSWER_TIME = 1.0f;
	const int FRUSTRATION_WRONG = 2; // N.B. Since the question is repeated until it is correct, the net effect will be FRUSTRATION_WRONG * n - FRUSTRATION_RIGHT (or _FAST)
	const int FRUSTRATION_RIGHT = -1;
	const int FRUSTRATION_FAST = -2;
	const int MIN_FRUSTRATION = -2;
	const int MAX_FRUSTRATION = 3;
	const int PARTS_PER_UPGRADE = 11;
	const float MIN_THRUST_FACTOR = 0.1f;
	const int V_FACTOR = 4;
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
			int chance = initialChanceOfCorrect + improvementRate * (question.timesAnsweredCorrectly + question.timesAnsweredWrong) - question.baseDifficulty;
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
		public int timesAnsweredWrong { get; private set; }
		public bool wasWrong { get; private set; }
		public bool wasAsked { get; private set; }
		public bool isNew { get; private set; }
		public bool wasMastered { get; private set; }

		List<float> answerTimes;
		int timesAnsweredFast = 0;

		public const int NUM_ANSWER_TIMES_TO_RECORD = 3;
		const float ANSWER_TIME_MAX = 60.0f;
		const float INITIAL_ANSWER_TIME = TARGET_TIME + 0.01f;

		public TestQuestion(int _baseDifficulty) {
			baseDifficulty = _baseDifficulty;
			timesAnsweredCorrectly = 0;
			timesAnsweredWrong = 0;
			wasWrong = false;
			wasAsked = false;
			isNew = true;
			InitAnswerTimes ();
		}

		public bool IsMastered() => GetAverageAnswerTime () < TARGET_TIME;
        public float GetAverageAnswerTime() => answerTimes.Average();

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
			RecordAnswerTime (time);
			if (time <= TARGET_TIME) {
				++timesAnsweredFast;
			}
			bool newlyMastered = false;
			if (!wasMastered) {
				if (IsMastered ()) {
					newlyMastered = true;
					wasMastered = true;
				}
			}
			return newlyMastered;
		}

		public void AnswerWrong() {
			++timesAnsweredWrong;
			wasWrong = true;
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

	void Start () {
		Test (new KidModel( 1000, 5, -999.0f, 10.0f) );
		Test (new KidModel( 110, 5, -7.0f, 10.0f) );
		Test (new KidModel( 60, 3, 15.0f, 6.0f) );
		Test (new KidModel( 30, 1, 30.0f, 3.0f) );
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
		float maxThrustFactor = CalcMaxThrustFactor ();
		float q = CalcQ (MIN_THRUST_FACTOR, maxThrustFactor, V_FACTOR);
		for (int day = 0; !IsReadyForGauntlet(targetPlanet); ++day) {
			Debug.Log ("Day = " + day);
			TestDay(kid, maxThrustFactor, q, ref targetPlanet, ref upgradeLevel, ref rocketParts, ref frustration, ref recordHeight);
		}
		Debug.Log ("Ready for gauntlet. Num mastered = " + questions.Count (question => question.wasMastered) + " total right = " + questions.Sum(question => question.timesAnsweredCorrectly) + " total wrong = " + questions.Sum(question => question.timesAnsweredWrong)  );
		foreach (var question in questions) {
			Debug.Log (question);
		}
	}

	void TestDay (KidModel kid, float maxThrustFactor, float q, ref int targetPlanet, ref int upgradeLevel, ref int rocketParts, ref int frustration, ref float recordHeight) {
		float timeToday = 0;
		for (int i = 0; ( i < MIN_QUIZZES_PER_DAY || timeToday < MIN_TIME_PER_DAY ) && !IsReadyForGauntlet (targetPlanet); ++i ) {
			Debug.Log ("Quiz " + i + " upgradeLevel = " + upgradeLevel + " targetPlanet " + targetPlanet + " rocketparts = " + rocketParts + " frustration = " + frustration);
			timeToday += TestQuiz (kid, maxThrustFactor, q, ref targetPlanet, ref upgradeLevel, ref rocketParts, ref frustration, ref recordHeight);
		}
	}

	float TestQuiz(KidModel kid, float maxThrustFactor, float q, ref int targetPlanet, ref int upgradeLevel, ref int rocketParts, ref int frustration, ref float recordHeight) {
		float height = 0;
		float time = 0;
		float questionsAnswered = 0;
		bool isNewRecord = false;
		int numNew = 0;
		int numMastered = 0;
		int numWrong = 0;
		bool reachedNewPlanet = false;
		bool gotUpgrade = false;
		float baseThrust = GetTargetHeight (upgradeLevel) / MAX_QUESTIONS_PER_QUIZ;

		ResetQuestionsForNewQuiz ();
		for (int i = 0; i < MAX_QUESTIONS_PER_QUIZ - numWrong && !reachedNewPlanet; ++i) {
			TestQuestion nextQuestion = GetNextQuestion (frustration);
			if (nextQuestion.isNew) {
				++numNew;
			}
			nextQuestion.Ask ();
			Debug.Log("Frustration = " + frustration + " " + nextQuestion);
			float questionTime = kid.AnswerTime (nextQuestion);
			while( !kid.AnswersCorrectly (nextQuestion)) {
				frustration += FRUSTRATION_WRONG;
				nextQuestion.AnswerWrong ();
				questionTime += kid.AnswerTime (nextQuestion);
			}
			++questionsAnswered;
			frustration += (questionTime <= TARGET_TIME) ? FRUSTRATION_FAST : FRUSTRATION_RIGHT;
			frustration = Mathf.Clamp (frustration, MIN_FRUSTRATION, MAX_FRUSTRATION);
			bool isNewlyMastered = nextQuestion.AnswerRight (questionTime);
			if (nextQuestion.wasWrong) {
				++numWrong;
			}
			Debug.Log("Answered " + nextQuestion);
			time += questionTime;
//			Debug.Log(GetHeightIncrease(1.0f, MIN_THRUST_FACTOR, maxThrustFactor, q, 0, TARGET_TIME, V_FACTOR));
//			Debug.Log(GetHeightIncrease(1.0f, MIN_THRUST_FACTOR, maxThrustFactor, q, TARGET_TIME, TARGET_TIME, V_FACTOR));
//			Debug.Log(GetHeightIncrease(1.0f, MIN_THRUST_FACTOR, maxThrustFactor, q, 11.8f, TARGET_TIME, V_FACTOR));
//			Debug.Log(GetHeightIncrease(1.0f, MIN_THRUST_FACTOR, maxThrustFactor, q, 60.0f, TARGET_TIME, V_FACTOR));
//			q = CalcQ (MIN_THRUST_FACTOR, 2.0f, V_FACTOR);
//			Debug.Log(GetHeightIncrease(1.0f, MIN_THRUST_FACTOR, 2.0f, q, 0, TARGET_TIME, V_FACTOR));
//			Debug.Log(GetHeightIncrease(1.0f, MIN_THRUST_FACTOR, 2.0f, q, TARGET_TIME, TARGET_TIME, V_FACTOR));
//			Debug.Log(GetHeightIncrease(1.0f, MIN_THRUST_FACTOR, 2.0f, q, 11.8f, TARGET_TIME, V_FACTOR));
//			Debug.Log(GetHeightIncrease(1.0f, MIN_THRUST_FACTOR, 2.0f, q, 60.0f, TARGET_TIME, V_FACTOR));
			height += GetHeightIncrease(baseThrust, MIN_THRUST_FACTOR, maxThrustFactor, q, questionTime, TARGET_TIME, V_FACTOR);
			if (isNewlyMastered) {
				++rocketParts;
				++numMastered;
			}
			if (height > recordHeight) {
				if (targetPlanet < PLANET_HEIGHTS.Length && recordHeight >= PLANET_HEIGHTS [targetPlanet]) {
					height = PLANET_HEIGHTS [targetPlanet];
					++targetPlanet;
					reachedNewPlanet = true;
				}
				recordHeight = height;
				isNewRecord = true;
			}

		}
		while (rocketParts >= PARTS_PER_UPGRADE) {
			rocketParts -= PARTS_PER_UPGRADE;
			++upgradeLevel;
			gotUpgrade = true;
		}
		if (gotUpgrade || isNewRecord || reachedNewPlanet) {
			Debug.Log ("Answered " + questionsAnswered + " questions (" + numNew + " new, " + numWrong + " wrong, " + numMastered + " mastered) in " + time + " seconds. Reached " + height + (isNewRecord ? " new record" : "") + (reachedNewPlanet ? (" reached planet " + (targetPlanet - 1)) : "") + (gotUpgrade ? (" gotUpgrade " + upgradeLevel) : ""));
		}
		return time;
	}

	static bool IsReadyForGauntlet (int targetPlanet) => targetPlanet >= PLANET_HEIGHTS.Length - 1;

	TestQuestion GetNextQuestion(int frustration) {
		var allowed = questions.Where (IsAllowed);

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
				return (frustration > 0) ? allowed.First () : allowed.ElementAt(Random.Range(0, allowed.Count()));
			}
		}
		var orderedCandidates = candidates.OrderBy (q => q.GetAverageAnswerTime ());
		return (frustration > 0) ? orderedCandidates.First () : orderedCandidates.Last ();
	}

	void ResetQuestionsForNewQuiz() {
		foreach (TestQuestion question in questions) {
			question.ResetForNewQuiz ();
		}
	}

	bool IsAllowed(TestQuestion question) => !question.wasAsked && !question.IsMastered ();
    float GetTargetHeight(int upgradeLevel) => (upgradeLevel < PLANET_HEIGHTS.Length) ? PLANET_HEIGHTS [upgradeLevel] : PLANET_HEIGHTS [PLANET_HEIGHTS.Length - 1] * 2.0f;
	float GetHeightIncrease(float baseThrust, float minThrustFactor, float maxThrustFactor, float Q, float timeRequired, float allottedTime, int v) => baseThrust * (minThrustFactor + (maxThrustFactor - minThrustFactor) / Mathf.Pow(1.0f + Q * Mathf.Exp(timeRequired-allottedTime), 1.0f/v));

	float CalcMaxThrustFactor() {
		float minHeightRatio = float.MaxValue;
		for (int i = 0; i < PLANET_HEIGHTS.Length - 1; ++i) {
			float heightRatio = PLANET_HEIGHTS [i + 1] / PLANET_HEIGHTS [i];
			if (heightRatio < minHeightRatio) {
				minHeightRatio = heightRatio;
			}
		}
		return minHeightRatio;
	}

	float CalcQ(float minThrustFactor, float maxThrustFactor, int v) {
		float m = minThrustFactor;
		float M = maxThrustFactor;
		return Mathf.Pow((M - m) / (1.0f - m), v) - 1.0f;
	}
}
