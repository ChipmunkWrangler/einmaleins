using UnityEngine;
using UnityEngine.UI;

internal class GiveUpOkButton : MonoBehaviour
{
    private const float TransitionTime = EnterAnswerButtonController.TransitionTime;

    [SerializeField] private Selectable button;

    public void OnGiveUp()
    {
        Show();
    }

    private void Hide()
    {
        button.enabled = false;
        ScaleTo(Vector3.zero);
    }

    private void Start()
    {
        Hide();
    }

    private void Show()
    {
        button.enabled = true;
        ScaleTo(Vector3.one);
    }

    private void ScaleTo(Vector3 tgtScale)
    {
        iTween.ScaleTo(button.gameObject,
            iTween.Hash("scale", tgtScale, "easeType", iTween.EaseType.easeInSine, "time", TransitionTime));
    }
}