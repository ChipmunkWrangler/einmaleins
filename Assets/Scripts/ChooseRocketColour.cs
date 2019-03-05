using UnityEngine;

internal class ChooseRocketColour : MonoBehaviour
{
    [SerializeField] private ColorPickerCircle colourPicker;
    [SerializeField] private RocketColour data;
    private bool isPaint;
    [SerializeField] private MeshRenderer rocketMesh;

    public void StartPaint()
    {
        colourPicker.gameObject.SetActive(true);
        isPaint = true;
    }

    public void StopPaint()
    {
        if (isPaint) data.SetColour(colourPicker.TheColor);
        isPaint = false;
    }

    private void Start()
    {
        colourPicker.gameObject.SetActive(false);
        colourPicker.SetNewColor(rocketMesh.material.color);
    }

    private void Update()
    {
        if (isPaint) rocketMesh.material.color = colourPicker.TheColor;
    }
}