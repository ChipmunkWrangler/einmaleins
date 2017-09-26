using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class RocketParts {
	const string prefsKey = "rocketParts";
	const int PARTS_TO_BUILD_ROCKET = 15;
	const int PARTS_PER_UPGRADE = 10;

	public static void Inc() {
		UnityEngine.Assertions.Assert.IsTrue (FlashQuestions.ASK_LIST_LENGTH <= PARTS_TO_BUILD_ROCKET); // having enough rocket parts to run FlashQuestions should imply having enough questions
		SetNumParts (GetNumParts () + 1);
	}

	public static int GetNumParts() {
		return MDPrefs.GetInt (prefsKey, 0);
	}
		
	public static int GetNumPartsRequired() {
		return IsRocketBuilt () ? PARTS_PER_UPGRADE : PARTS_TO_BUILD_ROCKET;
	}

	public static bool HasEnoughPartsToUpgrade() {
		return IsRocketBuilt() && GetNumParts () >= PARTS_PER_UPGRADE && GetUpgradeLevel() < GetNumUpgrades();
	}

	public static bool HasReachedPlanetToUpgrade() {
		return GetUpgradeLevel () < TargetPlanet.GetIdx ();
	}

	public static bool IsRocketBuilt() {
		return MDPrefs.GetBool (prefsKey + ":isBuilt");
	}
		
	public static bool CanBuild() {
		return !IsRocketBuilt() && GetNumParts() >= PARTS_TO_BUILD_ROCKET;
	}

	public static void Build() {
		if (CanBuild()) {
			SetNumParts (GetNumParts () - PARTS_TO_BUILD_ROCKET);
			MDPrefs.SetBool (prefsKey + ":isBuilt", true);
		}
	}

	public static void Upgrade() {
		if (HasEnoughPartsToUpgrade () && HasReachedPlanetToUpgrade()) {
			SetNumParts (GetNumParts () - PARTS_PER_UPGRADE);
			MDPrefs.SetInt (prefsKey + ":upgradeLevel", GetUpgradeLevel () + 1);
		}
	}

	public static int GetNumUpgrades() {
		UnityEngine.Assertions.Assert.AreEqual ((Questions.GetNumQuestions() - PARTS_TO_BUILD_ROCKET) % PARTS_PER_UPGRADE, 0);
		return (Questions.GetNumQuestions() - PARTS_TO_BUILD_ROCKET) / PARTS_PER_UPGRADE;
	}

	public static int GetUpgradeLevel() {
		return MDPrefs.GetInt (prefsKey + ":upgradeLevel", 0);
	}

	static void SetNumParts(int newNum) {
		MDPrefs.SetInt (prefsKey, newNum);
	}
		
			
}
