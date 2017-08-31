using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveOnAnswer : MonoBehaviour, OnCorrectAnswer, OnWrongAnswer {
	[SerializeField] UnityEngine.UI.Button button = null;

	public void OnCorrectAnswer (Question question) {
		button.interactable = false;
	}

	public void OnWrongAnswer () {
		button.interactable = false;
	}

	public void OnAnswerChanged() {
		button.interactable = true;
	}
}
