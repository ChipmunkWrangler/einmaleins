using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

internal class LaunchButtonController : MonoBehaviour
{
    private readonly string[] launchButtonLabels =
    {
        "Auf zum Mars",
        "Auf zum Jupiter",
        "Auf zum Saturn",
        "Auf zum Uranus",
        "Auf zum Neptun",
        "Auf zum Pluto",
        "Auf ins All"
    };

    [SerializeField] private Button launchButton;
    [SerializeField] private Text launchButtonText;
    [SerializeField] private TargetPlanet targetPlanet;

    public void Deactivate()
    {
        launchButton.enabled = false;
        launchButton.gameObject.SetActive(false);
    }

    public void ActivateLaunch()
    {
        launchButtonText.text =
            LocalizationManager.GetTermTranslation(launchButtonLabels[targetPlanet.GetTargetPlanetIdx()]);
        launchButton.enabled = true;
        launchButton.gameObject.SetActive(true);
    }
}