using System.Collections.Generic;
using UnityEngine;

class Fuel : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text fuelCountText = null;
    [SerializeField] EffortTracker effortTracker = null;

    public void UpdateFuelDisplay(int numAnswersLeftInQuiz)
    {
        int fuelCount = Mathf.Max(0, numAnswersLeftInQuiz);
        fuelCountText.text = fuelCount.ToString();
    }

    void Start()
    {
        UpdateFuelDisplay(effortTracker.GetNumAnswersInQuiz(Goal.IsReadyForGauntlet()));
    }
}
