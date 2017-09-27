using UnityEngine;
using UnityEngine.UI;

public class LaunchButtonController : MonoBehaviour, OnQuestionChanged {
	[SerializeField] Button launchButton = null;
	[SerializeField] Button upgradeButton = null;
	[SerializeField] Text launchButtonText = null;
	[SerializeField] Text upgradeButtonLabel = null;
	[SerializeField] Text reachPlanetToUpgradeLabel = null;
	[SerializeField] Text doneText = null;
	[SerializeField] string buildRocketText = "";
	[SerializeField] string upgradeRocketText = "";
	readonly string[] launchButtonLabels = {
		"Auf zum Mars",
		"Auf zum Jupiter",
		"Auf zum Saturn",
		"Auf zum Uranus",
		"Auf zum Neptun",
		""
	};
	readonly string[] reachPlanetLabels = {
		"Erreiche Mars, um Deine Rakete zu verbessern",
		"Erreiche Jupiter, um Deine Rakete zu verbessern",
		"Erreiche Saturn, um Deine Rakete zu verbessern",
		"Erreiche Uranus, um Deine Rakete zu verbessern",
		"Erreiche Neptun, um Deine Rakete zu verbessern",
		""
	};

	public void OnQuestionChanged(Question question) {
		bool noMoreQuestions = question == null;
		ActivateIfCanLaunch (noMoreQuestions);
	}
		
	void ActivateIfCanLaunch (bool noMoreQuestions)
	{
		bool canLaunch = RocketParts.IsRocketBuilt();
		bool canBuild = RocketParts.CanBuild ();
		bool canUpgrade = RocketParts.HasEnoughPartsToUpgrade () && RocketParts.HasReachedPlanetToUpgrade();
		upgradeButtonLabel.text = canBuild ? buildRocketText : upgradeRocketText;
		upgradeButton.gameObject.SetActive (noMoreQuestions && (canBuild || canUpgrade));
		launchButtonText.text = launchButtonLabels [TargetPlanet.GetIdx ()];
		launchButton.gameObject.SetActive (noMoreQuestions && canLaunch && !canUpgrade);
		reachPlanetToUpgradeLabel.text = reachPlanetLabels [TargetPlanet.GetIdx ()];
		reachPlanetToUpgradeLabel.gameObject.SetActive (noMoreQuestions && canLaunch && (RocketParts.HasEnoughPartsToUpgrade () && !RocketParts.HasReachedPlanetToUpgrade()));
		doneText.gameObject.SetActive( noMoreQuestions && !canLaunch && !canUpgrade && !canBuild);

	}


}
