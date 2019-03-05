using UnityEngine;
using UnityEngine.SceneManagement;

internal class GotoScene : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync(sceneName);
    }
}