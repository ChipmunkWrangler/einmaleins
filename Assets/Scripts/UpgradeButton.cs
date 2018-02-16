using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Button button = null;
    [SerializeField] UnityEngine.UI.Text label = null;
    [SerializeField] string engineTermPrefix = "engineNames_";

    void Start()
    {
        if (RocketParts.instance.isRocketBuilt)
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
        button.gameObject.SetActive(false);
    }

    public void Hide()
    {
        button.gameObject.SetActive(false);
    }

    void Show()
    {
        label.text = I2.Loc.LocalizationManager.GetTermTranslation(engineTermPrefix + RocketParts.instance.upgradeLevel);
        button.gameObject.SetActive(true);
        button.enabled = true;
    }

}
