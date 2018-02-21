using System.Collections;
using UnityEngine;

public class KickoffLaunch : MonoBehaviour {
    [SerializeField] Celebrate Celebrator = null;
    [SerializeField] float Delay = 1.0F;
    [SerializeField] int CountdownTime = 3;
    [SerializeField] UnityEngine.UI.Text CountdownText = null;
    [SerializeField] GameObject[] UIElementsToActivateOnLaunchCode = null;
    [SerializeField] GameObject[] UIElementsToDeactivateOnLaunchCode = null;
    [SerializeField] GameObject[] UIElementsToActivateOnLaunchButton = null;
    [SerializeField] GameObject[] UIElementsToDeactivateOnLaunchButton = null;
    [SerializeField] GameObject[] UIElementsToActivateOnCountdown = null;
    [SerializeField] GameObject[] UIElementsToDeactivateOnCountdown = null;
    [SerializeField] GameObject[] UIElementsToActivateOnPlay = null;
    [SerializeField] GameObject[] UIElementsToDeactivateOnPlay = null;
    [SerializeField] GameObject[] UIElementsToDeactivateIfGivingUpIsForbidden = null;
    [SerializeField] GoalButtonControler GoalButtonController = null;
    [SerializeField] FlashThrust Thrust = null;
    [SerializeField] Questions QuestionContainer = null;
    [SerializeField] QuestionPicker QPicker = null;
    [SerializeField] Goal GoalStatus = null;

	void Start () {
		if (MDPrefs.GetBool ("autolaunch")) {
			MDPrefs.SetBool ("autolaunch", false);
			PreLaunch ();
		} else {
			ShowLaunchButton ();
		}
	}

	public void PreLaunch() {
		Question launchCodeQuestion = QuestionContainer.GetGaveUpQuestion ();
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
		foreach (var element in UIElementsToActivateOnLaunchButton) {
			element.SetActive (true);
		}
		foreach (var element in UIElementsToDeactivateOnLaunchButton) {
			element.SetActive (false);
		}

		GoalButtonController.OnQuestionChanged (null);
	}

	IEnumerator Kickoff () {
		Thrust.OnCountdown ();
		CountdownText.text = "";
		CountdownText.gameObject.SetActive (true);
		foreach (var element in UIElementsToActivateOnCountdown) {
			element.SetActive (true);
		}
		foreach (var element in UIElementsToDeactivateOnCountdown) {
			element.SetActive (false);
		}
		yield return new WaitForSeconds (Delay);
		for (int i = CountdownTime; i > 0; --i) {
			CountdownText.text = i.ToString ();
			yield return new WaitForSeconds (1.0F);
		}
		CountdownText.text = "";
		CountdownText.gameObject.SetActive (false);
		foreach (var element in UIElementsToDeactivateOnPlay) {
			element.SetActive (false);
		}
		yield return null;
		Thrust.Accelerate ();
		Celebrator.OnCorrectAnswer (null, false); // this triggers the question once the flames are done
		yield return new WaitForSeconds(Celebrate.Duration);
		foreach (var element in UIElementsToActivateOnPlay) {
			element.SetActive (true);
		}
		if (!Goal.IsGivingUpAllowed(GoalStatus.CalcCurGoal())) {
			foreach (var element in UIElementsToDeactivateIfGivingUpIsForbidden) {
				element.SetActive (false);
			}
		}
	}

	void RequestLaunchCode(Question launchCodeQuestion) {
		foreach (var element in UIElementsToActivateOnLaunchCode) {
			element.SetActive (true);
		}
		foreach (var element in UIElementsToDeactivateOnLaunchCode) {
			element.SetActive (false);
		}
		QPicker.ShowQuestion (launchCodeQuestion);
		launchCodeQuestion.IsLaunchCode = true;
	}

	public void OnCorrectAnswer(Question question, bool isNewlyMastered ) {
		if (question.IsLaunchCode) { 
			Launch ();
		}
	}
}
