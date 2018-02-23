using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class GetRocketColour : MonoBehaviour
{
    [SerializeField] Renderer rocketRenderer = null;

    void Start()
    {
        rocketRenderer.material.color = MDPrefs.GetColor(ChooseRocketColour.PrefsKey, rocketRenderer.material.color);
    }
}
