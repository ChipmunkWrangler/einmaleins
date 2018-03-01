using CrazyChipmunk;
using UnityEngine;

[CreateAssetMenu(menuName = "TimesTables/RocketColour")]
class RocketColour : ScriptableObject
{
    [SerializeField] Prefs prefs = null;
    public static readonly string PrefsKey = "rocketColour";

    public bool HasChosenColour() => prefs.HasKey(PrefsKey + ".r");
    public void SetColour(Color c)
    {
        prefs.SetColor(PrefsKey, c);
    }
}

