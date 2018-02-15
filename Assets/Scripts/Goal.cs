using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour {
    [SerializeField] EffortTracker effortTracker = null;

	public enum CurGoal {
		UPGRADE_ROCKET,
		FLY_TO_PLANET,
		GAUNTLET,
		DONE_FOR_TODAY,
		WON // try to get high score
	}

    public static bool IsGivingUpAllowed(CurGoal curGoal) => curGoal == Goal.CurGoal.FLY_TO_PLANET;

    public static bool IsReadyForGauntlet()
    {
        return (TargetPlanet.GetTargetPlanetIdx() == TargetPlanet.GetMaxPlanetIdx()) &&
            RocketParts.instance.upgradeLevel == RocketParts.instance.maxUpgradeLevel - 1;
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
		
    static bool ShouldUpgrade() => (RocketParts.instance.hasEnoughPartsToUpgrade || !ChooseRocketColour.HasChosenColour()) && !RocketParts.instance.justUpgraded;
    static bool IsLeavingSolarSystem() => TargetPlanet.GetTargetPlanetIdx() > TargetPlanet.GetMaxPlanetIdx();

    bool IsDoneForToday() => effortTracker.IsDoneForToday() && !RocketParts.instance.justUpgraded;
}
