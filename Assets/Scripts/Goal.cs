using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class Goal : MonoBehaviour
{
    [SerializeField] EffortTracker effortTracker = null;

    public enum CurGoal
    {
        UpgradeRocket,
        FlyToPlanet,
        Gauntlet,
        DoneForToday,
        Won // try to get high score
    }

    public static bool IsGivingUpAllowed(CurGoal curGoal) => curGoal == Goal.CurGoal.FlyToPlanet;

    public static bool IsReadyForGauntlet()
    {
        return (TargetPlanet.GetTargetPlanetIdx() == TargetPlanet.GetMaxPlanetIdx()) &&
            RocketParts.Instance.UpgradeLevel == RocketParts.Instance.MaxUpgradeLevel - 1;
    }

    public CurGoal CalcCurGoal()
    {
        CurGoal curGoal;
        if (ShouldUpgrade())
        {
            curGoal = CurGoal.UpgradeRocket;
        }
        else if (IsDoneForToday())
        {
            curGoal = CurGoal.DoneForToday;
        }
        else if (IsLeavingSolarSystem())
        {
            curGoal = CurGoal.Won;
        }
        else
        {
            curGoal = IsReadyForGauntlet() ? CurGoal.Gauntlet : CurGoal.FlyToPlanet;
        }

        return curGoal;
    }

    static bool ShouldUpgrade() => (RocketParts.Instance.HasEnoughPartsToUpgrade || !ChooseRocketColour.HasChosenColour()) && !RocketParts.Instance.JustUpgraded;
    static bool IsLeavingSolarSystem() => TargetPlanet.GetTargetPlanetIdx() > TargetPlanet.GetMaxPlanetIdx();

    bool IsDoneForToday() => effortTracker.IsDoneForToday() && !RocketParts.Instance.JustUpgraded;
}
