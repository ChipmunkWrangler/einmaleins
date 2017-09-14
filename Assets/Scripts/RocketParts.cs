using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class RocketParts {
	const string prefsKey = "rocketParts";
	const int PARTS_TO_BUILD_ROCKET = FlashQuestions.ASK_LIST_LENGTH;
	const int PARTS_PER_UPGRADE = 5;

	public static void Inc() {
		UnityEngine.Assertions.Assert.AreEqual ((Questions.maxNum - PARTS_TO_BUILD_ROCKET) % PARTS_PER_UPGRADE, 0);
		SetNumParts (GetNumParts () + 1);
	}

	public static int GetNumParts() {
		return MDPrefs.GetInt (prefsKey, 0);
	}
		
	public static int GetNumPartsRequired() {
		return IsRocketBuilt () ? PARTS_PER_UPGRADE : PARTS_TO_BUILD_ROCKET;
	}

	public static bool CanUpgrade() {
		return GetNumParts () >= PARTS_PER_UPGRADE;
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

	static void SetNumParts(int newNum) {
		MDPrefs.SetInt (prefsKey, newNum);
	}
		
			
}
