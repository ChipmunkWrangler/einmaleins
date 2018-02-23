using UnityEngine;

class HideOnWrong : MonoBehaviour, IOnWrongAnswer, IOnGiveUp
{
    const float TransitionTime = EnterAnswerButtonController.TransitionTime;

    [SerializeField] float timeToHide = 0;

    void IOnWrongAnswer.OnWrongAnswer(bool wasNew)
    {
        ScaleDown();
        ScaleUpAfterDelay();
    }

    void IOnGiveUp.OnGiveUp(Question question)
    {
        ScaleDown();
    }

    void ScaleDown()
    {
        iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.zero, "easeType", iTween.EaseType.easeInSine, "time", TransitionTime));
    }

    void ScaleUpAfterDelay()
    {
        iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.one, "easeType", iTween.EaseType.easeInSine, "time", TransitionTime, "delay", timeToHide + TransitionTime));
    }
}
