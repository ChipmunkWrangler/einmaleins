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
		if (RocketParts.instance.hasEnoughPartsToUpgrade && !RocketParts.instance.justUpgraded) {
			curGoal = CurGoal.UPGRADE_ROCKET;
		} else if (effortTracker.IsDoneForToday() && !RocketParts.instance.justUpgraded) {
			curGoal = CurGoal.DONE_FOR_TODAY;
		} else if (TargetPlanet.IsLeavingSolarSystem()) {
			curGoal = CurGoal.WON;
		} else {
			curGoal = (TargetPlanet.GetTargetPlanetIdx () == TargetPlanet.GetNumPlanets() - 1) ? CurGoal.GAUNTLET : CurGoal.FLY_TO_PLANET;
		}

		return curGoal;
	}
}
