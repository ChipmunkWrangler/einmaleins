using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Button Button = null;
    [SerializeField] UnityEngine.UI.Text Label = null;
    [SerializeField] string EngineTermPrefix = "engineNames_";

    void Start()
    {
        if (RocketParts.Instance.IsRocketBuilt)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void OnPressed()
    {
        Button.gameObject.SetActive(false);
    }

    public void Hide()
    {
        Button.gameObject.SetActive(false);
    }

    void Show()
    {
        Label.text = I2.Loc.LocalizationManager.GetTermTranslation(EngineTermPrefix + RocketParts.Instance.UpgradeLevel);
        Button.gameObject.SetActive(true);
        Button.enabled = true;
    }

}
