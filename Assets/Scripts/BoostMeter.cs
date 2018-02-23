﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BoostMeter : MonoBehaviour, IOnQuestionChanged, IOnQuizAborted
{
    const float TimeToZero = Question.FastTime * 5.2F / 0.75F; // 5.2 is the original height, 0.75 is the y that should be covered in FAST_TIME
    const float HideTime = 0.3F;

    [SerializeField] RectTransform mask = null;
    [SerializeField] Transform meter = null;

    float originalY;

    public void OnGiveUp()
    {
        StopMeter();
        meter.gameObject.SetActive(false);
    }

    void IOnQuizAborted.OnQuizAborted()
    {
        StopMeter();
        meter.gameObject.SetActive(false);
    }

    void IOnQuestionChanged.OnQuestionChanged(Question question)
    {
        if (question != null && !question.IsLaunchCode)
        {
            ResetMask();
            ShowMeter();
            StartMeter(TimeToZero);
        }
    }

    void OnCorrectAnswer()
    {
        StopMeter();
        HideMeter();
    }

    void Start()
    {
        originalY = mask.localPosition.y;
        meter.gameObject.SetActive(false);
    }

    void ResetMask()
    {
        SetMaskY(originalY);
    }

    void ShowMeter()
    {
        meter.gameObject.SetActive(true);
    }

    void HideMeter()
    {
        StartMeter(HideTime);
    }

    void StartMeter(float t)
    {
        iTween.ValueTo(mask.gameObject, iTween.Hash("from", mask.localPosition.y, "to", mask.localPosition.y - mask.rect.height, "time", t, "onupdate", "SetMaskY"));
    }

    void StopMeter()
    {
        iTween.Stop(mask.gameObject);
    }

    void SetMaskY(float y)
    {
        meter.SetParent(mask.parent);
        Vector3 pos = mask.localPosition;
        pos.y = y;
        mask.localPosition = pos;
        meter.SetParent(mask);
    }
}
