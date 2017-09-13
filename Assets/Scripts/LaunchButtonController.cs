using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchButtonController : MonoBehaviour, OnQuestionChanged {
	[SerializeField] Questions questions = null;
	[SerializeField] UnityEngine.UI.Button launchButton = null;
	[SerializeField] UnityEngine.UI.Button upgradeButton = null;
	[SerializeField] UnityEngine.UI.Text upgradeButtonText = null;
	[SerializeField] UnityEngine.UI.Text doneText = null;
	[SerializeField] string buildRocketText = "";
	[SerializeField] string upgradeRocketText = "";

	public void OnQuestionChanged(Question question) {
		bool noMoreQuestions = question == null;
		ActivateIfCanLaunch (noMoreQuestions, CanLaunch (noMoreQuestions));
	}

	public bool CanLaunch(bool noMoreQuestions) {
		return noMoreQuestions && questions.GetNumMastered () >= FlashQuestions.ASK_LIST_LENGTH;
	}

	void ActivateIfCanLaunch (bool noMoreQuestions, bool canLaunch)
	{
		if (launchButton.gameObject.activeSelf != canLaunch) {
			launchButton.interactable = canLaunch;
			launchButton.gameObject.SetActive (canLaunch);
		}
		bool canUpgrade = RocketPartCounter.GetNumRocketParts () > 0;
		if (canUpgrade) {
			upgradeButtonText.text = canLaunch ? upgradeRocketText : buildRocketText;
		}
		upgradeButton.gameObject.SetActive (canUpgrade);
		doneText.gameObject.SetActive( noMoreQuestions && !canLaunch && !canUpgrade );

	}


}
