using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : MonoBehaviour, OnCorrectAnswer, OnWrongAnswer {
	[SerializeField] UnityEngine.UI.Text fuelCountText = null;
	[SerializeField] Goal goal = null;
	int fuelCount;

	void Start() {
		fuelCount = EffortTracker.GetNumAnswersInQuiz (goal.calcCurGoal() == Goal.CurGoal.GAUNTLET);
		UpdateFuelDisplay ();
	}

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		ReduceFuel ();
	}

	public void OnWrongAnswer(bool wasNew) {
		ReduceFuel ();
	}

	void ReduceFuel () {
		--fuelCount;
		if (fuelCount < 0) {
			fuelCount = 0;
		}
		UpdateFuelDisplay ();
	}

	void UpdateFuelDisplay() {
		fuelCountText.text = fuelCount.ToString();
	}
}
