using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Question {
	public int a {  get; private set; }
	public int b { get; private set; }
	public int idx { get; private set; }
	public int difficulty { get; private set; }
	public System.DateTime reviewAt { get; private set; }

	public float Debug_chanceOfCorrectAnswer;

	public const int NEW_CARD_DIFFICULTY = 3;

	const int MASTERED_DIFFICULTY = 0;
	const float FAST_TIME = 15.0f;
	const float OK_TIME = 60.0f;
	const int ADD_TO_DIFFICULTY_FAST = -3;
	const int ADD_TO_DIFFICULTY_OK = -1;
	const int ADD_TO_DIFFICULTY_WRONG = 2;
	const int MAX_DIFFICULTY = 5;
	const int NUM_ANSWER_TIMES_TO_RECORD = 3;
	readonly int[] reviewInSecondsByDifficulty = {
		5 * 60 * 60 * 24, // not actually used
		1 * 60 * 60 * 24,
		1 * 60 * 60,
		10 * 60,
		2 * 60,
		25
	};
	string prefsKey;
	List<float> answerTimes;
	bool wasMastered; // even if it is no longer mastered. This is for awarding rocket parts
	bool isRetry; // if a question is answered wrong, then isRetry is true until it is answered right (which must happen before proceeding to a new question)
	bool isMandatoryReview; // if you get a question wrong, you have to review it in this session regardless of difficulty level and reviewAt

	public Question(int _a, int _b) {
		a = _a;
		b = _b;
		difficulty = NEW_CARD_DIFFICULTY;
	}

	public bool IsAnswerCorrect(string answer) {
		int result;
		int correctAnswer = a * b;
		if (int.TryParse (answer, out result)) {
			return result == correctAnswer;
		}
		return false;
	}

	public bool IsNew() {
		return reviewAt == System.DateTime.MinValue && !isRetry;
	}

	public bool IsMastered() {
		return difficulty <= MASTERED_DIFFICULTY;
	}

	public bool IsUrgent() {
		return isMandatoryReview;
	}

	public bool LastAnswerWasFast() {
		return answerTimes.Count > 0 && answerTimes [answerTimes.Count - 1] < FAST_TIME;
	}

	public float GetAverageAnswerTime() {
		return (answerTimes.Count == 0) ? float.MaxValue : answerTimes.Average ();
	}

	public bool UpdateState(bool isCorrect, float timeRequired) {
		bool isNewlyMastered = false;
		if (isCorrect) {
			RecordAnswerTime (timeRequired);
			if (isRetry) {
				isRetry = false;
			} else { // right first try!
				if (!IsMastered()) {
					difficulty += (timeRequired <= FAST_TIME && !isMandatoryReview) ? ADD_TO_DIFFICULTY_FAST : ADD_TO_DIFFICULTY_OK;  // if you got it right, but you got it wrong like a minute ago, being fast isn't so impressive
					if (IsMastered()) {
						difficulty = MASTERED_DIFFICULTY;
						isNewlyMastered = !wasMastered;
						wasMastered = true;
					}
				} // else once it is mastered we leave it alone
				isMandatoryReview = false;
			}
			difficulty = Mathf.Clamp (difficulty, MASTERED_DIFFICULTY, MAX_DIFFICULTY);
			reviewAt = CCTime.Now().AddSeconds(reviewInSecondsByDifficulty[difficulty]);
		} else {
			difficulty += ADD_TO_DIFFICULTY_WRONG;
			difficulty = Mathf.Clamp (difficulty, MASTERED_DIFFICULTY, MAX_DIFFICULTY);
			isRetry = true;
			isMandatoryReview = true;
		}
		UnityEngine.Assertions.Assert.IsFalse (IsNew());
		Debug.Log(ToString());
		return isNewlyMastered;
	}

//	public void Load(int idx) {
//		stage = Stage.Hard;
//		switch (idx) {
//		case 0: // 1x1
//		case 1: // 1x2
//		case 2: // 1x3
//		case 3: // 1x4
//		case 10: // 2x2
//		case 11: // 2x3
//		case 12: // 2x4
//		case 19: // 3x3
//		case 20: // 3x4	
//			correctInARow = 5;
//			break;
//		case 4: // 1x5
//		case 5: // 1x6
//		case 6: // 1x7
//		case 13: // 2x5
//		case 14: // 2x6
//		case 15: // 2x7
//		case 21: // 3x5
//		case 22: // 3x6
//		case 23: // 3x7
//		case 27: // 4x4
//		case 29: // 4x6
//		case 30: // 4x7
//		case 34: // 5x5
//		case 35: // 5x6
//		case 40: // 6x6
//		case 41: // 6x7
//		case 45: // 7x7
//			correctInARow = 4;
//			break;
//		case 7: // 1x8
//		case 16: // 2x8
//		case 24: // 3x8
//		case 31: // 4x8
//		case 36: // 5x7
//			correctInARow = 3;			
//			break;
//		case 28: // 4x5
//		case 37: // 5x8
//		case 42: // 6x8
//			correctInARow = 2;
//			break;
//		case 46: // 7x8
//		case 49: // 8x8
//			correctInARow = 1;
//			break;
//		case 8: // 1x9
//		case 9: // 1x10
//		case 17: // 2x9
//		case 18: // 2x10
//		case 25: // 3x9
//		case 26: // 3x10
//		case 32: // 4x9
//		case 33: // 4x10
//		case 38: // 5x9
//		case 39: // 5x10
//		case 43: // 6x9
//		case 44: // 6x10
//		case 47: // 7x9
//		case 48: // 7x10			
//		default:
//			stage = Stage.Inactive;
//			break;
//		}
//	}
		
	public void Load(string _prefsKey, int _idx) {
		prefsKey = _prefsKey;
		idx = _idx;
		difficulty = MDPrefs.GetInt (prefsKey + ":difficulty", NEW_CARD_DIFFICULTY);
		answerTimes = MDPrefs.GetFloatArray (prefsKey + ":times").ToList();
		wasMastered = MDPrefs.GetBool (prefsKey + ":wasMastered");
		reviewAt = MDPrefs.GetDateTime (prefsKey + ":reviewAt", System.DateTime.MinValue);
		isRetry = MDPrefs.GetBool (prefsKey + ":isRetry");
		isMandatoryReview = MDPrefs.GetBool (prefsKey + ":isMandatoryReview");
	}

	public void Save() {
		UnityEngine.Assertions.Assert.AreNotEqual (prefsKey.Length, 0);
		MDPrefs.SetInt(prefsKey + ":difficulty", difficulty);
		MDPrefs.SetFloatArray (prefsKey + ":times", answerTimes.ToArray());
		MDPrefs.SetBool (prefsKey + ":wasMastered", wasMastered);
		MDPrefs.SetDateTime (prefsKey + ":reviewAt", reviewAt);
		MDPrefs.SetBool (prefsKey + ":isRetry", isRetry);
		MDPrefs.SetBool (prefsKey + ":isMandatoryReview", isMandatoryReview);
	}

	public override string ToString() {
		string s = idx + " is " + a + " * " + b + " : difficulty = " + difficulty + " wasMastered = " + wasMastered + " isRetry " + isRetry + " isMandatoryReview " + isMandatoryReview + " times = ";
		foreach (var time in answerTimes) {
			s += time + " ";
		}
		s += " reviewAt " + reviewAt;
		return s;
	}

	void RecordAnswerTime (float timeRequired)
	{
		if (answerTimes.Count >= NUM_ANSWER_TIMES_TO_RECORD) {
			answerTimes.RemoveRange (0, 1 + answerTimes.Count - NUM_ANSWER_TIMES_TO_RECORD);
		}
		answerTimes.Add (timeRequired);

	}
}
