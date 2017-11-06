using UnityEngine;

public class LaunchButtonController : MonoBehaviour {
	[SerializeField] GameObject launchButton = null;
	[SerializeField] UnityEngine.UI.Text launchButtonText = null;

	readonly string[] launchButtonLabels = {
		"Auf zum Mars",
		"Auf zum Jupiter",
		"Auf zum Saturn",
		"Auf zum Uranus",
		"Auf zum Neptun",
		"Auf zum Pluto",
		"Auf ins All"
	};

	public void Deactivate() {
		launchButton.SetActive(false);
	}

	public void ActivateLaunch() {
		launchButtonText.text = I2.Loc.LocalizationManager.GetTermTranslation (launchButtonLabels [TargetPlanet.GetTargetPlanetIdx ()]);
		launchButton.SetActive (true);
	}
}
