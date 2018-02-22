using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : MonoBehaviour {
    [SerializeField] UnityEngine.UI.Text fuelCountText = null;

	void Start() {
		UpdateFuelDisplay (EffortTracker.GetNumAnswersInQuiz (Goal.IsReadyForGauntlet ()));
	}

	public void UpdateFuelDisplay(int numAnswersLeftInQuiz) {
		int fuelCount = Mathf.Max(0, numAnswersLeftInQuiz);
		fuelCountText.text = fuelCount.ToString();
	}
}
