using System;
using UnityEngine;

internal class GoalButtonController : MonoBehaviour, IOnQuestionChanged
{
    [SerializeField] private GameObject doneText;
    [SerializeField] private Goal goal;
    [SerializeField] private LaunchButtonController launchButton;
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private GameObject youWinText;

    public void OnQuestionChanged(Question question)
    {
        UpdateButtonStatus(question == null);
    }

    private void UpdateButtonStatus(bool noMoreQuestions = true)
    {
        launchButton.Deactivate();
        upgradeButton.SetActive(false);
        youWinText.SetActive(false);
        doneText.SetActive(false);
        if (noMoreQuestions)
            switch (goal.CalcCurGoal())
            {
                case Goal.CurGoal.UpgradeRocket:
                    upgradeButton.SetActive(true);
                    break;
                case Goal.CurGoal.FlyToPlanet:
                case Goal.CurGoal.Gauntlet:
                    launchButton.ActivateLaunch();
                    break;
                case Goal.CurGoal.DoneForToday:
                    doneText.SetActive(true);
                    break;
                case Goal.CurGoal.Won:
                    launchButton.ActivateLaunch();
                    youWinText.SetActive(true);
                    break;
                default:
                    throw new InvalidOperationException("Invalid goal: " + goal.CalcCurGoal());
            }
    }
}