using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberButtonLayoutController : MonoBehaviour {
    [SerializeField] Transform SmallScreenParent = null;
    [SerializeField] float MaxSmallScreenInches = 2.5F;
    [SerializeField] Transform MultiplierStars = null;
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
		return Screen.width / dpi <= MaxSmallScreenInches;
	}

	void UseCompactButtonLayout ()
	{
		while (transform.childCount > 0) {
			transform.GetChild (0).SetParent (SmallScreenParent);
		}
		if (MultiplierStars) {
			MultiplierStars.localRotation = Quaternion.Euler (0, 0, -90.0F);
		}
	}
}
