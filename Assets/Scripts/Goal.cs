using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour {
	[SerializeField] EffortTracker effortTracker;

	public enum CurGoal {
		UPGRADE_ROCKET,
		FLY_TO_PLANET,
		GAUNTLET,
		DONE_FOR_TODAY,
		WON // try to get high score
	}

	public CurGoal CalcCurGoal() {
		CurGoal curGoal;
		if (ShouldUpgrade()) {
			curGoal = CurGoal.UPGRADE_ROCKET;
		} else if (IsDoneForToday()) {
			curGoal = CurGoal.DONE_FOR_TODAY;
		} else if (IsLeavingSolarSystem()) {
			curGoal = CurGoal.WON;
		} else {
			curGoal = IsReadyForGauntlet() ? CurGoal.GAUNTLET : CurGoal.FLY_TO_PLANET;
		}

		return curGoal;
	}
		
	public static bool IsGivingUpAllowed(CurGoal curGoal) {
		return curGoal == Goal.CurGoal.FLY_TO_PLANET;
	}

	public static bool IsReadyForGauntlet() {
		return (TargetPlanet.GetTargetPlanetIdx () == TargetPlanet.GetMaxPlanetIdx ()) && 
			RocketParts.instance.upgradeLevel == RocketParts.instance.maxUpgradeLevel - 1;
	}

	static bool ShouldUpgrade() {
		return (RocketParts.instance.hasEnoughPartsToUpgrade || !ChooseRocketColour.HasChosenColour()) && !RocketParts.instance.justUpgraded;
	}

	bool IsDoneForToday() {
		return effortTracker.IsDoneForToday () && !RocketParts.instance.justUpgraded;
	}

	static bool IsLeavingSolarSystem() {
		return TargetPlanet.GetTargetPlanetIdx () > TargetPlanet.GetMaxPlanetIdx();
	}
}
