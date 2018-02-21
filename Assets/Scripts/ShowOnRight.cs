using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnRight : MonoBehaviour, IOnQuestionChanged, IOnWrongAnswer, IOnQuizAborted
{
    [SerializeField] bool HideOnRight = false;
    [SerializeField] bool EvenIfWrongFirst = false;

    const float TransitionTime = EnterAnswerButtonController.TransitionTime;

    bool WasWrong;

    void Start()
    {
        gameObject.transform.localScale = HideOnRight ? Vector3.one : Vector3.zero;
    }

    public void OnCorrectAnswer()
    {
        if (EvenIfWrongFirst || !WasWrong)
        {
            ScaleTo(HideOnRight ? Vector3.zero : Vector3.one);
        }
    }

    public void OnQuizAborted()
    {
        ScaleTo(Vector3.zero);
    }

    public void OnQuestionChanged(Question question)
    {
        WasWrong = false;
        ScaleTo(HideOnRight == (question != null) ? Vector3.one : Vector3.zero);
    }

    public void OnWrongAnswer(bool wasNew)
    {
        WasWrong = true;
    }

    void ScaleTo(Vector3 tgtScale)
    {
        iTween.ScaleTo(gameObject, iTween.Hash("scale", tgtScale, "easeType", iTween.EaseType.easeInSine, "time", TransitionTime));
    }
}
