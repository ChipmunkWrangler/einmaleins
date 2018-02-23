using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrazyChipmunk;

class ChooseRocketColour : MonoBehaviour
{
    public static readonly string PrefsKey = "rocketColour";

    [SerializeField] MeshRenderer rocketMesh = null;
    [SerializeField] ColorPickerCircle colourPicker = null;
    bool isPaint;

    public static bool HasChosenColour() => Prefs.HasKey(PrefsKey + ".r");

    public void StartPaint()
    {
        colourPicker.gameObject.SetActive(true);
        isPaint = true;
    }

    public void StopPaint()
    {
        if (isPaint)
        {
            Prefs.SetColor(PrefsKey, colourPicker.TheColor);
        }
        isPaint = false;
    }

    void Start()
    {
        colourPicker.gameObject.SetActive(false);
        colourPicker.SetNewColor(rocketMesh.material.color);
    }

    void Update()
    {
        if (isPaint)
        {
            rocketMesh.material.color = colourPicker.TheColor;
        }
    }
}
