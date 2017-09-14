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
		pos.y = (RocketParts.IsRocketBuilt()) ? builtY : hiddenY;
		gameObject.transform.position = pos;
		if (!RocketParts.IsRocketBuilt () && RocketParts.CanBuild ()) {
			Build ();
		}
	}

	public void OnUpgrade() {
		UnityEngine.Assertions.Assert.AreEqual (exhaustParticles.Length, RocketParts.GetNumUpgrades () + 1);
		counter.OnSpend (RocketParts.GetNumParts (), RocketParts.GetNumParts () - RocketParts.GetNumPartsRequired());
		RocketParts.Upgrade ();
		StartEngine ();
	}

	void StartEngine ()
	{
		int upgradeLevel = RocketParts.GetUpgradeLevel ();
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
		exhaustParticles [RocketParts.GetUpgradeLevel ()].Stop ();
		button.OnDoneBuildOrUpgrade ();
	}

	void GotoBasePos(string onComplete) {
		iTween.MoveTo (gameObject, iTween.Hash ("y", builtY, "time", buildingTime, "delay", buildingDelay, "easetype", iTween.EaseType.easeOutQuad, "oncomplete", onComplete));
	}

	void Build ()
	{
		GotoBasePos("DoneBuilding");
		buildParticles.Play ();
		counter.OnSpend (RocketParts.GetNumParts (), RocketParts.GetNumParts () - RocketParts.GetNumPartsRequired());
	}

	void DoneBuilding() {
		RocketParts.Build ();
		buildParticles.Stop ();
		button.OnDoneBuildOrUpgrade ();
	}

}
