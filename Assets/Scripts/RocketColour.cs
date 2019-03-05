using CrazyChipmunk;
using UnityEngine;

[CreateAssetMenu(menuName = "TimesTables/RocketColour")]
internal class RocketColour : ScriptableObject
{
    public static readonly string PrefsKey = "rocketColour";
    [SerializeField] private Prefs prefs;

    public bool HasChosenColour()
    {
        return prefs.HasKey(PrefsKey + ".r");
    }

    public void SetColour(Color c)
    {
        prefs.SetColor(PrefsKey, c);
    }
}