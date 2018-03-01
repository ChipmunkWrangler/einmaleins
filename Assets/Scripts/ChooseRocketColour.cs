using CrazyChipmunk;
using UnityEngine;

class ChooseRocketColour : MonoBehaviour
{
    [SerializeField] MeshRenderer rocketMesh = null;
    [SerializeField] ColorPickerCircle colourPicker = null;
    [SerializeField] RocketColour data = null;
    bool isPaint;

    public void StartPaint()
    {
        colourPicker.gameObject.SetActive(true);
        isPaint = true;
    }

    public void StopPaint()
    {
        if (isPaint)
        {
            data.SetColour(colourPicker.TheColor);
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
