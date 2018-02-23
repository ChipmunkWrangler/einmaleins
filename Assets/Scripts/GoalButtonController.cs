using UnityEngine;

class GoalButtonController : MonoBehaviour, IOnQuestionChanged
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
            }
        }
    }
}
