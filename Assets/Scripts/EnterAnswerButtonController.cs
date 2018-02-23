using UnityEngine;

class EnterAnswerButtonController : MonoBehaviour, IOnWrongAnswer, IOnQuizAborted, IOnQuestionChanged, IOnGiveUp
{
    public const float TransitionTime = 0.25F;

    [SerializeField] UnityEngine.UI.Button button = null;

    bool isHiding;
    bool isShowing;

    void IOnWrongAnswer.OnWrongAnswer(bool wasNew)
    {
        button.interactable = false; // don't hide, just show the give up button on top
    }

    void IOnQuizAborted.OnQuizAborted()
    {
        Hide();
    }

    void IOnQuestionChanged.OnQuestionChanged(Question question)
    {
        if (question == null)
        {
            Hide();
        }
        else
        {
            Show(); // behind the give up button
            button.interactable = false;
        }
    }

    void IOnGiveUp.OnGiveUp(Question question)
    {
        Hide();
    }

    void OnCorrectAnswer()
    {
        Hide();
    }

    void OnAnswerChanged(bool isAnswerEmpty)
    {
        button.interactable = !isAnswerEmpty;
    }

    void Show()
    {
        if (!isShowing)
        {
            ScaleTo(Vector3.one);
            isShowing = true;
        }
        isHiding = false;
        button.interactable = true;
    }

    void Hide()
    {
        if (!isHiding)
        {
            ScaleTo(Vector3.zero);
            isHiding = true;
        }
        isShowing = false;
        button.interactable = false;
    }

    void ScaleTo(Vector3 tgtScale)
    {
        iTween.ScaleTo(gameObject, iTween.Hash("scale", tgtScale, "easeType", iTween.EaseType.easeInSine, "time", TransitionTime));
    }
}
