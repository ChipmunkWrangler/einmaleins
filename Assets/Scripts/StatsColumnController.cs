using UnityEngine;
using UnityEngine.UI;

internal class StatsColumnController : MonoBehaviour
{
    [SerializeField] private Image[] cells;
    [SerializeField] private float fadeTime = 0.5F;
    [SerializeField] private Image header;
    [SerializeField] private Color highlightColor = Color.yellow;
    private bool isSomethingHighlighed;

    private int numMastered;
    [SerializeField] private Image rowHeader;

    public bool SetMasteryLevel(int row, Question q, bool seenMastered)
    {
        if (q.IsMastered())
        {
            numMastered++;
            if (seenMastered)
            {
                cells[row].CrossFadeAlpha(0, 0, false);
            }
            else
            {
                cells[row].color = highlightColor;
                cells[row].CrossFadeAlpha(0, fadeTime, false);
                isSomethingHighlighed = true;
            }

            seenMastered = true;
        }

        return seenMastered;
    }

    public void DoneSettingMasteryLevels()
    {
        if (numMastered >= QuestionGenerator.MaxMultiplicand)
        {
            var text = header.gameObject.GetComponentInChildren<Text>();
            var rowHeaderText = rowHeader.gameObject.GetComponentInChildren<Text>();
            if (isSomethingHighlighed)
            {
                header.color = highlightColor;
                header.CrossFadeAlpha(0, fadeTime, false);
                rowHeader.color = highlightColor;
                rowHeader.CrossFadeAlpha(0, fadeTime, false);
                text.CrossFadeAlpha(0, fadeTime, false);
                rowHeaderText.CrossFadeAlpha(0, fadeTime, false);
            }
            else
            {
                header.CrossFadeAlpha(0, 0, false);
                rowHeader.CrossFadeAlpha(0, 0, false);
                text.text = "";
                rowHeaderText.text = "";
            }
        }
    }
}