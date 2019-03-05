using UnityEngine;
using UnityEngine.SceneManagement;

internal class GotoChoosePlayer : MonoBehaviour
{
    public void LoadChoosePlayerScene()
    {
        PlayerPrefs.Save();
        SceneManager.LoadSceneAsync("choosePlayer");
    }
}