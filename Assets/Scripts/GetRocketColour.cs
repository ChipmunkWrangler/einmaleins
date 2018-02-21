using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRocketColour : MonoBehaviour {
    [SerializeField] Renderer RocketRenderer = null;

	void Start () {
		RocketRenderer.material.color = MDPrefs.GetColor (ChooseRocketColour.PrefsKey, RocketRenderer.material.color);
	}
}
