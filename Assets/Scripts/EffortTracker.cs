using UnityEngine;

public class EffortTracker : MonoBehaviour, IOnWrongAnswer, IOnGiveUp {
    [SerializeField] Goal goal = null;
    [SerializeField] Questions questions = null;
    [SerializeField] Fuel fuel = null;

    const int FrustrationWrong = 2; // N.B. Since the question is repeated until it is correct, the net effect will be FRUSTRATION_WRONG * n - FRUSTRATION_RIGHT (or _FAST)
    const int FrustrationGiveUp = 2;
    const int FrustrationRight = -1;
    const int FrustrationFast = -2;
    const int NumAnswersPerQuiz = 7; // the bigger this is, the more new questions the kid will be confronted with at once
    const int GauntletAskListLength = 55;
    const int NumAnswersLeftWhenLaunchCodeAsked = 3;
    readonly EffortTrackerPersistantData Data = new EffortTrackerPersistantData();

    int numAnswersLeftInQuiz;
    int NumAnswersLeftInQuiz { 
		get {
			return numAnswersLeftInQuiz;
		}
		set {
			numAnswersLeftInQuiz = value;
			fuel.UpdateFuelDisplay (numAnswersLeftInQuiz);
		}
	}
    bool isQuizStarted;
    bool allowGivingUp;

	public bool IsDoneForToday() => Data.IsDoneForToday ();

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		float answerTime = question.GetLastAnswerTime ();
		Data.TimeToday += answerTime + Celebrate.Duration;
		Data.Frustration += (answerTime <= Question.FastTime) ? FrustrationFast : FrustrationRight;
		if (isQuizStarted) {
			--NumAnswersLeftInQuiz;
		}
	}

	public Question GetQuestion() {
		if (!isQuizStarted) {
			StartQuiz ();
		}
		Debug.Log ("frustration = " + Data.Frustration + " numAnswersInQuiz " + NumAnswersLeftInQuiz);
		if (NumAnswersLeftInQuiz <= 0) {
			return null;
		}
		bool isFrustrated = Data.Frustration > 0;

		if (NumAnswersLeftInQuiz <= NumAnswersLeftWhenLaunchCodeAsked && !isFrustrated) {
			Question q = questions.GetLaunchCodeQuestion ();
			if (q != null) {
				return q;
			}
		}
		return questions.GetQuestion (isFrustrated, !allowGivingUp);
	}

	public void OnWrongAnswer(bool wasNew) {
		Data.Frustration += FrustrationWrong;
		if (isQuizStarted) {
			--NumAnswersLeftInQuiz;
		}
	}

	public void OnGiveUp(Question question) {
		Data.Frustration += FrustrationGiveUp;
	}

	public static int GetNumAnswersInQuiz(bool isGauntlet) {
		return isGauntlet ? GauntletAskListLength : NumAnswersPerQuiz;
	}

	void StartQuiz() {
		Data.Load ();
		Goal.CurGoal curGoal = goal.CalcCurGoal();
		UnityEngine.Assertions.Assert.IsTrue (curGoal == Goal.CurGoal.FLY_TO_PLANET || curGoal == Goal.CurGoal.GAUNTLET || curGoal == Goal.CurGoal.WON, "unexpected goal " + curGoal);
		allowGivingUp = Goal.IsGivingUpAllowed(curGoal);
		NumAnswersLeftInQuiz = GetNumAnswersInQuiz(curGoal == Goal.CurGoal.GAUNTLET);
		questions.ResetForNewQuiz();
		isQuizStarted = true;
	}

	public void EndQuiz() {
		isQuizStarted = false;
		RocketParts.Instance.JustUpgraded = false;
		++Data.QuizzesToday;
		Save ();
	}

	public void Save() {
		Data.Save ();
		questions.Save ();
	}
}

public class EffortTrackerPersistantData {
    public int QuizzesToday = -1;
    public float TimeToday;
    public int Frustration {
        get { return frustration; }
        set { frustration = Mathf.Clamp (value, MinFrustration, MaxFrustration); }
	}

    const string PrefsKey = "effortTracking";
	const int MinFrustration = -2;
	const int MaxFrustration = 3;
    const int MinQuizzesPerDay = 3;
    const float MinTimePerDay = 5 * 60.0F;

	int frustration;

	public bool IsDoneForToday() {
		if (QuizzesToday < 0) {
			Load ();
		}
		return QuizzesToday >= MinQuizzesPerDay && TimeToday >= MinTimePerDay;
	}

	public void Save() {
		MDPrefs.SetDateTime (PrefsKey + ":date", System.DateTime.Today);
		MDPrefs.SetInt (PrefsKey + ":frustration", Frustration);
		MDPrefs.SetInt (PrefsKey + ":quizzesToday", QuizzesToday);
		MDPrefs.SetFloat (PrefsKey + ":timeToday", TimeToday);
	}

	public void Load() {
		Frustration = MDPrefs.GetInt (PrefsKey + ":frustration", 0);
		if (MDPrefs.GetDateTime (PrefsKey + ":date", System.DateTime.MinValue) < System.DateTime.Today) {
			QuizzesToday = 0;
			TimeToday = 0;
		} else {
			QuizzesToday = MDPrefs.GetInt (PrefsKey + ":quizzesToday", 0);
			TimeToday = MDPrefs.GetFloat (PrefsKey + ":timeToday", 0);
		}
	}
}