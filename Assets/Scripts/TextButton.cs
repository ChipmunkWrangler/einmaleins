using UnityEngine;
using UnityEngine.UI;

internal class TextButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private float fadedAlpha = 0.5F;
    [SerializeField] private Text text;
    [SerializeField] private float transitionTime = 0.1F;
    private bool wasInteractable = true;

    public void SetActive(bool b)
    {
        gameObject.SetActive(b);
    }

    public void SetText(string newText)
    {
        GetComponentInChildren<Text>().text = newText;
    }

    private void Update()
    {
        if (wasInteractable != button.interactable)
        {
            wasInteractable = button.interactable;
            FadeTo(wasInteractable ? 1.0F : fadedAlpha);
        }
    }

    private void FadeTo(float tgtAlpha)
    {
        text.CrossFadeAlpha(tgtAlpha, transitionTime, false);
    }
}