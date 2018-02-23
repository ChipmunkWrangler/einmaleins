using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class GiveUpButtonController : MonoBehaviour, IOnWrongAnswer, IOnQuizAborted, IOnQuestionChanged, IOnGiveUp
{
    const float TransitionTime = EnterAnswerButtonController.TransitionTime;

    [SerializeField] UnityEngine.UI.Button button = null;
    [SerializeField] UnityEngine.UI.Image image = null;

    void IOnQuizAborted.OnQuizAborted()
    {
        SetInteractibility(false);
    }

    void IOnWrongAnswer.OnWrongAnswer(bool wasNew)
    {
        SetInteractibility(true);
    }

    void IOnQuestionChanged.OnQuestionChanged(Question question)
    {
        SetInteractibility(question != null);
    }

    void IOnGiveUp.OnGiveUp(Question question)
    {
        SetInteractibility(false);
    }

    void OnCorrectAnswer()
    {
        SetInteractibility(false);
    }

    void OnAnswerChanged(bool isAnswerEmpty)
    {
        SetInteractibility(isAnswerEmpty);
    }

    void SetInteractibility(bool b)
    {
        if (button.interactable != b)
        {
            button.interactable = b;
            image.raycastTarget = b; // want to be able to tap the ok button, which is behind this one, immediately
            ScaleTo(b ? Vector3.one : Vector3.zero);
        }
    }

    void ScaleTo(Vector3 tgtScale)
    {
        iTween.ScaleTo(gameObject, iTween.Hash("scale", tgtScale, "easeType", iTween.EaseType.easeInSine, "time", TransitionTime));
    }
}
