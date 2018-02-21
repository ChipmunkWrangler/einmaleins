using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour {
    [SerializeField] EffortTracker EffortTracker = null;

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
            RocketParts.Instance.UpgradeLevel == RocketParts.Instance.MaxUpgradeLevel - 1;
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
		
    static bool ShouldUpgrade() => (RocketParts.Instance.HasEnoughPartsToUpgrade || !ChooseRocketColour.HasChosenColour()) && !RocketParts.Instance.JustUpgraded;
    static bool IsLeavingSolarSystem() => TargetPlanet.GetTargetPlanetIdx() > TargetPlanet.GetMaxPlanetIdx();

    bool IsDoneForToday() => EffortTracker.IsDoneForToday() && !RocketParts.Instance.JustUpgraded;
}
