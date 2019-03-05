using CrazyChipmunk;
using UnityEngine;

internal class GetRocketColour : MonoBehaviour
{
    [SerializeField] private Prefs prefs;
    [SerializeField] private Renderer rocketRenderer;

    private void Start()
    {
        rocketRenderer.material.color = prefs.GetColor(RocketColour.PrefsKey, rocketRenderer.material.color);
    }
}