using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Celebrate : MonoBehaviour, OnCorrectAnswer, OnWrongAnswer, OnQuestionChanged {
	[SerializeField] float duration = 3.0f;
	[SerializeField] ParticleSystem[] particles = null;
	[SerializeField] ParticleSystem masteryParticles;
	[SerializeField] QuestionPicker questionPicker = null;
	[SerializeField] bool continueAfterQuestions = false;
	bool isCelebrating;
	Coroutine coroutine;
	public int curParticleIdx { private get; set; }
		
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
			GetParticles ().gameObject.SetActive (true);
			GetParticles ().Play ();
			if (isNewlyMastered && GetMasteryParticles()) {
				GetMasteryParticles().Play ();
			}
		}
	}

	void StopCelebrating ()
	{
		if (isCelebrating) {
			GetParticles ().Stop ();
			if (GetMasteryParticles ()) {
				GetMasteryParticles ().Stop ();
			}
			isCelebrating = false;
		}
	}

	void StopTimer() {
		if (coroutine != null) {
			StopCoroutine (coroutine);
		}
	}

	ParticleSystem GetParticles() {
		return particles [curParticleIdx];
	}

	ParticleSystem GetMasteryParticles() {
		return masteryParticles;
	}

}
