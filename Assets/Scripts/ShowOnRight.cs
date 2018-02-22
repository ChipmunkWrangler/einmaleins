using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnRight : MonoBehaviour, IOnQuestionChanged, IOnWrongAnswer, IOnQuizAborted
{
    [SerializeField] bool hideOnRight = false;
    [SerializeField] bool evenIfWrongFirst = false;

    const float TransitionTime = EnterAnswerButtonController.TransitionTime;

    bool wasWrong;

    void Start()
    {
        gameObject.transform.localScale = hideOnRight ? Vector3.one : Vector3.zero;
    }

    public void OnCorrectAnswer()
    {
        if (evenIfWrongFirst || !wasWrong)
        {
            ScaleTo(hideOnRight ? Vector3.zero : Vector3.one);
        }
    }

    public void OnQuizAborted()
    {
        ScaleTo(Vector3.zero);
    }

    public void OnQuestionChanged(Question question)
    {
        wasWrong = false;
        ScaleTo(hideOnRight == (question != null) ? Vector3.one : Vector3.zero);
    }

    public void OnWrongAnswer(bool wasNew)
    {
        wasWrong = true;
    }

    void ScaleTo(Vector3 tgtScale)
    {
        iTween.ScaleTo(gameObject, iTween.Hash("scale", tgtScale, "easeType", iTween.EaseType.easeInSine, "time", TransitionTime));
    }
}
