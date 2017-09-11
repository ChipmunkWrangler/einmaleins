using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickoffLaunch : MonoBehaviour {
	[SerializeField] QuestionPicker questionPicker = null;
	[SerializeField] float delay = 1.0f;
	[SerializeField] int countdownTime = 3;
	[SerializeField] UnityEngine.UI.Text countdownText = null;
	[SerializeField] GameObject[] uiElementsToActivate;
	[SerializeField] GameObject[] uiElementsToDeactivate;
	[SerializeField] LaunchButtonController launchButtonController;

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

	void ShowLaunchButton() {
		countdownText.text = "";
		foreach (var element in uiElementsToActivate) {
			element.SetActive (false);
		}
		foreach (var element in uiElementsToDeactivate) {
			element.SetActive (true);
		}

		launchButtonController.ActivateIfCanLaunch (true, true);
	}

	IEnumerator Kickoff () {
		countdownText.text = "";
		countdownText.gameObject.SetActive (true);
		foreach (var element in uiElementsToDeactivate) {
			element.SetActive (false);
		}
		foreach (var element in uiElementsToActivate) {
			element.SetActive (false);
		}
		yield return new WaitForSeconds (delay);
		for (int i = countdownTime; i > 0; --i) {
			countdownText.text = i.ToString ();
			yield return new WaitForSeconds (1.0f);
		}
		countdownText.text = "";
		countdownText.gameObject.SetActive (false);
		foreach (var element in uiElementsToActivate) {
			element.SetActive (true);
		}
		yield return null;
		questionPicker.NextQuestion ();
	}
}
