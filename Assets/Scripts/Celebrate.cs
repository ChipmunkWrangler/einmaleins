using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celebrate : MonoBehaviour, OnCorrectAnswer, OnWrongAnswer, OnQuestionChanged {
	[SerializeField] float duration;
	[SerializeField] GameObject particleParent;
	[SerializeField] QuestionPicker questionPicker;
	[SerializeField] bool continueAfterQuestions;
	ParticleSystem[] particles_;
	bool isCelebrating;
	Coroutine coroutine;
		
	public void OnQuestionChanged(Question question) {
		StopTimer ();
		if (continueAfterQuestions && question == null) {
			StartCelebrating (); // indefinitely
		} else {
			StopCelebrating ();
		}
	}

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		StopTimer ();
		coroutine = StartCoroutine (DoCelebration ());
	}

	public void OnWrongAnswer() {
		StopTimer ();
		StopCelebrating ();
	}
		
	IEnumerator DoCelebration() {
		StartCelebrating ();
		yield return new WaitForSeconds(duration);
		StopCelebrating ();
		questionPicker.NextQuestion ();
	}

	void StartCelebrating() {
		if (!isCelebrating) {
			isCelebrating = true;
			foreach (ParticleSystem particles in GetParticles()) {
				particles.Play ();
			}
		}
	}

	void StopCelebrating ()
	{
		if (isCelebrating) {
			foreach (ParticleSystem particles in GetParticles()) {
				particles.Stop ();
			}
			isCelebrating = false;
		}
	}

	void StopTimer() {
		if (coroutine != null) {
			StopCoroutine (coroutine);
		}
	}

	ParticleSystem[] GetParticles() {
		if (particles_ == null) {
			particles_ = particleParent.GetComponentsInChildren<ParticleSystem> ();
		}
		return particles_;
	}
}
