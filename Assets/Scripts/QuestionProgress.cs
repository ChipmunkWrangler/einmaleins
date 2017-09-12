using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Material))]
public class QuestionProgress : MonoBehaviour, OnCorrectAnswer, OnQuestionChanged {
	[SerializeField] float transitionTime = 0.5f;
	[SerializeField] float delay;
	float speed;
	float startTime;
	float curFraction;
	float targetFraction;
	Material mat_;

	public void OnCorrectAnswer (Question question, bool isNewlyMastered) {
		SetProgress (0);
//		targetFraction = question.GetMasteryFraction ();
		speed = (targetFraction - curFraction) / transitionTime;
		startTime = delay + Time.time;
	}

	public void OnQuestionChanged(Question question) {
//		curFraction = question.GetMasteryFraction ();
		targetFraction = curFraction;
	}
				
	void Update() {
		if (Time.time >= startTime && !Mathf.Approximately(curFraction, targetFraction)) {
			SetProgress(Mathf.MoveTowards (curFraction, targetFraction, Time.deltaTime * speed));
		}
	}

	void SetProgress (float progress) {
		curFraction = progress;
		GetMaterial().SetFloat ("_Progress", curFraction);
	}

	Material GetMaterial() {
		if (!mat_) {
			mat_ = GetComponent<Renderer> ().material;
		}
		return mat_;
	}

}
