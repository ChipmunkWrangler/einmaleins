using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GoalButtonControler : MonoBehaviour, OnQuestionChanged
{
    [SerializeField] LaunchButtonController launchButton = null;
    [SerializeField] GameObject upgradeButton = null;
    [SerializeField] GameObject doneText = null;
    [SerializeField] GameObject youWinText = null;
    [SerializeField] Goal goal = null;

    public void OnQuestionChanged(Question question)
    {
        UpdateButtonStatus(noMoreQuestions: question == null);
    }

    void UpdateButtonStatus(bool noMoreQuestions = true)
    {
        launchButton.Deactivate();
        upgradeButton.SetActive(false);
        youWinText.SetActive(false);
        doneText.SetActive(false);
        if (noMoreQuestions)
        {
            switch (goal.CalcCurGoal())
            {
                case Goal.CurGoal.UPGRADE_ROCKET:
                    upgradeButton.SetActive(true);
                    break;
                case Goal.CurGoal.FLY_TO_PLANET:
                case Goal.CurGoal.GAUNTLET:
                    launchButton.ActivateLaunch();
                    break;
                case Goal.CurGoal.DONE_FOR_TODAY:
                    doneText.SetActive(true);
                    break;
                case Goal.CurGoal.WON:
                    launchButton.ActivateLaunch();
                    youWinText.SetActive(true);
                    break;
            }
        }
    }
}
