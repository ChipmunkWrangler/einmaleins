﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickoffLaunch : MonoBehaviour {
	[SerializeField] QuestionPicker questionPicker = null;
	[SerializeField] float delay = 1.0f;
	[SerializeField] int countdownTime = 3;
	[SerializeField] UnityEngine.UI.Text countdownText = null;
	[SerializeField] GameObject[] uiElementsToActivateOnLaunchButton;
	[SerializeField] GameObject[] uiElementsToDeactivateOnLaunchButton;
	[SerializeField] GameObject[] uiElementsToActivateOnCountdown;
	[SerializeField] GameObject[] uiElementsToDeactivateOnCountdown;
	[SerializeField] GameObject[] uiElementsToActivateOnPlay;
	[SerializeField] GameObject[] uiElementsToDeactivateOnPlay;
	[SerializeField] LaunchButtonController launchButtonController;
	[SerializeField] UnityEngine.UI.Text doneText;
	[SerializeField] string doneString;

	void Start () {
		if (MDPrefs.GetBool ("autolaunch")) {
			MDPrefs.SetBool ("autolaunch", false);
			Launch ();	
		} else {
			ShowLaunchButton ();
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

		launchButtonController.OnQuestionChanged (null);
		if (!launchButtonController.CanLaunch (true)) {
			doneText.text = doneString;
		}
	}

	IEnumerator Kickoff () {
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
		foreach (var element in uiElementsToActivateOnPlay) {
			element.SetActive (true);
		}
		foreach (var element in uiElementsToDeactivateOnPlay) {
			element.SetActive (false);
		}
		yield return null;
		questionPicker.NextQuestion ();
	}
}