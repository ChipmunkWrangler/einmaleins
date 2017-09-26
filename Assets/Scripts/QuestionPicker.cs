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
		SplitSubscribers ();
		Test ();
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
		float totalTime = 0;
		int day = 0;
		float maxDayTime = 0;
		const float CHANCE_WRONG = 0.25f;
		const float MIN_ANSWER_TIME = 5.0f;
		const float MAX_ANSWER_TIME = 60.0f;
		CCTime.SetNow(System.DateTime.UtcNow.Date.AddHours(7));
		while(true) {
			++day;
			Debug.Log ("Day " + day);
			questions.Reset();
			NextQuestion ();
			if (curQuestion == null) {
				break;
			}
			float dayTime = 0;
			while (curQuestion != null) {
				bool right;
				float time = 0;
				do {
					float chance = Mathf.Clamp01((float)curQuestion.difficulty / Question.NEW_CARD_DIFFICULTY);
					right = Random.value > CHANCE_WRONG * chance; 
					float answerTime = Random.Range (MIN_ANSWER_TIME, MAX_ANSWER_TIME);
					time += answerTime;
					CCTime.SetNow( CCTime.Now().AddSeconds(answerTime) );
					HandleAnswer (right, time);
				} while (!right);
				dayTime += time;
				if (dayTime > maxDayTime) {
					maxDayTime = dayTime;
				}
				NextQuestion();
			}
			Debug.Log ("dayTime = " + dayTime);
			totalTime += dayTime;
			CCTime.SetNow( CCTime.Now().AddDays (1).Date.AddHours (7));
		}
		Debug.Log ("totalTime = " + totalTime + " maxDay = " + maxDayTime);
		UnityEngine.Assertions.Assert.IsFalse(questions.questions.Any(q => !q.IsMastered()));
	}
}
