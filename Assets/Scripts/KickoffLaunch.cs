﻿using System.Collections;
using UnityEngine;

public class KickoffLaunch : MonoBehaviour, OnCorrectAnswer {
	[SerializeField] Celebrate celebrate = null;
	[SerializeField] float delay = 1.0f;
	[SerializeField] int countdownTime = 3;
	[SerializeField] UnityEngine.UI.Text countdownText = null;
	[SerializeField] GameObject[] uiElementsToActivateOnLaunchCode;
	[SerializeField] GameObject[] uiElementsToDeactivateOnLaunchCode;
	[SerializeField] GameObject[] uiElementsToActivateOnLaunchButton;
	[SerializeField] GameObject[] uiElementsToDeactivateOnLaunchButton;
	[SerializeField] GameObject[] uiElementsToActivateOnCountdown;
	[SerializeField] GameObject[] uiElementsToDeactivateOnCountdown;
	[SerializeField] GameObject[] uiElementsToActivateOnPlay;
	[SerializeField] GameObject[] uiElementsToDeactivateOnPlay;
	[SerializeField] GoalButtonControler goalButtonController;
	[SerializeField] FlashThrust thrust;
	[SerializeField] Questions questions;
	[SerializeField] QuestionPicker questionPicker;

	void Start () {
		if (MDPrefs.GetBool ("autolaunch")) {
			MDPrefs.SetBool ("autolaunch", false);
			PreLaunch ();
		} else {
			ShowLaunchButton ();
		}
	}

	public void PreLaunch() {
		Question launchCodeQuestion = questions.GetGaveUpQuestion ();
		if (launchCodeQuestion == null) {
			Launch ();
		} else {
			RequestLaunchCode (launchCodeQuestion);
		}
	}

	public void Launch() {
		StartCoroutine (Kickoff ());
	}

	public void ShowLaunchButton() {
		foreach (var element in uiElementsToActivateOnLaunchButton) {
			element.SetActive (true);
		}
		foreach (var element in uiElementsToDeactivateOnLaunchButton) {
			element.SetActive (false);
		}

		goalButtonController.OnQuestionChanged (null);
	}

	IEnumerator Kickoff () {
		thrust.OnCountdown ();
		countdownText.text = "";
		countdownText.gameObject.SetActive (true);
		foreach (var element in uiElementsToActivateOnCountdown) {
			element.SetActive (true);
		}
		foreach (var element in uiElementsToDeactivateOnCountdown) {
			element.SetActive (false);
		}
		yield return new WaitForSeconds (delay);
		for (int i = countdownTime; i > 0; --i) {
			countdownText.text = i.ToString ();
			yield return new WaitForSeconds (1.0f);
		}
		countdownText.text = "";
		countdownText.gameObject.SetActive (false);
		foreach (var element in uiElementsToDeactivateOnPlay) {
			element.SetActive (false);
		}
		yield return null;
		thrust.Accelerate ();
		celebrate.OnCorrectAnswer (null, false); // this triggers the question once the flames are done
		yield return new WaitForSeconds(Celebrate.duration);
		foreach (var element in uiElementsToActivateOnPlay) {
			element.SetActive (true);
		}
	}

	void RequestLaunchCode(Question launchCodeQuestion) {
		foreach (var element in uiElementsToActivateOnLaunchCode) {
			element.SetActive (true);
		}
		foreach (var element in uiElementsToDeactivateOnLaunchCode) {
			element.SetActive (false);
		}
		questionPicker.ShowQuestion (launchCodeQuestion);
		launchCodeQuestion.isLaunchCode = true;
	}

	public void OnCorrectAnswer(Question question, bool isNewlyMastered ) {
		if (question.isLaunchCode) { 
			Launch ();
		}
	}
}
