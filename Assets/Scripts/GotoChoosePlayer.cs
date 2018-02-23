using UnityEngine;

class GotoChoosePlayer : MonoBehaviour
{
    public void LoadChoosePlayerScene()
    {
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("choosePlayer");
    }
}
