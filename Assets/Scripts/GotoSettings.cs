using UnityEngine;
using UnityEngine.SceneManagement;

internal class GotoSettings : MonoBehaviour
{
    public void LoadSettingsScene()
    {
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync("settings");
    }
}