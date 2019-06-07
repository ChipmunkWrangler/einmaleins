using CrazyChipmunk;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

internal class GotoLaunch : MonoBehaviour
{
    [SerializeField] private Selectable button;
    [SerializeField] private Prefs prefs;

    public void LoadLaunchScene(bool autolaunch)
    {
        button.enabled = false;
        prefs.SetBool("autolaunch", autolaunch);
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync("launch");
    }
}