using System.Collections.Generic;
using CrazyChipmunk;
using UnityEngine;
using UnityEngine.UI;

internal class KickoffLaunch : MonoBehaviour
{
    [SerializeField] private Celebrate celebrate;
    [SerializeField] private Text countdownText;
    [SerializeField] private int countdownTime = 3;
    [SerializeField] private float delay = 1.0F;
    [SerializeField] private Goal goal;
    [SerializeField] private GoalButtonController goalButtonController;
    [SerializeField] private Prefs prefs;
    [SerializeField] private QuestionPicker questionPicker;
    [SerializeField] private Questions questions;
    [SerializeField] private FlashThrust thrust;
    [SerializeField] private GameObject[] uiElementsToActivateOnCountdown;
    [SerializeField] private GameObject[] uiElementsToActivateOnLaunchButton;
    [SerializeField] private GameObject[] uiElementsToActivateOnLaunchCode;
    [SerializeField] private GameObject[] uiElementsToActivateOnPlay;
    [SerializeField] private GameObject[] uiElementsToDeactivateIfGivingUpIsForbidden;
    [SerializeField] private GameObject[] uiElementsToDeactivateOnCountdown;
    [SerializeField] private GameObject[] uiElementsToDeactivateOnLaunchButton;
    [SerializeField] private GameObject[] uiElementsToDeactivateOnLaunchCode;
    [SerializeField] private GameObject[] uiElementsToDeactivateOnPlay;

    public void PreLaunch()
    {
        var launchCodeQuestion = questions.GetGaveUpQuestion();
        if (launchCodeQuestion == null)
            Launch();
        else
            RequestLaunchCode(launchCodeQuestion);
    }

    public void Launch()
    {
        StartCoroutine(Kickoff());
    }

    public void ShowLaunchButton()
    {
        foreach (var element in uiElementsToActivateOnLaunchButton) element.SetActive(true);
        foreach (var element in uiElementsToDeactivateOnLaunchButton) element.SetActive(false);

        goalButtonController.OnQuestionChanged(null);
    }

    public void OnCorrectAnswer(Question question, bool isNewlyMastered)
    {
        if (question.IsLaunchCode) Launch();
    }

    private void Start()
    {
        if (prefs.GetBool("autolaunch"))
        {
            prefs.SetBool("autolaunch", false);
            PreLaunch();
        }
        else
        {
            ShowLaunchButton();
        }
    }

    private IEnumerator<WaitForSeconds> Kickoff()
    {
        thrust.OnCountdown();
        countdownText.text = "";
        countdownText.gameObject.SetActive(true);
        foreach (var element in uiElementsToActivateOnCountdown) element.SetActive(true);
        foreach (var element in uiElementsToDeactivateOnCountdown) element.SetActive(false);
        yield return new WaitForSeconds(delay);
        for (var i = countdownTime; i > 0; --i)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1.0F);
        }

        countdownText.text = "";
        countdownText.gameObject.SetActive(false);
        foreach (var element in uiElementsToDeactivateOnPlay) element.SetActive(false);
        yield return null;
        thrust.Accelerate();
        celebrate.OnCorrectAnswer(null, false); // this triggers the question once the flames are done
        yield return new WaitForSeconds(Celebrate.Duration);
        foreach (var element in uiElementsToActivateOnPlay) element.SetActive(true);
        if (!Goal.IsGivingUpAllowed(goal.CalcCurGoal()))
            foreach (var element in uiElementsToDeactivateIfGivingUpIsForbidden)
                element.SetActive(false);
    }

    private void RequestLaunchCode(Question launchCodeQuestion)
    {
        foreach (var element in uiElementsToActivateOnLaunchCode) element.SetActive(true);
        foreach (var element in uiElementsToDeactivateOnLaunchCode) element.SetActive(false);
        questionPicker.ShowQuestion(launchCodeQuestion);
        launchCodeQuestion.IsLaunchCode = true;
    }
}