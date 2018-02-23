﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class RocketPartCounter : MonoBehaviour, IOnQuestionChanged, IOnQuizAborted
{
    [SerializeField] Text numText = null;
    [SerializeField] Image[] imagesToHighlight = null;
    [SerializeField] Text[] textsToHighlight = null;
    [SerializeField] Text[] textsToFadeOut = null;
    [SerializeField] int baseFontSize = 0;
    [SerializeField] Color highlightColour = Color.yellow;
    [SerializeField] int highlightFontSize = 0;
    [SerializeField] float highlightFadeTime = 0.5F;
    [SerializeField] float scoreCountdownDuration = 5.0F;
    [SerializeField] float scoreCountdownDelay = 0.5F;

    Color[] baseColor;

    public void Spend(int oldNumParts, int newNumParts)
    {
        StopAllCoroutines();
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(CountTextDown(oldNumParts, newNumParts));
        }
    }

    void IOnQuestionChanged.OnQuestionChanged(Question question)
    {
        Unhighlight();
    }

    void IOnQuizAborted.OnQuizAborted()
    {
        Unhighlight();
    }

    void OnCorrectAnswer(Question question, bool isNewlyMastered)
    {
        if (isNewlyMastered)
        {
            RocketParts.Instance.Inc();
            foreach (Image image in imagesToHighlight)
            {
                StartCoroutine(FadeImage(highlightColour, highlightFadeTime, image));
            }
            foreach (Text text in textsToHighlight)
            {
                StartCoroutine(FadeText(highlightColour, highlightFadeTime, text));
            }
            foreach (Text text in textsToHighlight)
            {
                StartCoroutine(Scale(highlightFontSize, highlightFadeTime, text));
            }
            UpdateText(RocketParts.Instance.NumParts);
        }
    }

    void Awake()
    {
        baseColor = new Color[textsToHighlight.Length];
        int i = 0;
        foreach (Text text in textsToHighlight)
        {
            baseColor[i++] = text.color;
        }
    }

    void Start()
    {
        UpdateText(RocketParts.Instance.NumParts);
    }

    void Unhighlight()
    {
        int i = 0;
        foreach (Text text in textsToHighlight)
        {
            StartCoroutine(FadeText(baseColor[i++], highlightFadeTime, text));
        }
        foreach (Text text in textsToHighlight)
        {
            StartCoroutine(Scale(baseFontSize, highlightFadeTime, text));
        }
    }

    IEnumerator FadeText(Color end, float fadeTime, Text text)
    {
        Color startColor = text.color;
        float startTime = Time.time;
        float t = 0;
        while (t <= 1.0F)
        {
            text.color = Color.Lerp(startColor, end, t);
            t = (Time.time - startTime) / fadeTime;
            yield return null;
        }
    }

    IEnumerator Scale(int endFontSize, float tweenTime, Text text)
    {
        int startFontSize = text.fontSize;
        float startTime = Time.time;
        float t = 0;
        while (t <= 1.0F)
        {
            text.fontSize = Mathf.RoundToInt(Mathf.Lerp(startFontSize, endFontSize, t));
            t = (Time.time - startTime) / tweenTime;
            yield return null;
        }
    }

    IEnumerator FadeImage(Color end, float fadeTime, Image image)
    {
        Color startColor = image.color;
        float startTime = Time.time;
        float t = 0;
        while (t <= 1.0F)
        {
            image.color = Color.Lerp(startColor, end, t);
            t = (Time.time - startTime) / fadeTime;
            yield return null;
        }
    }

    void UpdateText(int numParts)
    {
        if (RocketParts.Instance.UpgradeLevel >= RocketParts.Instance.MaxUpgradeLevel - 1 && numParts <= 0)
        { // the numParts check is for counting down following the final upgrade. -1 is because the final upgrade had hidden rocket parts
            if (numText.text.Length > 0)
            {
                numText.text = "";
                foreach (Text text in textsToFadeOut)
                {
                    text.text = "";
                }
            }
        }
        else
        {
            numText.text = I2.Loc.LocalizationManager.GetTermTranslation("numRocketParts").Replace("{[numParts]}", numParts.ToString()).Replace("{[numPartsRequired]}", RocketParts.Instance.NumPartsRequired.ToString());
        }
    }

    IEnumerator CountTextDown(int oldScore, int newScore)
    {
        yield return new WaitForSeconds(scoreCountdownDelay);
        float secsPerNum = Mathf.Abs(scoreCountdownDuration / (float)(newScore - oldScore));
        for (int i = oldScore; i >= newScore; --i)
        {
            UpdateText(i);
            yield return new WaitForSeconds(secsPerNum);
        }
        UpdateText(newScore);
    }
}
