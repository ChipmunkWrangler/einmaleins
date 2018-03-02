using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class Goal : MonoBehaviour
{
    [SerializeField] EffortTracker effortTracker = null;
    [SerializeField] RocketColour chooseRocketColourData = null;
    [SerializeField] TargetPlanet targetPlanet = null;

    public enum CurGoal
    {
        UpgradeRocket,
        FlyToPlanet,
        Gauntlet,
        DoneForToday,
        Won // try to get high score
    }

    public static bool IsGivingUpAllowed(CurGoal curGoal) => curGoal == Goal.CurGoal.FlyToPlanet;

    public bool IsReadyForGauntlet()
    {
        return (targetPlanet.GetTargetPlanetIdx() == TargetPlanet.GetMaxPlanetIdx()) &&
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

    bool ShouldUpgrade() => (RocketParts.Instance.HasEnoughPartsToUpgrade || !chooseRocketColourData.HasChosenColour()) && !RocketParts.Instance.JustUpgraded;
    bool IsLeavingSolarSystem() => targetPlanet.GetTargetPlanetIdx() > TargetPlanet.GetMaxPlanetIdx();

    bool IsDoneForToday() => effortTracker.IsDoneForToday() && !RocketParts.Instance.JustUpgraded;
}
