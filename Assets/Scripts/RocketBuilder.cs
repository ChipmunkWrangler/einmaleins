using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBuilder : MonoBehaviour {
    [SerializeField] float BuiltY = 0;
    [SerializeField] float HiddenY = -2.0F;
    [SerializeField] float MaxY = 1.0F;
    [SerializeField] float BuildingTime = 5.0F;
    [SerializeField] float UpgradeFlightTime = 5.0F;
    [SerializeField] float BuildingDelay = 1.0F;
    [SerializeField] RocketPartCounter Counter = null;
    [SerializeField] UpgradeButton UpgradeButton = null;
    [SerializeField] ParticleSystem BuildParticles = null;
    [SerializeField] ParticleSystem[] ExhaustParticles = null;
    [SerializeField] GameObject LaunchButton = null;
    [SerializeField] GameObject RocketPartsWidget = null;
    [SerializeField] GameObject ChooseColourButton = null;

    iTween.EaseType[] EaseTypes = { //new iTween.EaseType[]
		iTween.EaseType.linear,
		iTween.EaseType.easeInOutCubic,
		iTween.EaseType.easeOutQuad,
		iTween.EaseType.easeInOutQuint,
		iTween.EaseType.easeOutExpo,
		iTween.EaseType.linear,
		iTween.EaseType.easeOutExpo,
	};

	void Start () {
		ChooseColourButton.SetActive (false);
		if (RocketParts.Instance.IsRocketBuilt) {
			SetY (BuiltY);
			RocketPartsWidget.SetActive (RocketParts.Instance.UpgradeLevel < RocketParts.Instance.MaxUpgradeLevel - 1);
			if (!RocketParts.Instance.HasEnoughPartsToUpgrade) {
				DoneBuildingOrUpgrading ();
			}
		} else {
			SetY (HiddenY);
			RocketPartsWidget.SetActive (false);
			Build ();
		}
	}

	public void OnUpgrade() {
		UnityEngine.Assertions.Assert.AreEqual (ExhaustParticles.Length - 1, RocketParts.Instance.MaxUpgradeLevel);
		if (RocketParts.Instance.Upgrade ()) {
			Counter.OnSpend (RocketParts.Instance.NumParts + RocketParts.Instance.NumPartsRequired, RocketParts.Instance.NumParts);
			StartEngine ();
			iTween.MoveTo (gameObject, iTween.Hash ("y", MaxY, "time", UpgradeFlightTime, "delay", BuildingDelay, "easetype", EaseTypes[RocketParts.Instance.UpgradeLevel], "oncomplete", "Descend"));
		}
	}

	void StartEngine ()
	{
		for (int i = 0; i < ExhaustParticles.Length; ++i) {
			if (ExhaustParticles [i] != null) {
				ExhaustParticles [i].gameObject.SetActive (false);
			}
		}
		int upgradeLevel = RocketParts.Instance.UpgradeLevel;
		ExhaustParticles [upgradeLevel].gameObject.SetActive (true);
		ExhaustParticles [upgradeLevel].Play ();
	}

	void Descend() {
		GotoBasePos ("DoneUpgrading");
	}

	void DoneUpgrading() {
		DoneBuildingOrUpgrading ();
	}

	void GotoBasePos(string onComplete) {
		iTween.MoveTo (gameObject, iTween.Hash ("y", BuiltY, "time", BuildingTime, "delay", BuildingDelay, "easetype", iTween.EaseType.easeOutQuad, "oncomplete", onComplete));
	}

	void Build ()
	{
		GotoBasePos("DoneBuilding");
		BuildParticles.Play ();
		StartEngine ();
	}

	void DoneBuilding() {
		RocketParts.Instance.IsRocketBuilt = true;
		BuildParticles.Stop ();
		DoneBuildingOrUpgrading ();
	}

	void DoneBuildingOrUpgrading() {
		ExhaustParticles [RocketParts.Instance.UpgradeLevel].Stop ();
		UpgradeButton.Hide ();
		if (ChooseRocketColour.HasChosenColour ()) {
			LaunchButton.SetActive (true);
		} else {
			LaunchButton.SetActive (false);
			ChooseColourButton.SetActive (true);
		}
	}
		
	void SetY(float y) {
		Vector3 pos = gameObject.transform.position;
		pos.y = y;
		gameObject.transform.position = pos;
	}				
}
