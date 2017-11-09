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
			curGoal = (TargetPlanet.GetTargetPlanetIdx () == TargetPlanet.GetNumPlanets() - 1) ? CurGoal.GAUNTLET : CurGoal.FLY_TO_PLANET;
		}

		return curGoal;
	}
		
	bool ShouldUpgrade() {
		return RocketParts.instance.hasEnoughPartsToUpgrade && !RocketParts.instance.justUpgraded;
	}

	bool IsDoneForToday() {
		return effortTracker.IsDoneForToday () && !RocketParts.instance.justUpgraded;
	}

	bool IsLeavingSolarSystem() {
		return TargetPlanet.GetTargetPlanetIdx () >= TargetPlanet.GetNumPlanets();
	}
}
