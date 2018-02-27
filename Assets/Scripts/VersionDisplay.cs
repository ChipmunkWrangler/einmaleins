using UnityEngine;

class VersionDisplay : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text text = null;
    void Start()
    {
        text.text = Application.version;
    }
}
