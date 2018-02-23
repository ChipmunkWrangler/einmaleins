using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class GiveUpOkButton : MonoBehaviour, IOnGiveUp
{
    const float TransitionTime = EnterAnswerButtonController.TransitionTime;

    [SerializeField] UnityEngine.UI.Button button = null;

    void IOnGiveUp.OnGiveUp(Question question)
    {
        Show();
    }

    void Hide()
    {
        button.enabled = false;
        ScaleTo(Vector3.zero);
    }

    void Start()
    {
        Hide();
    }

    void Show()
    {
        button.enabled = true;
        ScaleTo(Vector3.one);
    }

    void ScaleTo(Vector3 tgtScale)
    {
        iTween.ScaleTo(button.gameObject, iTween.Hash("scale", tgtScale, "easeType", iTween.EaseType.easeInSine, "time", TransitionTime));
    }
}
