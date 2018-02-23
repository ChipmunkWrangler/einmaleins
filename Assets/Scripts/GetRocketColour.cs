using CrazyChipmunk;
using UnityEngine;

class GetRocketColour : MonoBehaviour
{
    [SerializeField] Renderer rocketRenderer = null;

    void Start()
    {
        rocketRenderer.material.color = Prefs.GetColor(ChooseRocketColour.PrefsKey, rocketRenderer.material.color);
    }
}
