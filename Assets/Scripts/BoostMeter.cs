using UnityEngine;

internal class BoostMeter : MonoBehaviour, IOnQuestionChanged, IOnQuizAborted
{
    private const float
        TimeToZero =
            Question.FastTime * 5.2F /
            0.75F; // 5.2 is the original height, 0.75 is the y that should be covered in FAST_TIME

    private const float HideTime = 0.3F;

    [SerializeField] private RectTransform mask;
    [SerializeField] private Transform meter;

    private float originalY;

    void IOnQuestionChanged.OnQuestionChanged(Question question)
    {
        if (question != null && !question.IsLaunchCode)
        {
            ResetMask();
            ShowMeter();
            StartMeter(TimeToZero);
        }
    }

    void IOnQuizAborted.OnQuizAborted()
    {
        StopMeter();
        meter.gameObject.SetActive(false);
    }

    public void OnGiveUp()
    {
        StopMeter();
        meter.gameObject.SetActive(false);
    }

    private void OnCorrectAnswer()
    {
        StopMeter();
        HideMeter();
    }

    private void Start()
    {
        originalY = mask.localPosition.y;
        meter.gameObject.SetActive(false);
    }

    private void ResetMask()
    {
        SetMaskY(originalY);
    }

    private void ShowMeter()
    {
        meter.gameObject.SetActive(true);
    }

    private void HideMeter()
    {
        StartMeter(HideTime);
    }

    private void StartMeter(float t)
    {
        iTween.ValueTo(mask.gameObject,
            iTween.Hash("from", mask.localPosition.y, "to", mask.localPosition.y - mask.rect.height, "time", t,
                "onupdate", "SetMaskY"));
    }

    private void StopMeter()
    {
        iTween.Stop(mask.gameObject);
    }

    private void SetMaskY(float y)
    {
        meter.SetParent(mask.parent);
        var pos = mask.localPosition;
        pos.y = y;
        mask.localPosition = pos;
        meter.SetParent(mask);
    }
}