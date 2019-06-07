using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

internal class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Selectable button;
    [SerializeField] private string engineTermPrefix = "engineNames_";
    [SerializeField] private Text label;

    public void OnPressed()
    {
        button.gameObject.SetActive(false);
    }

    public void Hide()
    {
        button.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (RocketParts.Instance.IsRocketBuilt)
            Show();
        else
            Hide();
    }

    private void Show()
    {
        label.text = LocalizationManager.GetTermTranslation(engineTermPrefix + RocketParts.Instance.UpgradeLevel);
        button.gameObject.SetActive(true);
        button.enabled = true;
    }
}