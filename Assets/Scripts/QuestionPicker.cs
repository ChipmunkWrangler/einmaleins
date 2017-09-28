using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestionPicker : MonoBehaviour {
	[SerializeField] GameObject[] subscribers;
	[SerializeField] Questions questions;

	Question curQuestion;
	List<OnQuestionChanged> onQuestionChangedSubscribers;
	List<OnCorrectAnswer> onCorrectAnswerSubscribers;
	List<OnWrongAnswer> onWrongAnswerSubscribers;
	float questionTime;

	void Start () {
//		Test ();
		SplitSubscribers ();
	}

	public void Abort() {
		questions.Abort ();
		NextQuestion ();
	}

	public void Reset() {
		questions.Reset ();
	}

	public void NextQuestion() {
		curQuestion = questions.GetNextQuestion ();
		foreach (OnQuestionChanged subscriber in onQuestionChangedSubscribers) {
			subscriber.OnQuestionChanged (curQuestion);
		}
		questionTime = Time.time;
	}

	public void OnAnswer(string answer) {
		if (curQuestion == null || answer.Length == 0) {
			return;
		}
		bool isCorrect = curQuestion.IsAnswerCorrect (answer);
		float answerTime = Time.time - questionTime;
		HandleAnswer (isCorrect, answerTime);
		questions.Save ();
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

	void HandleAnswer (bool isCorrect, float answerTime)
	{
		bool wasNew = curQuestion.IsNew ();
		bool isNewlyMastered = curQuestion.UpdateState (isCorrect, answerTime);
		if (isCorrect) {
			foreach (OnCorrectAnswer subscriber in onCorrectAnswerSubscribers) {
				subscriber.OnCorrectAnswer (curQuestion, isNewlyMastered);
			}
		}
		else {
			foreach (OnWrongAnswer subscriber in onWrongAnswerSubscribers) {
				subscriber.OnWrongAnswer (wasNew);
			}
		}
	}

	void Test() {
		subscribers = new GameObject[1];
		subscribers[0] = gameObject;		
		SplitSubscribers ();

		float totalTime = 0;
		int session = 0;
		float maxSessionTime = 0;
		const float INITIAL_CHANCE_RIGHT = 0.6f;
		const float MIN_CHANCE_RIGHT = 0.1f;
		const float MAX_CHANCE_RIGHT = 0.9f;
		const float RIGHT_CHANCE_INCREASE_WHEN_ANSWER_RIGHT = 0.1f;
		const float RIGHT_CHANCE_DECREASE_PER_DAY = 0.05f;
		const float MIN_ANSWER_TIME = 5.0f;
		const float MAX_ANSWER_TIME = 60.0f;
		CCTime.SetNow(System.DateTime.UtcNow.Date.AddHours(7));
		while(questions.questions.Any(q => !q.IsMastered())) {
			questions.Reset();
			NextQuestion ();
			if (curQuestion != null) {
				++session;
				Debug.Log ("Session " + session);
				float sessionTime = 0;
				while (curQuestion != null) {
					bool right;
					float time = 0;
					if (curQuestion.IsNew()) {
						curQuestion.Debug_chanceOfCorrectAnswer = INITIAL_CHANCE_RIGHT;
					}
					curQuestion.Debug_chanceOfCorrectAnswer = Mathf.Clamp(curQuestion.Debug_chanceOfCorrectAnswer, MIN_CHANCE_RIGHT, MAX_CHANCE_RIGHT);
					do {
						right = Random.value <= curQuestion.Debug_chanceOfCorrectAnswer; 
						time += Random.Range (MIN_ANSWER_TIME, MAX_ANSWER_TIME);
						HandleAnswer (right, time);
					} while (!right);
					curQuestion.Debug_chanceOfCorrectAnswer += RIGHT_CHANCE_INCREASE_WHEN_ANSWER_RIGHT;
					CCTime.SetNow (CCTime.Now ().AddSeconds (time));
					sessionTime += time;
					if (sessionTime > maxSessionTime) {
						maxSessionTime = sessionTime;
					}
					NextQuestion ();
				}
				Debug.Log ("sessionTime = " + sessionTime);
				totalTime += sessionTime;
			}
			CCTime.SetNow( CCTime.Now().AddDays (1).Date.AddHours (7));
			foreach (Question question in questions.questions) {
				if (!question.IsNew ()) {
					question.Debug_chanceOfCorrectAnswer -= RIGHT_CHANCE_DECREASE_PER_DAY;
				}
			}
//			CCTime.SetNow( CCTime.Now().AddMinutes(10) );
		}
		Debug.Log ("sessions = " + session + " totalTime = " + Mathf.RoundToInt(totalTime) + " maxSessionTime = " + Mathf.RoundToInt(maxSessionTime));

	}

	/* Sample results:
	 * 	const float INITIAL_CHANCE_RIGHT = 0f;
		const float MIN_CHANCE_RIGHT = 0.1f;
		const float MAX_CHANCE_RIGHT = 0.9f;
		const float RIGHT_CHANCE_INCREASE_WHEN_ANSWER_RIGHT = 0.15f;
		const float RIGHT_CHANCE_DECREASE_PER_DAY = 0.05f;
		const float MIN_ANSWER_TIME = 5.0f;
		const float MAX_ANSWER_TIME = 60.0f;
	    	sessions = 66 - 80 totalTime = 42478 - 52241 maxSessionTime = 1492 - 3065
	    const float INITIAL_CHANCE_RIGHT = 0.2f;
	    	sessions = 53 - 62 totalTime = 28015 - 30388 maxSessionTime =  931 - 1583
	    const float INITIAL_CHANCE_RIGHT = 0.4f;
			sessions = 38 - 56 totalTime = 15839 - 21181 maxSessionTime = 576 - 923
		const float INITIAL_CHANCE_RIGHT = 0.6f;
			sessions = 30 - 32 totalTime = 11051 - 11943 maxSessionTime = 507 - 623
		const float INITIAL_CHANCE_RIGHT = 0.8f;
			sessions = 19 - 37 totalTime =  6892 -  7798 maxSessionTime = 482 - 549

		RIGHT_CHANCE_INCREASE_WHEN_ANSWER_RIGHT = 0.1f;
			sessions = 83 - 90 totalTime = 58322 - 65213 maxSessionTime = 1753 - 2432
	    const float INITIAL_CHANCE_RIGHT = 0.2f;
			sessions = 72 - 84 totalTime = 39335 - 46654 maxSessionTime =  968 - 1592
	    const float INITIAL_CHANCE_RIGHT = 0.4f;
			sessions = 49 - 58 totalTime = 22364 - 25897 maxSessionTime =  816 - 1130			
*/	    
}
