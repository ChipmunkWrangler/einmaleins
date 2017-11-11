using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostMeter : MonoBehaviour, OnQuestionChanged, OnCorrectAnswer {
	[SerializeField] RectTransform mask = null;
	[SerializeField] Transform meter = null;
	[SerializeField] float timeToZero = 15f;

	float originalY;

//	bool isRunning = false;

	void Start() {
		originalY = mask.localPosition.y;
	}

	public void OnQuestionChanged(Question question) {
		ResetMask ();
		ShowMeter ();
		StartMeter ();
//		isRunning = true;
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
		meter.gameObject.SetActive (false);
	}

	void StartMeter() {
		iTween.ValueTo(mask.gameObject, iTween.Hash("from", mask.localPosition.y, "to", mask.localPosition.y - mask.rect.height, "time", timeToZero, "onupdate", "SetMaskY"));
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
