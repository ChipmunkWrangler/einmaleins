using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBuilder : MonoBehaviour {
	[SerializeField] float builtY = 0;
	[SerializeField] float hiddenY = -2.0f;
	[SerializeField] float maxY = 1.0f;
	[SerializeField] float buildingTime = 5.0f;
	[SerializeField] float upgradeFlightTime = 5.0f;
	[SerializeField] float buildingDelay = 1.0f;
	[SerializeField] RocketPartCounter counter = null;
	[SerializeField] UpgradeButton button = null;
	[SerializeField] ParticleSystem buildParticles = null;
	[SerializeField] ParticleSystem[] exhaustParticles = null;
	iTween.EaseType[] easeTypes = new iTween.EaseType[] {
		iTween.EaseType.easeInOutCubic,
		iTween.EaseType.easeOutQuad,
		iTween.EaseType.easeInOutQuint,
		iTween.EaseType.easeOutExpo
	};

	void Start () {
		Vector3 pos = gameObject.transform.position;
		pos.y = (RocketParts.instance.isRocketBuilt) ? builtY : hiddenY;
		gameObject.transform.position = pos;
		if (!RocketParts.instance.isRocketBuilt&& RocketParts.instance.canBuild) {
			Build ();
		}
	}

	public void OnUpgrade() {
		UnityEngine.Assertions.Assert.AreEqual (exhaustParticles.Length, RocketParts.instance.numUpgrades + 1);
		if (RocketParts.instance.Upgrade ()) {
			Questions.OnUpgrade ();
			counter.OnSpend (RocketParts.instance.numParts + RocketParts.instance.numPartsRequired, RocketParts.instance.numParts);
			StartEngine ();
		}
	}

	void StartEngine ()
	{
		int upgradeLevel = RocketParts.instance.upgradeLevel;
		for (int i = 0; i < exhaustParticles.Length; ++i) {
			exhaustParticles [i].gameObject.SetActive (i == upgradeLevel);
		}
		exhaustParticles [upgradeLevel].Play ();
		iTween.MoveTo (gameObject, iTween.Hash ("y", maxY, "time", upgradeFlightTime, "delay", buildingDelay, "easetype", easeTypes[upgradeLevel-1], "oncomplete", "Descend"));
	}

	void Descend() {
		GotoBasePos ("DoneUpgrading");
	}

	void DoneUpgrading() {
		exhaustParticles [RocketParts.instance.upgradeLevel].Stop ();
		button.OnDoneBuildOrUpgrade ();
	}

	void GotoBasePos(string onComplete) {
		iTween.MoveTo (gameObject, iTween.Hash ("y", builtY, "time", buildingTime, "delay", buildingDelay, "easetype", iTween.EaseType.easeOutQuad, "oncomplete", onComplete));
	}

	void Build ()
	{
		GotoBasePos("DoneBuilding");
		buildParticles.Play ();
		counter.OnSpend (RocketParts.instance.numParts, RocketParts.instance.numParts - RocketParts.instance.numPartsRequired);
	}

	void DoneBuilding() {
		RocketParts.instance.Build ();
		buildParticles.Stop ();
		button.OnDoneBuildOrUpgrade ();
	}

}
