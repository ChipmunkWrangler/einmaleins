using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celebrate : MonoBehaviour, OnCorrectAnswer, OnWrongAnswer, OnQuestionChanged {
	[SerializeField] float duration;
	[SerializeField] GameObject particleParent;
	[SerializeField] QuestionPicker questionPicker;
	ParticleSystem[] particles;
	bool isCelebrating;
	Coroutine coroutine;

	void Start() {
		particles = particleParent.GetComponentsInChildren<ParticleSystem> ();
	}
		
	public void OnQuestionChanged(Question question) {
		StopTimer ();
		if (question == null) { 
			StartCelebrating (); // indefinitely
		} else {
			StopCelebrating ();
		}
	}

	public void OnCorrectAnswer(Question question) {
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
			foreach (ParticleSystem particles in particles) {
				particles.Play ();
			}
		}
	}

	void StopCelebrating ()
	{
		if (isCelebrating) {
			foreach (ParticleSystem particles in particles) {
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
}
