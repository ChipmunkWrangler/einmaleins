using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LaunchButtonController : MonoBehaviour, OnQuestionChanged {
	[SerializeField] Button launchButton = null;
	[SerializeField] Button upgradeButton = null;
	[SerializeField] Text launchButtonText = null;
	[SerializeField] Text upgradeButtonLabel = null;
	[SerializeField] Text reachPlanetToUpgradeLabel = null;
	[SerializeField] Text doneText = null;
	[SerializeField] string buildRocketText = "";
	[SerializeField] string upgradeRocketText = "";
	[SerializeField] Questions questions = null;
	const string prefsKey = "hasLaunchedOnDate";

	readonly string[] launchButtonLabels = {
		"Auf zum Mars",
		"Auf zum Jupiter",
		"Auf zum Saturn",
		"Auf zum Uranus",
		"Auf zum Neptun",
		"Auf zum Pluto",
		"Auf ins All"
	};
	readonly string[] reachPlanetLabels = {
		"Erreiche Mars, um Deine Rakete zu verbessern",
		"Erreiche Jupiter, um Deine Rakete zu verbessern",
		"Erreiche Saturn, um Deine Rakete zu verbessern",
		"Erreiche Uranus, um Deine Rakete zu verbessern",
		"Erreiche Neptun, um Deine Rakete zu verbessern",
		"",
		""
	};

	public void OnQuestionChanged(Question question) {
		bool noMoreQuestions = question == null;
		ActivateIfCanLaunch (noMoreQuestions);
	}

	public void OnLaunch() {
		MDPrefs.SetDateTime (prefsKey, System.DateTime.Today);
	}

	void ActivateIfCanLaunch (bool noMoreQuestions)
	{
		bool hasLaunchedToday = MDPrefs.GetDateTime (prefsKey, System.DateTime.MinValue) >= System.DateTime.Today;
		bool canLaunch = RocketParts.instance.isRocketBuilt && (!hasLaunchedToday || questions.HasMasteredAllQuestions() || questions.HasEnoughFlashQuestions());
		bool canBuild = RocketParts.instance.canBuild;
		bool canUpgrade = RocketParts.instance.hasEnoughPartsToUpgrade && RocketParts.instance.hasReachedPlanetToUpgrade;
		upgradeButtonLabel.text = canBuild ? buildRocketText : upgradeRocketText;
		upgradeButton.gameObject.SetActive (noMoreQuestions && (canBuild || canUpgrade));
		launchButtonText.text = launchButtonLabels [TargetPlanet.GetTargetPlanetIdx ()];
		launchButton.gameObject.SetActive (noMoreQuestions && canLaunch && !canUpgrade);
		reachPlanetToUpgradeLabel.text = reachPlanetLabels [TargetPlanet.GetTargetPlanetIdx ()];
		reachPlanetToUpgradeLabel.gameObject.SetActive (noMoreQuestions && canLaunch && (RocketParts.instance.hasEnoughPartsToUpgrade && !RocketParts.instance.hasReachedPlanetToUpgrade));
		doneText.gameObject.SetActive( noMoreQuestions && !canLaunch && !canUpgrade && !canBuild);
	}
}
