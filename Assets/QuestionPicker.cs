using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionPicker : MonoBehaviour {
	private Questions questions;
	[SerializeField] QuestionDisplay display;
	private Question curQuestion;
	[SerializeField] UnityEngine.UI.Text placeholder;
	[SerializeField] float victoryCelebrationSecs;
	[SerializeField] ParticleSystem[] victoryParticles;

	// Use this for initialization
	void Start () {
		questions = new Questions();
		NextQuestion ();
	}

	void NextQuestion() {
		curQuestion = PickQuestion ();
		if (curQuestion == null) {
			display.DisplayQuestion ("You are done for now!");
		} else {
			display.DisplayQuestion (curQuestion.GetQuestionString ());
		}
	}

	Question PickQuestion() {
		return questions.GetNextQuestion ();
	}

	public void OnAnswer(UnityEngine.UI.InputField input) {
		if (curQuestion == null) {
			return;
		}	
		bool isCorrect = curQuestion.IsAnswerCorrect (input.text);
		curQuestion.UpdateInterval (isCorrect);
		if (isCorrect) {
			StartCoroutine (OnCorrectAnswer (input));
		} else {
			placeholder.text = "Versuche es noch einmal...";
			input.text = "";
			foreach (ParticleSystem particles in victoryParticles) {
				particles.Stop ();
			}
		}
	}

	private IEnumerator OnCorrectAnswer(UnityEngine.UI.InputField input) {
		input.readOnly = true;
		foreach (ParticleSystem particles in victoryParticles) {
			particles.Play ();
		}
		yield return new WaitForSeconds(victoryCelebrationSecs);
		foreach (ParticleSystem particles in victoryParticles) {
			particles.Stop ();
		}
		ResetInput (input);
		NextQuestion ();
	}

	private void ResetInput(UnityEngine.UI.InputField input) {
		input.text = "";
		placeholder.text = "ergibt...";
		input.readOnly = false;
	}

}
