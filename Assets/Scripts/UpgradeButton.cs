using UnityEngine;

class UpgradeButton : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Button button = null;
    [SerializeField] UnityEngine.UI.Text label = null;
    [SerializeField] string engineTermPrefix = "engineNames_";

    public void OnPressed()
    {
        button.gameObject.SetActive(false);
    }

    public void Hide()
    {
        button.gameObject.SetActive(false);
    }

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

    void Show()
    {
        label.text = I2.Loc.LocalizationManager.GetTermTranslation(engineTermPrefix + RocketParts.Instance.UpgradeLevel);
        button.gameObject.SetActive(true);
        button.enabled = true;
    }
}
