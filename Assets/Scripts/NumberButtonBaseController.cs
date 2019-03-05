using CrazyChipmunk;
using UnityEngine;

internal abstract class NumberButtonBaseController : MonoBehaviour
{
    [SerializeField] private float maxSmallScreenInches = 2.5F;
    [SerializeField] private Prefs prefs;

    protected string UserChosenLayout
    {
        get => prefs.GetString("buttonLayout", "");
        set => prefs.SetString("buttonLayout", value);
    }

    private void Start()
    {
        UpdateLayout();
    }

    protected virtual void UpdateLayout()
    {
        if (UserChosenLayout == "grid" || (UserChosenLayout == "" && IsSmallScreen()))
        {
            UseCompactButtonLayout();
        }
        else
        {
            UseNormalButtonLayout();
        }
    }

    private bool IsSmallScreen()
    {
        var dpi = Screen.dpi;
        if (dpi == 0)
        {
            return false;
        }

        return Screen.width / dpi <= maxSmallScreenInches;
    }

    protected abstract void UseNormalButtonLayout();

    protected abstract void UseCompactButtonLayout();
}