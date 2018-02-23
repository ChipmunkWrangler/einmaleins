using UnityEngine;

class HideOnWrong : MonoBehaviour, IOnWrongAnswer
{
    const float TransitionTime = EnterAnswerButtonController.TransitionTime;

    [SerializeField] float timeToHide = 0;

    public void OnGiveUp()
    {
        ScaleDown();
    }

    void IOnWrongAnswer.OnWrongAnswer(bool wasNew)
    {
        ScaleDown();
        ScaleUpAfterDelay();
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
