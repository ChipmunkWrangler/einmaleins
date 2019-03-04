using CrazyChipmunk;
using UnityEngine;

class GotoLaunch : MonoBehaviour
{
    [SerializeField] private Prefs prefs;
    [SerializeField] private UnityEngine.UI.Button button;

    public void LoadLaunchScene(bool autolaunch)
    {
        button.enabled = false;
        prefs.SetBool("autolaunch", autolaunch);
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("launch");
    }
}
