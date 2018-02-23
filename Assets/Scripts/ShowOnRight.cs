using UnityEngine;

class ShowOnRight : MonoBehaviour, IOnQuestionChanged, IOnWrongAnswer, IOnQuizAborted
{
    const float TransitionTime = EnterAnswerButtonController.TransitionTime;

    [SerializeField] bool hideOnRight = false;
    [SerializeField] bool evenIfWrongFirst = false;

    bool wasWrong;

    void IOnQuizAborted.OnQuizAborted()
    {
        ScaleTo(Vector3.zero);
    }

    void IOnQuestionChanged.OnQuestionChanged(Question question)
    {
        wasWrong = false;
        ScaleTo(hideOnRight == (question != null) ? Vector3.one : Vector3.zero);
    }

    void IOnWrongAnswer.OnWrongAnswer(bool wasNew)
    {
        wasWrong = true;
    }

    void OnCorrectAnswer()
    {
        if (evenIfWrongFirst || !wasWrong)
        {
            ScaleTo(hideOnRight ? Vector3.zero : Vector3.one);
        }
    }

    void Start()
    {
        gameObject.transform.localScale = hideOnRight ? Vector3.one : Vector3.zero;
    }

    void ScaleTo(Vector3 tgtScale)
    {
        iTween.ScaleTo(gameObject, iTween.Hash("scale", tgtScale, "easeType", iTween.EaseType.easeInSine, "time", TransitionTime));
    }
}
