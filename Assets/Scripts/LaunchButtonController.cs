using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchButtonController : MonoBehaviour, OnQuestionChanged {
	[SerializeField] UnityEngine.UI.Button launchButton = null;
	[SerializeField] UnityEngine.UI.Button upgradeButton = null;
	[SerializeField] UnityEngine.UI.Text upgradeButtonLabel = null;
	[SerializeField] UnityEngine.UI.Text doneText = null;
	[SerializeField] string buildRocketText = "";
	[SerializeField] string upgradeRocketText = "";

	public void OnQuestionChanged(Question question) {
		bool noMoreQuestions = question == null;
		ActivateIfCanLaunch (noMoreQuestions);
	}
		
	void ActivateIfCanLaunch (bool noMoreQuestions)
	{
		bool canLaunch = RocketParts.IsRocketBuilt();
		bool canBuild = RocketParts.CanBuild ();
		bool canUpgrade = RocketParts.CanUpgrade ();
		upgradeButtonLabel.text = canBuild ? buildRocketText : upgradeRocketText;
		upgradeButton.gameObject.SetActive (noMoreQuestions && (canBuild || canUpgrade));
		launchButton.gameObject.SetActive (noMoreQuestions && canLaunch);
		doneText.gameObject.SetActive( noMoreQuestions && !canLaunch && !canUpgrade && !canBuild);

	}


}
