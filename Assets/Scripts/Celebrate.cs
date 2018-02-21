using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celebrate : MonoBehaviour, IOnWrongAnswer, IOnQuestionChanged, IOnQuizAborted {
	public static readonly float Duration = 3.0F;
    [SerializeField] ParticleSystem[] ExhaustParticles = null;
    [SerializeField] ParticleSystem FastAnswerParticles = null;
    [SerializeField] ParticleSystem MasteryParticles = null;
    [SerializeField] ParticleSystem SmokeParticles = null;
    [SerializeField] QuestionPicker QPicker = null;
    [SerializeField] bool ContinueAfterQuestions = false;
    bool IsCelebrating;
    Coroutine Co;

	public void OnQuizAborted() {
		StopTimer ();
		StopCelebrating ();
		StopSmoke ();
	}
		
	public void OnQuestionChanged(Question question) {
		StopTimer ();
		if (ContinueAfterQuestions && question == null) {
			StartCelebrating (false, false); // indefinitely
		} else {
			StopCelebrating ();
		}
	}

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		StopTimer ();
		if (question == null || !question.IsLaunchCode) {
			float percentOn = (question == null) ? 1F : Mathf.Min (1F, FlashThrust.GetThrustFactor (question.GetLastAnswerTime ()));
			Co = StartCoroutine (DoCelebration (question != null && question.GetLastAnswerTime () <= Question.FastTime, isNewlyMastered, percentOn));
		}
	}

	public void OnWrongAnswer(bool wasNew) {
		StopTimer ();
		StopCelebrating ();
	}
		
	IEnumerator DoCelebration(bool isFastAnswer, bool isNewlyMastered, float percentOn) {
		float exhaustTime = Duration * percentOn;
		if (exhaustTime > 0) {
			StartCelebrating (isFastAnswer, isNewlyMastered);
			yield return new WaitForSeconds (exhaustTime);
			StopCelebrating ();
		}
		if (exhaustTime < Duration) { 
			StartSmoke ();
			yield return new WaitForSeconds (Duration - exhaustTime);
			StopSmoke ();
		}
		QPicker.NextQuestion ();
	}

	void StartCelebrating(bool isFastAnswer, bool isNewlyMastered) {
		if (!IsCelebrating) {
			IsCelebrating = true;
			GetExhaustParticles ().gameObject.SetActive (true);
			GetExhaustParticles ().Play ();
			if (isNewlyMastered) {
				MasteryParticles.Play ();
			}
			if (isFastAnswer) {
				FastAnswerParticles.Play ();
			}

		}
	}

	void StopCelebrating ()
	{
		if (IsCelebrating) {
			GetExhaustParticles ().Stop ();
			MasteryParticles.Stop ();
			FastAnswerParticles.Stop ();
			IsCelebrating = false;
		}
	}

	void StartSmoke() {
		SmokeParticles.Play ();
	}

	void StopSmoke() {
		SmokeParticles.Stop ();
	}

	void StopTimer() {
		if (Co != null) {
			StopCoroutine (Co);
			Co = null;
		}
	}

	ParticleSystem GetExhaustParticles() => ExhaustParticles [RocketParts.Instance.UpgradeLevel];
}
