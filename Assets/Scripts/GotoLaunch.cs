using CrazyChipmunk;
using UnityEngine;

class GotoLaunch : MonoBehaviour
{
    [SerializeField] Prefs prefs = null;
    [SerializeField] UnityEngine.UI.Button button = null;

    public void LoadLaunchScene(bool autolaunch)
    {
        button.enabled = false;
        prefs.SetBool("autolaunch", autolaunch);
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("launch");
    }
}
