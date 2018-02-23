using UnityEngine;

class GotoBuildRocket : MonoBehaviour
{
    public void LoadScene()
    {
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("rocketBuilding");
    }
}
