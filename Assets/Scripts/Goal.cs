using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour {
	[SerializeField] Questions questions;

	public enum CurGoal {
		COLLECT_PARTS,
		BUILD_ROCKET,
		UPGRADE_ROCKET,
		FLY_TO_PLANET,
		GAUNTLET,
		DONE_FOR_TODAY,
		WON // try to get high score
	}

	public CurGoal calcCurGoal() {
		CurGoal curGoal;
		if (RocketParts.instance.isRocketBuilt) {
			int targetPlanetIdx = TargetPlanet.GetTargetPlanetIdx ();
			if (targetPlanetIdx == TargetPlanet.GetLastReachedIdx ()) {
				curGoal = RocketParts.instance.hasEnoughPartsToUpgrade ? CurGoal.UPGRADE_ROCKET : CurGoal.COLLECT_PARTS;
			} else if (questions.effortTracker.HasMaxedEffort ()) {
				curGoal = CurGoal.DONE_FOR_TODAY;
			} else if (TargetPlanet.IsLeavingSolarSystem()) {
				curGoal = CurGoal.WON;
			} else {
				curGoal = (targetPlanetIdx == TargetPlanet.GetNumPlanets() - 1) ? CurGoal.GAUNTLET : CurGoal.FLY_TO_PLANET;
			}
		} else {
			curGoal = RocketParts.instance.canBuild ? CurGoal.BUILD_ROCKET : CurGoal.COLLECT_PARTS;
		}

		if (curGoal == CurGoal.COLLECT_PARTS && questions.effortTracker.GetQuestion (questions.questions) == null) {
			curGoal = CurGoal.DONE_FOR_TODAY;
		}
		return curGoal;
	}
}
