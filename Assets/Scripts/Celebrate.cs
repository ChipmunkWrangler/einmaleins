using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celebrate : MonoBehaviour, OnCorrectAnswer, OnWrongAnswer, OnQuestionChanged, OnQuizAborted {
	public const float duration = 3.0F;
	[SerializeField] ParticleSystem[] exhaustParticles = null;
	[SerializeField] ParticleSystem fastAnswerParticles = null;
	[SerializeField] ParticleSystem masteryParticles = null;
	[SerializeField] ParticleSystem smokeParticles = null;
	[SerializeField] QuestionPicker questionPicker = null;
	[SerializeField] bool continueAfterQuestions = false;
	bool isCelebrating;
	Coroutine coroutine;

	public void OnQuizAborted() {
		StopTimer ();
		StopCelebrating ();
		StopSmoke ();
	}
		
	public void OnQuestionChanged(Question question) {
		StopTimer ();
		if (continueAfterQuestions && question == null) {
			StartCelebrating (false, false); // indefinitely
		} else {
			StopCelebrating ();
		}
	}

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		StopTimer ();
		if (question == null || !question.isLaunchCode) {
			float percentOn = (question == null) ? 1F : Mathf.Min (1F, FlashThrust.GetThrustFactor (question.GetLastAnswerTime ()));
			coroutine = StartCoroutine (DoCelebration (question != null && question.GetLastAnswerTime () <= Question.FAST_TIME, isNewlyMastered, percentOn));
		}
	}

	public void OnWrongAnswer(bool wasNew) {
		StopTimer ();
		StopCelebrating ();
	}
		
	IEnumerator DoCelebration(bool isFastAnswer, bool isNewlyMastered, float percentOn) {
		float exhaustTime = duration * percentOn;
		if (exhaustTime > 0) {
			StartCelebrating (isFastAnswer, isNewlyMastered);
			yield return new WaitForSeconds (exhaustTime);
			StopCelebrating ();
		}
		if (exhaustTime < duration) { 
			StartSmoke ();
			yield return new WaitForSeconds (duration - exhaustTime);
			StopSmoke ();
		}
		questionPicker.NextQuestion ();
	}

	void StartCelebrating(bool isFastAnswer, bool isNewlyMastered) {
		if (!isCelebrating) {
			isCelebrating = true;
			GetExhaustParticles ().gameObject.SetActive (true);
			GetExhaustParticles ().Play ();
			if (isNewlyMastered) {
				masteryParticles.Play ();
			}
			if (isFastAnswer) {
				fastAnswerParticles.Play ();
			}

		}
	}

	void StopCelebrating ()
	{
		if (isCelebrating) {
			GetExhaustParticles ().Stop ();
			masteryParticles.Stop ();
			fastAnswerParticles.Stop ();
			isCelebrating = false;
		}
	}

	void StartSmoke() {
		smokeParticles.Play ();
	}

	void StopSmoke() {
		smokeParticles.Stop ();
	}

	void StopTimer() {
		if (coroutine != null) {
			StopCoroutine (coroutine);
			coroutine = null;
		}
	}

	ParticleSystem GetExhaustParticles() => exhaustParticles [RocketParts.instance.upgradeLevel];
}
