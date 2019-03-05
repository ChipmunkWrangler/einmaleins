using UnityEngine;
using UnityEngine.UI;

internal class VersionDisplay : MonoBehaviour
{
    [SerializeField] private Text text;

    private void Start()
    {
        text.text = Application.version;
    }
}