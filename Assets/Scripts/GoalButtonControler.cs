using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GoalButtonControler : MonoBehaviour, OnQuestionChanged {
	[SerializeField] LaunchButtonController launchButton;
	[SerializeField] GameObject gotoMainButton = null;
	[SerializeField] GameObject upgradeButton = null;
	[SerializeField] Text upgradeButtonLabel = null;
	[SerializeField] GameObject doneText = null;
	[SerializeField] GameObject youWinText = null;
	[SerializeField] string buildRocketTerm = "Rakete bauen";
	[SerializeField] string upgradeRocketTerm = "Rakete verbessern";
	[SerializeField] Goal goal = null;

	public void OnQuestionChanged(Question question) {
		bool noMoreQuestions = question == null;
		UpdateButtonStatus (noMoreQuestions);
	}

	void UpdateButtonStatus (bool noMoreQuestions = true)
	{
		launchButton.Deactivate();
		upgradeButton.SetActive (false);
		youWinText.SetActive (false);
		doneText.SetActive(false);
		if (gotoMainButton != null) {
			gotoMainButton.SetActive (false); 
		}
		if (noMoreQuestions) {
			switch (goal.calcCurGoal()) {
			case Goal.CurGoal.COLLECT_PARTS:
				if (gotoMainButton != null) {
					gotoMainButton.SetActive (true);
				}
				break;
			case Goal.CurGoal.BUILD_ROCKET:
				ActivateBuildAndUpgradeButton (buildRocketTerm);
				break;
			case Goal.CurGoal.UPGRADE_ROCKET:
				ActivateBuildAndUpgradeButton (upgradeRocketTerm);
				break;
			case Goal.CurGoal.FLY_TO_PLANET:
			case Goal.CurGoal.GAUNTLET:
				launchButton.ActivateLaunch ();
				break;
			case Goal.CurGoal.DONE_FOR_TODAY:
				doneText.SetActive(true);
				break;
			case Goal.CurGoal.WON:
				launchButton.ActivateLaunch ();
				youWinText.SetActive (true);
				break;
			}
		}
	}


	void ActivateBuildAndUpgradeButton(string term) {
		upgradeButtonLabel.text = I2.Loc.LocalizationManager.GetTermTranslation( term );
		upgradeButton.SetActive (true);
	}
}
