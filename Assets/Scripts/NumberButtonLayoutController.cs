using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberButtonLayoutController : MonoBehaviour {
	[SerializeField] Transform smallScreenParent = null;
	[SerializeField] float maxSmallScreenInches = 2.5f;
	[SerializeField] Transform multiplierStars = null;
	void Start () {
		if (IsSmallScreen()) {
			UseCompactButtonLayout ();
		}
	}

	bool IsSmallScreen() {
		float dpi = Screen.dpi;
		Debug.Log (dpi);
		if (dpi == 0) {
			return false;
		} 
		Debug.Log (Screen.width);
		Debug.Log (Screen.width / dpi);
		return Screen.width / dpi <= maxSmallScreenInches;
	}

	void UseCompactButtonLayout ()
	{
		while (transform.childCount > 0) {
			transform.GetChild (0).SetParent (smallScreenParent);
		}
		if (multiplierStars) {
			multiplierStars.localRotation = Quaternion.Euler (0, 0, -90.0f);
		}
	}
}
