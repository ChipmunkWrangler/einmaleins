using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPartCounter : MonoBehaviour, OnNewlyMastered {
	[SerializeField] UnityEngine.UI.Text text = null;
	int rocketParts;
	const string prefsKey = "rocketParts";

	// Use this for initialization
	void Start () {
		rocketParts  = MDPrefs.GetInt (prefsKey, 0);
		UpdateText ();
	}

	public void OnNewlyMastered() {
		++rocketParts;
		MDPrefs.SetInt (prefsKey, rocketParts);
		UpdateText ();
	}

	void UpdateText () {
		text.text = rocketParts.ToString();
	}


}
