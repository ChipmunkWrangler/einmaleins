using UnityEngine;

internal class NumberButtonSettingsController : NumberButtonBaseController
{
    [SerializeField] private GameObject normalParent;
    [SerializeField] private GameObject smallScreenParent;

    public void ToggleUserChosenLayout() // for the settings screen. Will not work on the launch screen!
    {
        UserChosenLayout = UserChosenLayout == "grid" ? "clock" : "grid";
        UpdateLayout();
    }

    protected override void UseNormalButtonLayout()
    {
        smallScreenParent.SetActive(false);
        normalParent.SetActive(true);
    }

    protected override void UseCompactButtonLayout()
    {
        normalParent.SetActive(false);
        smallScreenParent.SetActive(true);
    }
}