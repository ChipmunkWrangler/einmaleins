using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostMeter : MonoBehaviour, IOnQuestionChanged, IOnGiveUp, IOnQuizAborted
{
    [SerializeField] RectTransform Mask = null;
    [SerializeField] Transform Meter = null;

    const float TimeToZero = Question.FastTime * 5.2F / 0.75F; // 5.2 is the original height, 0.75 is the y that should be covered in FAST_TIME
    const float HideTime = 0.3F;

    float OriginalY;

    void Start()
    {
        OriginalY = Mask.localPosition.y;
        Meter.gameObject.SetActive(false);
    }

    public void OnQuizAborted()
    {
        StopMeter();
        Meter.gameObject.SetActive(false);
    }

    public void OnQuestionChanged(Question question)
    {
        if (question != null && !question.IsLaunchCode)
        {
            ResetMask();
            ShowMeter();
            StartMeter(TimeToZero);
        }
    }

    public void OnCorrectAnswer()
    {
        StopMeter();
        HideMeter();
    }

    public void OnGiveUp(Question question)
    {
        StopMeter();
        Meter.gameObject.SetActive(false);
    }

    void ResetMask()
    {
        SetMaskY(OriginalY);
    }

    void ShowMeter()
    {
        Meter.gameObject.SetActive(true);
    }

    void HideMeter()
    {
        StartMeter(HideTime);
    }

    void StartMeter(float t)
    {
        iTween.ValueTo(Mask.gameObject, iTween.Hash("from", Mask.localPosition.y, "to", Mask.localPosition.y - Mask.rect.height, "time", t, "onupdate", "SetMaskY"));
    }

    void StopMeter()
    {
        iTween.Stop(Mask.gameObject);
    }

    void SetMaskY(float y)
    {
        Meter.SetParent(Mask.parent);
        Vector3 pos = Mask.localPosition;
        pos.y = y;
        Mask.localPosition = pos;
        Meter.SetParent(Mask);
    }
}
