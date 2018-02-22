using UnityEngine;

public class LaunchButtonController : MonoBehaviour {
    [SerializeField] UnityEngine.UI.Button launchButton = null;
    [SerializeField] UnityEngine.UI.Text launchButtonText = null;

    readonly string[] LaunchButtonLabels = {
		"Auf zum Mars",
		"Auf zum Jupiter",
		"Auf zum Saturn",
		"Auf zum Uranus",
		"Auf zum Neptun",
		"Auf zum Pluto",
		"Auf ins All"
	};

	public void Deactivate() {
		launchButton.enabled = false;
		launchButton.gameObject.SetActive(false);
	}

	public void ActivateLaunch() {
		launchButtonText.text = I2.Loc.LocalizationManager.GetTermTranslation (LaunchButtonLabels [TargetPlanet.GetTargetPlanetIdx ()]);
		launchButton.enabled = true;
		launchButton.gameObject.SetActive (true);
	}
}
