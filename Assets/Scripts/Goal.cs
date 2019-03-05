using UnityEngine;

internal class Goal : MonoBehaviour
{
    public enum CurGoal
    {
        UpgradeRocket,
        FlyToPlanet,
        Gauntlet,
        DoneForToday,
        Won // try to get high score
    }

    [SerializeField] private RocketColour chooseRocketColourData;
    [SerializeField] private EffortTracker effortTracker;
    [SerializeField] private TargetPlanet targetPlanet;

    public static bool IsGivingUpAllowed(CurGoal curGoal)
    {
        return curGoal == CurGoal.FlyToPlanet;
    }

    public bool IsReadyForGauntlet()
    {
        return targetPlanet.GetTargetPlanetIdx() == TargetPlanet.GetMaxPlanetIdx() &&
               RocketParts.Instance.UpgradeLevel == RocketParts.Instance.MaxUpgradeLevel - 1;
    }

    public CurGoal CalcCurGoal()
    {
        CurGoal curGoal;
        if (ShouldUpgrade())
            curGoal = CurGoal.UpgradeRocket;
        else if (IsDoneForToday())
            curGoal = CurGoal.DoneForToday;
        else if (IsLeavingSolarSystem())
            curGoal = CurGoal.Won;
        else
            curGoal = IsReadyForGauntlet() ? CurGoal.Gauntlet : CurGoal.FlyToPlanet;

        return curGoal;
    }

    private bool ShouldUpgrade()
    {
        return (RocketParts.Instance.HasEnoughPartsToUpgrade || !chooseRocketColourData.HasChosenColour()) &&
               !RocketParts.Instance.JustUpgraded;
    }

    private bool IsLeavingSolarSystem()
    {
        return targetPlanet.GetTargetPlanetIdx() > TargetPlanet.GetMaxPlanetIdx();
    }

    private bool IsDoneForToday()
    {
        return effortTracker.IsDoneForToday() && !RocketParts.Instance.JustUpgraded;
    }
}