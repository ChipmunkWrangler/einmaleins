using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRocketColour : MonoBehaviour {
    [SerializeField] MeshRenderer RocketMesh = null;
    [SerializeField] ColorPickerCircle ColourPicker = null;
    bool IsPaint;
	public static readonly string PrefsKey = "rocketColour";

	void Start() {
		ColourPicker.gameObject.SetActive(false);
		ColourPicker.SetNewColor(RocketMesh.material.color);
	}

	void Update()
	{
		if (IsPaint)
		{
			RocketMesh.material.color = ColourPicker.TheColor;
		}
	}

	public static bool HasChosenColour() => MDPrefs.HasKey(PrefsKey + ".r");

	public void StartPaint()
	{
		ColourPicker.gameObject.SetActive(true);
		IsPaint = true;
	}

	public void StopPaint()
	{
		if (IsPaint) {
			MDPrefs.SetColor (PrefsKey, ColourPicker.TheColor);
		}
		IsPaint = false;
	}
}
