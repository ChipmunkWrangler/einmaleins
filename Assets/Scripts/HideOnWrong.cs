using UnityEngine;

internal class HideOnWrong : MonoBehaviour, IOnWrongAnswer
{
    private const float TransitionTime = EnterAnswerButtonController.TransitionTime;

    [SerializeField] private float timeToHide;

    void IOnWrongAnswer.OnWrongAnswer(bool wasNew)
    {
        ScaleDown();
        ScaleUpAfterDelay();
    }

    public void OnGiveUp()
    {
        ScaleDown();
    }

    private void ScaleDown()
    {
        iTween.ScaleTo(gameObject,
            iTween.Hash("scale", Vector3.zero, "easeType", iTween.EaseType.easeInSine, "time", TransitionTime));
    }

    private void ScaleUpAfterDelay()
    {
        iTween.ScaleTo(gameObject,
            iTween.Hash("scale", Vector3.one, "easeType", iTween.EaseType.easeInSine, "time", TransitionTime, "delay",
                timeToHide + TransitionTime));
    }
}