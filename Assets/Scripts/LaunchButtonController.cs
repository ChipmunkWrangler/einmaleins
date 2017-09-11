using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchButtonController : MonoBehaviour, OnQuestionChanged {
	[SerializeField] Questions questions;
	[SerializeField] UnityEngine.UI.Button button;
	[SerializeField] UnityEngine.UI.Text doneText;

	public void OnQuestionChanged(Question question) {
		bool noMoreQuestions = question == null;
		ActivateIfCanLaunch (noMoreQuestions, CanLaunch (noMoreQuestions));
	}

	public bool CanLaunch(bool noMoreQuestions) {
		return noMoreQuestions && questions.GetNumMastered () >= FlashQuestions.ASK_LIST_LENGTH;
	}

	void ActivateIfCanLaunch (bool noMoreQuestions, bool canLaunch)
	{
		if (button.gameObject.activeSelf != canLaunch) {
			button.interactable = canLaunch;
			button.gameObject.SetActive (canLaunch);
		}
		doneText.text = (noMoreQuestions && !canLaunch) ? "Fertig für Heute!" : "";

	}


}
