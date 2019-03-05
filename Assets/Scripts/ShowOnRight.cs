using UnityEngine;

internal class ShowOnRight : MonoBehaviour, IOnQuestionChanged, IOnWrongAnswer, IOnQuizAborted
{
    private const float TransitionTime = EnterAnswerButtonController.TransitionTime;
    [SerializeField] private bool evenIfWrongFirst;

    [SerializeField] private bool hideOnRight;

    private bool wasWrong;

    void IOnQuestionChanged.OnQuestionChanged(Question question)
    {
        wasWrong = false;
        ScaleTo(hideOnRight == (question != null) ? Vector3.one : Vector3.zero);
    }

    void IOnQuizAborted.OnQuizAborted()
    {
        ScaleTo(Vector3.zero);
    }

    void IOnWrongAnswer.OnWrongAnswer(bool wasNew)
    {
        wasWrong = true;
    }

    private void OnCorrectAnswer()
    {
        if (evenIfWrongFirst || !wasWrong) ScaleTo(hideOnRight ? Vector3.zero : Vector3.one);
    }

    private void Start()
    {
        gameObject.transform.localScale = hideOnRight ? Vector3.one : Vector3.zero;
    }

    private void ScaleTo(Vector3 tgtScale)
    {
        iTween.ScaleTo(gameObject,
            iTween.Hash("scale", tgtScale, "easeType", iTween.EaseType.easeInSine, "time", TransitionTime));
    }
}