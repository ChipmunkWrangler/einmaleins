using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchButtonController : MonoBehaviour, OnCorrectAnswer {
	[SerializeField] SlowQuestions questions;
	[SerializeField] UnityEngine.UI.Button button;

	void Start() {
		ActivateIfEnoughMastered ();
	}

	public void OnCorrectAnswer(Question question) {
		ActivateIfEnoughMastered ();
	}

	void ActivateIfEnoughMastered ()
	{
		button.interactable = questions.GetNumMastered () >= FlashQuestions.ASK_LIST_LENGTH;
	}
}
