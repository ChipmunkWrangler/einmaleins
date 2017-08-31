using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionProgress : MonoBehaviour, OnCorrectAnswer, OnQuestionChanged {
	[SerializeField] float transitionTime = 0.5f;
	float speed;
	float curFraction;
	float targetFraction;
	Material mat_;

	public void OnCorrectAnswer (Question question) {
		SetProgress (curFraction);
		targetFraction = question.GetMasteryFraction ();
		speed = (targetFraction - curFraction) / transitionTime;
	}

	public void OnQuestionChanged(Question question) {
		curFraction = question.GetMasteryFraction ();
		targetFraction = curFraction;
	}
				
	void Update() {
		if (!Mathf.Approximately(curFraction, targetFraction)) {
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
