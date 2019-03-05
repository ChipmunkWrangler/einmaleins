using UnityEngine;
using UnityEngine.UI;

internal class Fuel : MonoBehaviour
{
    [SerializeField] private EffortTracker effortTracker;
    [SerializeField] private Text fuelCountText;
    [SerializeField] private Goal goal;

    public void UpdateFuelDisplay(int numAnswersLeftInQuiz)
    {
        var fuelCount = Mathf.Max(0, numAnswersLeftInQuiz);
        fuelCountText.text = fuelCount.ToString();
    }

    private void Start()
    {
        UpdateFuelDisplay(effortTracker.GetNumAnswersInQuiz(goal.IsReadyForGauntlet()));
    }
}