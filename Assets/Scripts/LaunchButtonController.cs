﻿using UnityEngine;

public class LaunchButtonController : MonoBehaviour
{
    readonly string[] launchButtonLabels = 
    {
        "Auf zum Mars",
        "Auf zum Jupiter",
        "Auf zum Saturn",
        "Auf zum Uranus",
        "Auf zum Neptun",
        "Auf zum Pluto",
        "Auf ins All"
    };

    [SerializeField] UnityEngine.UI.Button launchButton = null;
    [SerializeField] UnityEngine.UI.Text launchButtonText = null;

    public void Deactivate()
    {
        launchButton.enabled = false;
        launchButton.gameObject.SetActive(false);
    }

    public void ActivateLaunch()
    {
        launchButtonText.text = I2.Loc.LocalizationManager.GetTermTranslation(launchButtonLabels[TargetPlanet.GetTargetPlanetIdx()]);
        launchButton.enabled = true;
        launchButton.gameObject.SetActive(true);
    }
}
