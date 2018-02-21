using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GoalButtonControler : MonoBehaviour, IOnQuestionChanged
{
    [SerializeField] LaunchButtonController LaunchButton = null;
    [SerializeField] GameObject UpgradeButton = null;
    [SerializeField] GameObject DoneText = null;
    [SerializeField] GameObject YouWinText = null;
    [SerializeField] Goal GoalStatus = null;

    public void OnQuestionChanged(Question question)
    {
        UpdateButtonStatus(noMoreQuestions: question == null);
    }

    void UpdateButtonStatus(bool noMoreQuestions = true)
    {
        LaunchButton.Deactivate();
        UpgradeButton.SetActive(false);
        YouWinText.SetActive(false);
        DoneText.SetActive(false);
        if (noMoreQuestions)
        {
            switch (GoalStatus.CalcCurGoal())
            {
                case Goal.CurGoal.UPGRADE_ROCKET:
                    UpgradeButton.SetActive(true);
                    break;
                case Goal.CurGoal.FLY_TO_PLANET:
                case Goal.CurGoal.GAUNTLET:
                    LaunchButton.ActivateLaunch();
                    break;
                case Goal.CurGoal.DONE_FOR_TODAY:
                    DoneText.SetActive(true);
                    break;
                case Goal.CurGoal.WON:
                    LaunchButton.ActivateLaunch();
                    YouWinText.SetActive(true);
                    break;
            }
        }
    }
}
