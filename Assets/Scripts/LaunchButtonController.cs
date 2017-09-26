using UnityEngine;
using UnityEngine.UI;

public class LaunchButtonController : MonoBehaviour, OnQuestionChanged {
	[SerializeField] Button launchButton = null;
	[SerializeField] Button upgradeButton = null;
	[SerializeField] Text upgradeButtonLabel = null;
	[SerializeField] Text doneText = null;
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
