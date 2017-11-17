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

	public CurGoal calcCurGoal() {
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
		
	bool IsReadyForGauntlet() {
		return (TargetPlanet.GetTargetPlanetIdx () == TargetPlanet.GetMaxPlanetIdx ()) && 
			RocketParts.instance.upgradeLevel == RocketParts.instance.maxUpgradeLevel;
	}

	bool ShouldUpgrade() {
		return RocketParts.instance.hasEnoughPartsToUpgrade && !RocketParts.instance.justUpgraded;
	}

	bool IsDoneForToday() {
		return (effortTracker.GetNumQuizzesLeftForToday () > 0) && !RocketParts.instance.justUpgraded;
	}

	bool IsLeavingSolarSystem() {
		return TargetPlanet.GetTargetPlanetIdx () > TargetPlanet.GetMaxPlanetIdx();
	}
}
