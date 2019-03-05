using System.Collections;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

internal class RocketPartCounter : MonoBehaviour, IOnQuestionChanged, IOnQuizAborted
{
    private Color[] baseColor;
    [SerializeField] private int baseFontSize;
    [SerializeField] private Color highlightColour = Color.yellow;
    [SerializeField] private float highlightFadeTime = 0.5F;
    [SerializeField] private int highlightFontSize;
    [SerializeField] private Image[] imagesToHighlight;
    [SerializeField] private Text numText;
    [SerializeField] private float scoreCountdownDelay = 0.5F;
    [SerializeField] private float scoreCountdownDuration = 5.0F;
    [SerializeField] private Text[] textsToFadeOut;
    [SerializeField] private Text[] textsToHighlight;

    void IOnQuestionChanged.OnQuestionChanged(Question question)
    {
        Unhighlight();
    }

    void IOnQuizAborted.OnQuizAborted()
    {
        Unhighlight();
    }

    public void Spend(int oldNumParts, int newNumParts)
    {
        StopAllCoroutines();
        if (gameObject.activeInHierarchy) StartCoroutine(CountTextDown(oldNumParts, newNumParts));
    }

    private void OnCorrectAnswer(Question question, bool isNewlyMastered)
    {
        if (isNewlyMastered)
        {
            RocketParts.Instance.Inc();
            foreach (var image in imagesToHighlight)
                StartCoroutine(FadeImage(highlightColour, highlightFadeTime, image));
            foreach (var text in textsToHighlight) StartCoroutine(FadeText(highlightColour, highlightFadeTime, text));
            foreach (var text in textsToHighlight) StartCoroutine(Scale(highlightFontSize, highlightFadeTime, text));
            UpdateText(RocketParts.Instance.NumParts);
        }
    }

    private void Awake()
    {
        baseColor = new Color[textsToHighlight.Length];
        var i = 0;
        foreach (var text in textsToHighlight) baseColor[i++] = text.color;
    }

    private void Start()
    {
        UpdateText(RocketParts.Instance.NumParts);
    }

    private void Unhighlight()
    {
        var i = 0;
        foreach (var text in textsToHighlight) StartCoroutine(FadeText(baseColor[i++], highlightFadeTime, text));
        foreach (var text in textsToHighlight) StartCoroutine(Scale(baseFontSize, highlightFadeTime, text));
    }

    private IEnumerator FadeText(Color end, float fadeTime, Text text)
    {
        var startColor = text.color;
        var startTime = Time.time;
        float t = 0;
        while (t <= 1.0F)
        {
            text.color = Color.Lerp(startColor, end, t);
            t = (Time.time - startTime) / fadeTime;
            yield return null;
        }
    }

    private IEnumerator Scale(int endFontSize, float tweenTime, Text text)
    {
        var startFontSize = text.fontSize;
        var startTime = Time.time;
        float t = 0;
        while (t <= 1.0F)
        {
            text.fontSize = Mathf.RoundToInt(Mathf.Lerp(startFontSize, endFontSize, t));
            t = (Time.time - startTime) / tweenTime;
            yield return null;
        }
    }

    private IEnumerator FadeImage(Color end, float fadeTime, Image image)
    {
        var startColor = image.color;
        var startTime = Time.time;
        float t = 0;
        while (t <= 1.0F)
        {
            image.color = Color.Lerp(startColor, end, t);
            t = (Time.time - startTime) / fadeTime;
            yield return null;
        }
    }

    private void UpdateText(int numParts)
    {
        if (RocketParts.Instance.UpgradeLevel >= RocketParts.Instance.MaxUpgradeLevel - 1 && numParts <= 0)
        {
            // the numParts check is for counting down following the final upgrade. -1 is because the final upgrade had hidden rocket parts
            if (numText.text.Length > 0)
            {
                numText.text = "";
                foreach (var text in textsToFadeOut) text.text = "";
            }
        }
        else
        {
            numText.text = LocalizationManager.GetTermTranslation("numRocketParts")
                .Replace("{[numParts]}", numParts.ToString()).Replace("{[numPartsRequired]}",
                    RocketParts.Instance.NumPartsRequired.ToString());
        }
    }

    private IEnumerator CountTextDown(int oldScore, int newScore)
    {
        yield return new WaitForSeconds(scoreCountdownDelay);
        var secsPerNum = Mathf.Abs(scoreCountdownDuration / (newScore - oldScore));
        for (var i = oldScore; i >= newScore; --i)
        {
            UpdateText(i);
            yield return new WaitForSeconds(secsPerNum);
        }

        UpdateText(newScore);
    }
}