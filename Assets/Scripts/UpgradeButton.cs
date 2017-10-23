using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour {
	[SerializeField] UnityEngine.UI.Button button = null;
	[SerializeField] UnityEngine.UI.Text label = null;
	[SerializeField] string[] engineNames = null;

	void Start () {
		UnityEngine.Assertions.Assert.AreEqual (engineNames.Length, RocketParts.instance.numUpgrades);
		ShowOrHide (RocketParts.instance.isRocketBuilt);
	}

	public void OnPressed() {
		button.gameObject.SetActive (false);
	}

	public void OnDoneBuildOrUpgrade() {
		ShowOrHide (false);
	}

	void ShowOrHide(bool enable) {		
		label.text = engineNames [RocketParts.instance.upgradeLevel];
		button.gameObject.SetActive (enable);
		button.enabled = enable;
	}
}
