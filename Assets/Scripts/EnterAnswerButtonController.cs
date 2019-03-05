using UnityEngine;
using UnityEngine.UI;

internal class EnterAnswerButtonController : MonoBehaviour, IOnWrongAnswer, IOnQuizAborted, IOnQuestionChanged
{
    public const float TransitionTime = 0.25F;

    [SerializeField] private Button button;

    private bool isHiding;
    private bool isShowing;

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

    void IOnQuizAborted.OnQuizAborted()
    {
        Hide();
    }

    void IOnWrongAnswer.OnWrongAnswer(bool wasNew)
    {
        button.interactable = false; // don't hide, just show the give up button on top
    }

    public void OnGiveUp()
    {
        Hide();
    }

    private void OnCorrectAnswer()
    {
        Hide();
    }

    private void OnAnswerChanged(bool isAnswerEmpty)
    {
        button.interactable = !isAnswerEmpty;
    }

    private void Show()
    {
        if (!isShowing)
        {
            ScaleTo(Vector3.one);
            isShowing = true;
        }

        isHiding = false;
        button.interactable = true;
    }

    private void Hide()
    {
        if (!isHiding)
        {
            ScaleTo(Vector3.zero);
            isHiding = true;
        }

        isShowing = false;
        button.interactable = false;
    }

    private void ScaleTo(Vector3 tgtScale)
    {
        iTween.ScaleTo(gameObject,
            iTween.Hash("scale", tgtScale, "easeType", iTween.EaseType.easeInSine, "time", TransitionTime));
    }
}