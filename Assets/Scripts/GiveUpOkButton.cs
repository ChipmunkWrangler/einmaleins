using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveUpOkButton : MonoBehaviour, IOnGiveUp
{
    const float TransitionTime = EnterAnswerButtonController.TransitionTime;

    [SerializeField] UnityEngine.UI.Button button = null;

    public void OnGiveUp(Question question)
    {
        Show();
    }

    public void Hide()
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
