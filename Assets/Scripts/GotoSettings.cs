using UnityEngine;

class GotoSettings : MonoBehaviour
{
    public void LoadSettingsScene()
    {
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("settings");
    }
}