using CrazyChipmunk;
using UnityEngine;

class GotoLaunch : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Button button = null;

    public void LoadLaunchScene(bool autolaunch)
    {
        button.enabled = false;
        Prefs.SetBool("autolaunch", autolaunch);
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("launch");
    }
}
