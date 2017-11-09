using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour {
	[SerializeField] Questions questions;

	public enum CurGoal {
		UPGRADE_ROCKET,
		FLY_TO_PLANET,
		GAUNTLET,
		DONE_FOR_TODAY,
		WON // try to get high score
	}

	public CurGoal calcCurGoal() {
		CurGoal curGoal;
		if (RocketParts.instance.hasEnoughPartsToUpgrade) {
			curGoal = CurGoal.UPGRADE_ROCKET;
		} else if (questions.effortTracker.HasMaxedEffort ()) {
			curGoal = CurGoal.DONE_FOR_TODAY;
		} else if (TargetPlanet.IsLeavingSolarSystem()) {
			curGoal = CurGoal.WON;
		} else {
			curGoal = (TargetPlanet.GetTargetPlanetIdx () == TargetPlanet.GetNumPlanets() - 1) ? CurGoal.GAUNTLET : CurGoal.FLY_TO_PLANET;
		}

		return curGoal;
	}
}
