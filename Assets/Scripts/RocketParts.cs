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
		MDPrefs.SetInt (prefsKey, GetNumRocketParts() + 1);
	}

	public static int GetNumRocketParts() {
		return MDPrefs.GetInt (prefsKey, 0);
	}

	public static bool CanUpgrade() {
		return GetNumRocketParts () > PARTS_PER_UPGRADE;
	}

	public static bool IsRocketBuilt() {
		return MDPrefs.GetBool (prefsKey + ":isBuilt");
	}
		
	public static bool CanBuild() {
		return !IsRocketBuilt() && GetNumRocketParts() > PARTS_TO_BUILD_ROCKET;
	}
}
