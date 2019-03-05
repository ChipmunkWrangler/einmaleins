using UnityEngine;
using UnityEngine.SceneManagement;

internal class GotoMain : MonoBehaviour
{
    public void LoadScene()
    {
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync("main");
    }
}