using UnityEngine;

public class GotoMain : MonoBehaviour
{
    public void LoadScene()
    {
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("main");
    }
}
