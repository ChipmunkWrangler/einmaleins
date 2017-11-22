﻿using UnityEngine;

public class EffortTracker : MonoBehaviour, OnWrongAnswer, OnCorrectAnswer, OnGiveUp {
	[SerializeField] Goal goal = null;
	[SerializeField] Questions questions = null;
	[SerializeField] Fuel fuel = null;

	const int FRUSTRATION_WRONG = 2; // N.B. Since the question is repeated until it is correct, the net effect will be FRUSTRATION_WRONG * n - FRUSTRATION_RIGHT (or _FAST)
	const int FRUSTRATION_GIVE_UP = 2;
	const int FRUSTRATION_RIGHT = -1;
	const int FRUSTRATION_FAST = -2;
	const int MIN_FRUSTRATION = -2;
	const int MAX_FRUSTRATION = 3;
	const int MIN_QUIZZES_PER_DAY = 3;
	const float MIN_TIME_PER_DAY = 5 * 60.0f;
	const int NUM_ANSWERS_PER_QUIZ = 7; // the bigger this is, the more new questions the kid will be confronted with at once
	const int GAUNTLET_ASK_LIST_LENGTH = 55;
	const int NUM_ANSWERS_LEFT_WHEN_LAUNCH_CODE_ASKED = 3;

	int _numAnswersLeftInQuiz;
	int numAnswersLeftInQuiz { 
		get {
			return _numAnswersLeftInQuiz;
		}
		set {
			_numAnswersLeftInQuiz = value;
			fuel.UpdateFuelDisplay (_numAnswersLeftInQuiz);
		}
	}
	bool isQuizStarted;
	int _frustration;
	int frustration {
		get { return _frustration; }
		set { _frustration = Mathf.Clamp (value, MIN_FRUSTRATION, MAX_FRUSTRATION); }
	}
	int quizzesToday = -1;
	float timeToday;
	const string prefsKey = "effortTracking";

	public bool IsDoneForToday() {
		if (quizzesToday < 0) {
			Load ();
		}
		return quizzesToday >= MIN_QUIZZES_PER_DAY && timeToday >= MIN_TIME_PER_DAY;
	}

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		float answerTime = question.GetLastAnswerTime ();
		timeToday += answerTime + Celebrate.duration;
		frustration += (answerTime <= Question.FAST_TIME) ? FRUSTRATION_FAST : FRUSTRATION_RIGHT;
		if (isQuizStarted) {
			--numAnswersLeftInQuiz;
		}
	}

	public Question GetQuestion() {
		if (!isQuizStarted) {
			StartQuiz ();
		}
		Debug.Log ("frustration = " + frustration + " numAnswersInQuiz " + numAnswersLeftInQuiz);
		if (numAnswersLeftInQuiz <= 0) {
			return null;
		}
		bool isFrustrated = frustration > 0;
		if (numAnswersLeftInQuiz <= NUM_ANSWERS_LEFT_WHEN_LAUNCH_CODE_ASKED && !isFrustrated) {
			Question q = questions.GetLaunchCodeQuestion ();
			if (q != null) {
				return q;
			}
		}
		return questions.GetQuestion (isFrustrated);
	}

	public void OnWrongAnswer(bool wasNew) {
		frustration += FRUSTRATION_WRONG;
		if (isQuizStarted) {
			--numAnswersLeftInQuiz;
		}
	}

	public void OnGiveUp(Question question) {
		frustration += FRUSTRATION_GIVE_UP;
	}

	public static int GetNumAnswersInQuiz(bool isGauntlet) {
		return isGauntlet ? GAUNTLET_ASK_LIST_LENGTH : NUM_ANSWERS_PER_QUIZ;
	}

	void StartQuiz() {
		Load ();
		Goal.CurGoal curGoal = goal.calcCurGoal();
		UnityEngine.Assertions.Assert.IsTrue (curGoal == Goal.CurGoal.FLY_TO_PLANET || curGoal == Goal.CurGoal.GAUNTLET || curGoal == Goal.CurGoal.WON, "unexpected goal " + curGoal);
		numAnswersLeftInQuiz = GetNumAnswersInQuiz(curGoal == Goal.CurGoal.GAUNTLET);
		questions.ResetForNewQuiz();
		isQuizStarted = true;
	}

	public void EndQuiz() {
		isQuizStarted = false;
		RocketParts.instance.justUpgraded = false;
		++quizzesToday;
		Save ();
	}

	public void Save() {
		MDPrefs.SetDateTime (prefsKey + ":date", System.DateTime.Today);
		MDPrefs.SetInt (prefsKey + ":frustration", frustration);
		MDPrefs.SetInt (prefsKey + ":quizzesToday", quizzesToday);
		MDPrefs.SetFloat (prefsKey + ":timeToday", timeToday);
		questions.Save ();
	}

	public void Load() {
		frustration = MDPrefs.GetInt (prefsKey + ":frustration", 0);
		if (MDPrefs.GetDateTime (prefsKey + ":date", System.DateTime.MinValue) < System.DateTime.Today) {
			quizzesToday = 0;
			timeToday = 0;
		} else {
			quizzesToday = MDPrefs.GetInt (prefsKey + ":quizzesToday", 0);
			timeToday = MDPrefs.GetFloat (prefsKey + ":timeToday", 0);
		}
	}
}
