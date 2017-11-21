using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRocketColour : MonoBehaviour {
	[SerializeField] MeshRenderer rocketMesh = null;
	[SerializeField] ColorPickerTriangle colourPicker = null;
	bool isPaint;

	void Start() {
		colourPicker.gameObject.SetActive(false);
	}

	void Update()
	{
		if (isPaint)
		{
			rocketMesh.material.color = colourPicker.TheColor;
		}
	}

	public void StartPaint()
	{
		colourPicker.gameObject.SetActive(true);
		colourPicker.SetNewColor(rocketMesh.material.color);
		isPaint = true;
	}

	private void StopPaint()
	{
		isPaint = false;
	}
}
