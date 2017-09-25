using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveOnAnswer : MonoBehaviour, OnCorrectAnswer, OnWrongAnswer, OnAnswerChanged {
	[SerializeField] UnityEngine.UI.Button button = null;

	public void OnCorrectAnswer (Question question, bool isNewlyMastered) {
		button.interactable = false;
	}

	public void OnWrongAnswer (bool wasNew) {
		button.interactable = false;
	}

	public void OnAnswerChanged(bool isAnswerEmpty) {
		button.interactable = !isAnswerEmpty;
	}
}
