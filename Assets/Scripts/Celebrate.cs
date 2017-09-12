using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celebrate : MonoBehaviour, OnCorrectAnswer, OnWrongAnswer, OnQuestionChanged {
	[SerializeField] float duration;
	[SerializeField] GameObject particleParent;
	[SerializeField] GameObject masteryParticleParent;
	[SerializeField] QuestionPicker questionPicker;
	[SerializeField] bool continueAfterQuestions;
	ParticleSystem[] particles_;
	ParticleSystem[] masteryParticles_;
	bool isCelebrating;
	Coroutine coroutine;
		
	public void OnQuestionChanged(Question question) {
		StopTimer ();
		if (continueAfterQuestions && question == null) {
			StartCelebrating (false); // indefinitely
		} else {
			StopCelebrating ();
		}
	}

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		StopTimer ();
		coroutine = StartCoroutine (DoCelebration (isNewlyMastered));
	}

	public void OnWrongAnswer() {
		StopTimer ();
		StopCelebrating ();
	}
		
	IEnumerator DoCelebration(bool superSize) {
		StartCelebrating (superSize);
		yield return new WaitForSeconds(duration);
		StopCelebrating ();
		questionPicker.NextQuestion ();
	}

	void StartCelebrating(bool isNewlyMastered) {
		if (!isCelebrating) {
			isCelebrating = true;
			foreach (ParticleSystem particles in GetParticles()) {
				particles.Play ();
			}
			if (isNewlyMastered) {
				foreach (ParticleSystem particles in GetMasteryParticles()) {
					particles.Play ();
				}
			}
		}
	}

	void StopCelebrating ()
	{
		if (isCelebrating) {
			foreach (ParticleSystem particles in GetParticles()) {
				particles.Stop ();
			}
			foreach (ParticleSystem particles in GetMasteryParticles()) {
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

	ParticleSystem[] GetMasteryParticles() {
		if (masteryParticles_ == null) {
			masteryParticles_ = masteryParticleParent.GetComponentsInChildren<ParticleSystem> ();
		}
		return masteryParticles_;
	}

}
