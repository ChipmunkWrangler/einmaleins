using CrazyChipmunk;
using UnityEngine;

class GetRocketColour : MonoBehaviour
{
    [SerializeField] Prefs prefs = null;
    [SerializeField] Renderer rocketRenderer = null;

    void Start()
    {
        rocketRenderer.material.color = prefs.GetColor(RocketColour.PrefsKey, rocketRenderer.material.color);
    }
}
