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
	readonly string[] orbitLaunchButtonLabels = {
		"Mars umrunden",
		"Jupiter umrunden",
		"Saturn umrunden",
		"Uranus umrunden",
		"Neptun umrunden",
		"Pluto umrunden"
	};

	public void Deactivate() {
		launchButton.SetActive(false);
	}

	public void ActivateLaunch() {
		ActivateAndSetText (launchButtonLabels);
	}

	public void ActivateOrbit() {
		ActivateAndSetText (orbitLaunchButtonLabels);
	}
		
	void ActivateAndSetText(string[] labels) {
		launchButtonText.text = labels [TargetPlanet.GetTargetPlanetIdx ()];
		launchButton.SetActive (true);
	}
}
