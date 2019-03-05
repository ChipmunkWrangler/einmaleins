using UnityEngine;
using UnityEngine.UI;

internal class GiveUpButtonController : MonoBehaviour, IOnWrongAnswer, IOnQuizAborted, IOnQuestionChanged
{
    private const float TransitionTime = EnterAnswerButtonController.TransitionTime;

    [SerializeField] private Button button;
    [SerializeField] private Image image;

    void IOnQuestionChanged.OnQuestionChanged(Question question)
    {
        SetInteractibility(question != null);
    }

    void IOnQuizAborted.OnQuizAborted()
    {
        SetInteractibility(false);
    }

    void IOnWrongAnswer.OnWrongAnswer(bool wasNew)
    {
        SetInteractibility(true);
    }

    public void OnGiveUp()
    {
        SetInteractibility(false);
    }

    private void OnCorrectAnswer()
    {
        SetInteractibility(false);
    }

    private void OnAnswerChanged(bool isAnswerEmpty)
    {
        SetInteractibility(isAnswerEmpty);
    }

    private void SetInteractibility(bool b)
    {
        if (button.interactable != b)
        {
            button.interactable = b;
            image.raycastTarget = b; // want to be able to tap the ok button, which is behind this one, immediately
            ScaleTo(b ? Vector3.one : Vector3.zero);
        }
    }

    private void ScaleTo(Vector3 tgtScale)
    {
        iTween.ScaleTo(gameObject,
            iTween.Hash("scale", tgtScale, "easeType", iTween.EaseType.easeInSine, "time", TransitionTime));
    }
}