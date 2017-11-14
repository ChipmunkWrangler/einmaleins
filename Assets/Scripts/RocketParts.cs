using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketParts : MonoBehaviour {
// public readonly
	public static RocketParts instance { get; private set; }

	public bool justUpgraded;

	public int numParts {
		get {
			return numParts_;
		}
		private set {
			numParts_ = value;
			Save ();
		}
	}
	public bool isRocketBuilt {
		get {
			return isRocketBuilt_;
		}
		set {
			isRocketBuilt_ = value;
			Save ();
		}
	}

	public int upgradeLevel {
		get {
			return upgradeLevel_;
		}
		private set {
			upgradeLevel_ = value;
			Save ();
		}
	}

	public int numPartsRequired { 
		get {
			return PARTS_PER_UPGRADE;
		}
	}

	public bool hasEnoughPartsToUpgrade {
		get {
			return numParts >= PARTS_PER_UPGRADE && upgradeLevel < numUpgrades;
		}
	}
		
	public int numUpgrades {
		get {
			UnityEngine.Assertions.Assert.AreEqual ((Questions.GetNumQuestions () - PARTS_TO_BUILD_ROCKET) % PARTS_PER_UPGRADE, 0);
			return 1 + (Questions.GetNumQuestions () - PARTS_TO_BUILD_ROCKET) / PARTS_PER_UPGRADE;
		}
	}

// public commands
	public void FinalUpgrade() {
		UnityEngine.Assertions.Assert.IsTrue (upgradeLevel <= TargetPlanet.GetLastReachedIdx());
		UnityEngine.Assertions.Assert.AreEqual (upgradeLevel, numUpgrades-1);
		DoUpgrade ();
	}

	public bool Upgrade() {
		if (hasEnoughPartsToUpgrade) {
			numParts -= PARTS_PER_UPGRADE;
			DoUpgrade ();
			return true;
		}
		return false;
	}

	public void Inc() {
		++numParts;
	}

// private
	const string prefsKey = "rocketParts";
	const int PARTS_TO_BUILD_ROCKET = 11;
	const int PARTS_PER_UPGRADE = 11;

	int numParts_;
	bool isRocketBuilt_;
	int upgradeLevel_;

	void Save() {
		MDPrefs.SetBool (prefsKey + ":isBuilt", isRocketBuilt);
		MDPrefs.SetInt (prefsKey + ":upgradeLevel", upgradeLevel);
		MDPrefs.SetInt (prefsKey, numParts);
	}

	void Load ()
	{
		numParts_ = MDPrefs.GetInt (prefsKey, 0);
		isRocketBuilt_ = MDPrefs.GetBool (prefsKey + ":isBuilt");
		upgradeLevel_ = MDPrefs.GetInt (prefsKey + ":upgradeLevel", 0);
	}
		
	void Awake() {
		if (instance == null) {
//			DontDestroyOnLoad (gameObject);
			instance = this;
			Load ();
		} else if (instance != this) {
			Destroy (gameObject); // there can be only one!
		}
	}
			
	void DoUpgrade ()
	{
		TargetPlanet.TargetNextPlanet ();
		++upgradeLevel;
		justUpgraded = true;
	}
}
