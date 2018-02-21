using UnityEngine;

public class EnterAnswerButtonController : MonoBehaviour, IOnWrongAnswer, IOnQuizAborted, IOnQuestionChanged, IOnGiveUp
{
    [SerializeField] UnityEngine.UI.Button button = null;

    bool IsHiding;
    bool IsShowing;

    public const float TransitionTime = 0.25F;

    public void OnQuizAborted()
    {
        Hide();
    }

    public void OnCorrectAnswer()
    {
        Hide();
    }

    public void OnWrongAnswer(bool wasNew)
    {
        button.interactable = false; // don't hide, just show the give up button on top
    }

    public void OnQuestionChanged(Question question)
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

    public void OnAnswerChanged(bool isAnswerEmpty)
    {
        button.interactable = !isAnswerEmpty;
    }

    public void OnGiveUp(Question question)
    {
        Hide();
    }

    void Show()
    {
        if (!IsShowing)
        {
            ScaleTo(Vector3.one);
            IsShowing = true;
        }
        IsHiding = false;
        button.interactable = true;
    }

    void Hide()
    {
        if (!IsHiding)
        {
            ScaleTo(Vector3.zero);
            IsHiding = true;
        }
        IsShowing = false;
        button.interactable = false;
    }

    void ScaleTo(Vector3 tgtScale)
    {
        iTween.ScaleTo(gameObject, iTween.Hash("scale", tgtScale, "easeType", iTween.EaseType.easeInSine, "time", TransitionTime));
    }
}
