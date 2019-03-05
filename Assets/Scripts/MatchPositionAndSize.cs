using UnityEngine;

internal class MatchPositionAndSize : MonoBehaviour
{
    [SerializeField] private RectTransform source;
    [SerializeField] private RectTransform target;

    private void Update()
    {
        source.position = target.position;
        source.sizeDelta = target.sizeDelta;
    }
}