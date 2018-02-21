using UnityEngine;

public class LaunchButtonController : MonoBehaviour {
    [SerializeField] UnityEngine.UI.Button LaunchButton = null;
    [SerializeField] UnityEngine.UI.Text LaunchButtonText = null;

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
		LaunchButton.enabled = false;
		LaunchButton.gameObject.SetActive(false);
	}

	public void ActivateLaunch() {
		LaunchButtonText.text = I2.Loc.LocalizationManager.GetTermTranslation (LaunchButtonLabels [TargetPlanet.GetTargetPlanetIdx ()]);
		LaunchButton.enabled = true;
		LaunchButton.gameObject.SetActive (true);
	}
}
