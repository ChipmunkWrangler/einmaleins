﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostMeter : MonoBehaviour, OnQuestionChanged, OnCorrectAnswer {
	[SerializeField] RectTransform mask = null;
	[SerializeField] Transform meter = null;

	const float timeToZero = Question.FAST_TIME * 5.2f / 0.75f; // 5.2 is the original height, 0.75 is the y that should be covered in FAST_TIME
	const float hideTime = 0.3f;

	float originalY;

	void Start() {
		originalY = mask.localPosition.y;
		meter.gameObject.SetActive (false);
	}

	public void OnQuestionChanged(Question question) {
		ResetMask ();
		ShowMeter ();
		StartMeter (timeToZero);
	}

	public void OnCorrectAnswer(Question question, bool isNewlyMastered) {
		StopMeter ();
		HideMeter ();
	}

	void ResetMask() {
		SetMaskY (originalY);
	}

	void ShowMeter() {
		meter.gameObject.SetActive (true);
	}

	void HideMeter() {
//		meter.gameObject.SetActive (false);
		StartMeter(hideTime);
	}

	void StartMeter(float t) {
		iTween.ValueTo(mask.gameObject, iTween.Hash("from", mask.localPosition.y, "to", mask.localPosition.y - mask.rect.height, "time", t, "onupdate", "SetMaskY"));
	}

	void StopMeter() {
		iTween.Stop (mask.gameObject);
	}

	void SetMaskY (float y)
	{
		meter.SetParent (mask.parent);
		Vector3 pos = mask.localPosition;
		pos.y = y;
		mask.localPosition = pos;
		meter.SetParent (mask);
	}
}