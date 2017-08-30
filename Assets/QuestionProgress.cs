using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionProgress : MonoBehaviour, OnCorrectAnswer, OnQuestionChanged {
	[SerializeField] float transitionTime = 0.5f;
	float speed;
	float curFraction;
	float targetFraction;
	Material mat;

	void Start() {
		mat = GetComponent<Renderer> ().material;
	}

	public void OnCorrectAnswer (Question question) {
		targetFraction = question.GetMasteryFraction();
		speed = (targetFraction - curFraction) / transitionTime;
	}

	public void OnQuestionChanged(Question question) {
		SetProgress(question.GetMasteryFraction ());
		targetFraction = curFraction;
	}
		
	void Update() {
		if (!Mathf.Approximately(curFraction, targetFraction)) {
			SetProgress(Mathf.MoveTowards (curFraction, targetFraction, Time.deltaTime * speed));
		}
	}

	void SetProgress (float progress) {
		print ("progress " + progress);
		curFraction = progress;
		mat.SetFloat ("_Progress", curFraction);
	}


}
